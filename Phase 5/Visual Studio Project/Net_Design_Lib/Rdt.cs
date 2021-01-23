using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.Timers;

namespace Net_Design_Lib
{
    /// <summary>
    /// Class which handles converting various types into packets and sending them via Udp.cs
    /// </summary>
    public class Rdt : IDisposable
    {
        Udp _udp = new Udp();
        PacketWindow packetWindow = new PacketWindow(20);

        uint currentSequenceNumber = 0;

        System.Timers.Timer timeoutClock = new System.Timers.Timer(50);

        EndPoint CurrentDestination;

        Stream tempFileWriter;
        string tempFilePath;
        private AutoResetEvent _packetSaved = new AutoResetEvent(false);

        //private int _bufferPos = 0;
        private byte[] _packetWriteBuffer = new byte[100000000];
        private List<Packet> _packetWriteBuffer2 = new List<Packet>();

        // Packet corruption chance and enable as well as the Random instance used to corrupt with a certain percent chance
        Random rand = new Random();

        NetDesign_UC_Lib.PacketErrorInfo packetErrorOptions;

        /// <summary>
        /// Sets the corruption options of Rdt
        /// </summary>
        /// <param name="dataPacketCorruptionEnabled"></param>
        /// <param name="dataPacketCorruptionChance"></param>
        /// <param name="ackPacketCorruptionEnabled"></param>
        /// <param name="ackPacketCorruptionChance"></param>
        public void SetCorruptionOptions(NetDesign_UC_Lib.PacketErrorInfo packetErrorOptions)
        {
            this.packetErrorOptions = packetErrorOptions;
        }

        public Rdt()
        {
            _udp.PacketReceived += Udp_PacketReceived;
            PacketReceived += Rdt_PacketReceived;
            timeoutClock.Elapsed += OnSendTimeout;
            timeoutClock.AutoReset = false;
        }

        public void SendFilesAsync(string[] filePaths, EndPoint destination, int packetSize = 1024)
        {
            Task.Run(() =>
            {
                var files = filePaths.ToList();
                foreach (string filePath in files)
                    SendFile(filePath, destination, packetSize);
            });
        }

        public void SendFile(string filePath, EndPoint destination, int packetSize = 1024)
        {
            CurrentDestination = destination;
            Stopwatch sw = Stopwatch.StartNew();

            double currentProgress = 0;
            double currentProgressLowRes = 0;

            lock (packetWindow)
            {
                packetWindow.Enqueue(new Packet(currentSequenceNumber, Encoding.ASCII.GetBytes(Path.GetFileName(filePath)), Packet.PacketType.FileStart));
                _udp.SendPacket(packetWindow.Last(), destination);
                currentSequenceNumber++;
            }

            double packetNumber = 1;

            FileInfo fi = new FileInfo(filePath);
            int requiredPackets = (int)Math.Ceiling((double)fi.Length / (packetSize - Packet.headerSize));

            using (Stream source = File.OpenRead(filePath))
            {
                byte[] streamBuffer = new byte[packetSize - Packet.headerSize];
                int bytesRead = 0;

                // While there are bytes to send
                do
                {
                    //if (packetWindow.Count == 0)
                    //    bytesRead = 0;
                    // Shifting window send
                    if (currentSequenceNumber < packetWindow.EndOfWindow)
                    {
                        //Console.WriteLine($"Window Containes Packets {packetWindow.Base} through {packetWindow.EndOfWindow}");
                        bytesRead = source.Read(streamBuffer, 0, streamBuffer.Length);
                        lock (packetWindow)
                        {
                            packetWindow.Enqueue(new Packet(currentSequenceNumber, streamBuffer.Take(bytesRead).ToArray()));
                            _udp.SendPacket(packetWindow.Last(), destination);
                            currentSequenceNumber++;
                        }
                        if (packetWindow.Base == currentSequenceNumber)
                        {
                            timeoutClock.Stop();
                            timeoutClock.Start();
                        }

                        // Update UI max of 100 times
                        currentProgress = packetNumber / requiredPackets;
                        packetNumber++;
                        if (currentProgress >= currentProgressLowRes + .01)
                        {
                            currentProgressLowRes = currentProgress;
                            SendProgressChanged?.Invoke(currentProgress);
                        }
                    }
                } while (bytesRead > 0);
            }

            // Makes sure the UI hits 100%
            SendProgressChanged?.Invoke(1);

            // Sends the fileEnd packet
            lock (packetWindow)
            {
                packetWindow.Enqueue(new Packet(currentSequenceNumber, Encoding.ASCII.GetBytes(Path.GetFileName(filePath)), Packet.PacketType.FileEnd));
                _udp.SendPacket(packetWindow.Last(), destination);
                currentSequenceNumber++;
            }

            timeoutClock.Stop();
            sw.Stop();
            FileTransferComplete?.Invoke(fi.Length, sw.Elapsed);

            currentSequenceNumber = 0;
        }

        public void BeginReceive(ushort port)
        {
            _udp.BeginReceive(port);
        }

        private void Udp_PacketReceived(Packet packet, EndPoint senderEndPoint, EndPoint receiverEndPoint)
        {
            // Handle incomming ack
            if (packet.TypeByte == Packet.PacketType.Ack)
            {
                // Corrupt data packet at receiver
                if (packetErrorOptions.AckCorruptionEnabled && PercentToBoolRand(packetErrorOptions.AckCorruptionChance))
                    packet = packet.AsCorrupt();

                // Drop data packet at receiver
                if (packetErrorOptions.AckDropEnabled && PercentToBoolRand(packetErrorOptions.AckDropChance))
                    return;

                if (packet != null && packet.IsValidChecksum())
                {
                    lock (packetWindow)
                    {
                        // Ack whatever packet came in
                        Packet p = packetWindow.FirstOrDefault(pac => pac.SequenceNumber == packet.SequenceNumber);
                        if (p != null)
                            p.HasBeenAcked = true;

                        //Dequeue any acked packets at the beginning
                        while (packetWindow.Count > 0 && packetWindow.First().HasBeenAcked)
                            packetWindow.Base = packetWindow.Dequeue().SequenceNumber;

                        if (packetWindow.Base == currentSequenceNumber)
                            timeoutClock.Stop();
                        else
                        {
                            timeoutClock.Stop();
                            timeoutClock.Start();
                        }
                    }
                }

                return;
            }

            // Corrupt data packet at receiver
            if (packetErrorOptions.DataCorruptionEnabled && PercentToBoolRand(packetErrorOptions.DataCorruptionChance))
                packet = packet.AsCorrupt();

            // Drop data packet at receiver
            if (packetErrorOptions.DataDropEnabled && PercentToBoolRand(packetErrorOptions.DataDropChance))
                return;

            // Check incomming data packet
            if (packet != null && packet.IsValidChecksum() /*&& packet.SequenceNumber == (currentSequenceNumber + 1)*/)
            {
                // Notify packet received
                PacketReceived?.Invoke(packet, senderEndPoint, receiverEndPoint);

                // Waits for filestream
                //if (!_packetSaved.WaitOne(50))
                //    StatusChanged?.Invoke("Missed writing one byte");

                // Create and send ack
                _udp.SendPacket(new Packet(packet.SequenceNumber, null, Packet.PacketType.Ack), senderEndPoint);
            }
        }

        private bool HasFileTransferStarted = false;

        private void Rdt_PacketReceived(Packet packet, EndPoint senderEndPoint, EndPoint receiverEndPoint)
        {
            if (packet.TypeByte == Packet.PacketType.Data)
                _packetWriteBuffer2.Add(packet);
            else if (packet.TypeByte == Packet.PacketType.FileStart)
            {
                StatusChanged?.Invoke($"Receiving \"{Encoding.ASCII.GetString(packet.GetNonHeaderData())}\"");
                HasFileTransferStarted = true;
            }
            else if (packet.TypeByte == Packet.PacketType.FileEnd && HasFileTransferStarted)
            {
                HasFileTransferStarted = false;
                tempFilePath = Path.GetTempFileName();
                tempFileWriter = File.OpenWrite(tempFilePath);

                var data = _packetWriteBuffer2.OrderBy(p => p.SequenceNumber).Distinct().ToList();

                foreach (Packet p in data)
                    tempFileWriter.Write(p.GetNonHeaderData(), 0, p.GetNonHeaderData().Length);

                tempFileWriter.Flush();
                tempFileWriter.Close();
                _packetWriteBuffer2.Clear();

                string fileName = Encoding.ASCII.GetString(packet.GetNonHeaderData());

                //_packetSaved.Set();
                FileReceived?.Invoke(new FileInfo(tempFilePath), fileName);

                currentSequenceNumber = 0;
            }
        }

        private void OnSendTimeout(Object source, ElapsedEventArgs e)
        {
            timeoutClock.Stop();
            timeoutClock.Start();
            lock (packetWindow)
            {
                foreach (Packet p in packetWindow)
                    _udp.SendPacket(p, CurrentDestination);
            }
        }

        public delegate void PacketReceivedDelegate(Packet packet, EndPoint senderEndPoint, EndPoint receiverEndPoint);
        public event PacketReceivedDelegate PacketReceived;

        // Fires when a file is received
        public delegate void FileReceivedDelegate(FileInfo tempFileInfo, string fileName);
        public event FileReceivedDelegate FileReceived;

        // Fires when a file transfer completes
        public delegate void FileTransferCompleteDelegate(long numBytes, TimeSpan timeToCompletion);
        public event FileTransferCompleteDelegate FileTransferComplete;

        // Fires when the status changes
        public delegate void StatusChangedDelegate(string status);
        public event StatusChangedDelegate StatusChanged;

        // Updates UI with send progress
        public delegate void SendProgressChangedDelegate(double percentComplete);
        public event SendProgressChangedDelegate SendProgressChanged;

        /// <summary>
        /// Returns true or false depending on percentChance
        /// (higher percentChance -> more likely to return true)
        /// </summary>
        /// <param name="percentChance"></param>
        /// <returns></returns>
        private bool PercentToBoolRand(double percentChance)
        {
            double randValue = rand.NextDouble();

            return randValue < percentChance;
        }

        public void Dispose()
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }
    }
}
