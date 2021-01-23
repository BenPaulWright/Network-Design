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

namespace Net_Design_Lib
{
    /// <summary>
    /// Class which handles converting various types into packets and sending them via Udp.cs
    /// </summary>
    public class Rdt : IDisposable
    {
        Udp _udp = new Udp();

        byte lastGoodReceivedPacketSequenceNumber = 255;
        byte currentSequenceNumber = 0;

        private AutoResetEvent _ackRecived = new AutoResetEvent(false);
        private Packet _ackPacket = new Packet(0, null);

        Stream tempFileWriter;
        string tempFilePath;
        private AutoResetEvent _packetSaved = new AutoResetEvent(false);

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
            Stopwatch sw = Stopwatch.StartNew();

            double currentProgress = 0;
            double currentProgressLowRes = 0;

            SendDataPacket(new Packet(currentSequenceNumber, Encoding.ASCII.GetBytes(Path.GetFileName(filePath)), Packet.PacketType.FileStart), destination);
            Increment(ref currentSequenceNumber);

            double packetNumber = 1;

            FileInfo fi = new FileInfo(filePath);
            int requiredPackets = (int)Math.Ceiling((double)fi.Length / (packetSize - Packet.headerSize));

            using (Stream source = File.OpenRead(filePath))
            {
                byte[] streamBuffer = new byte[packetSize - Packet.headerSize];
                int bytesRead;

                while ((bytesRead = source.Read(streamBuffer, 0, streamBuffer.Length)) > 0)
                {
                    Packet packet = new Packet(currentSequenceNumber, streamBuffer.Take(bytesRead).ToArray());
                    SendDataPacket(packet, destination);
                    Increment(ref currentSequenceNumber);

                    // Update UI max of 100 times
                    currentProgress = packetNumber / requiredPackets;
                    packetNumber++;
                    if (currentProgress >= currentProgressLowRes + .01)
                    {
                        currentProgressLowRes = currentProgress;
                        SendProgressChanged?.Invoke(currentProgress);
                    }
                }
            }

            // Makes sure the UI hits 100%
            SendProgressChanged?.Invoke(1);

            // Sends the fileEnd packet
            SendDataPacket(new Packet(currentSequenceNumber, Encoding.ASCII.GetBytes(Path.GetFileName(filePath)), Packet.PacketType.FileEnd), destination);
            Increment(ref currentSequenceNumber);

            sw.Stop();
            FileTransferComplete?.Invoke(fi.Length, sw.Elapsed);
        }

        private void SendDataPacket(Packet packet, EndPoint destination)
        {
            while (true)
            {
                _udp.SendPacket(packet, destination);
                Console.WriteLine($"Sent packet {packet.SequenceNumber}");

                if (!_ackRecived.WaitOne(30))
                    continue;

                lock (_ackPacket)
                {
                    // Corrupt ack packet at sender
                    if (packetErrorOptions.AckCorruptionEnabled && PercentToBoolRand(packetErrorOptions.AckCorruptionChance))
                        _ackPacket = _ackPacket.AsCorrupt();

                    // Drop ack packet at sender
                    if (packetErrorOptions.AckDropEnabled && PercentToBoolRand(packetErrorOptions.AckDropChance))
                        continue;

                    // Check ack
                    if (_ackPacket == null || !_ackPacket.IsValidChecksum() || _ackPacket.SequenceNumber != packet.SequenceNumber)
                        continue;
                }

                break;
            }
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
                lock (_ackPacket)
                {
                    _ackPacket = packet;
                }

                _ackRecived.Set();

                return;
            }

            // Corrupt data packet at receiver
            if (packetErrorOptions.DataCorruptionEnabled && PercentToBoolRand(packetErrorOptions.DataCorruptionChance))
                packet = packet.AsCorrupt();

            // Drop data packet at receiver
            if (packetErrorOptions.DataDropEnabled && PercentToBoolRand(packetErrorOptions.DataDropChance))
                return;

            // Check incomming data packet
            if (packet != null && packet.IsValidChecksum() && packet.SequenceNumber == GetNext(lastGoodReceivedPacketSequenceNumber))
            {
                // Update lastGoodReceivedPacketSequenceNumber
                lastGoodReceivedPacketSequenceNumber = packet.SequenceNumber;

                // Notify packet received
                PacketReceived?.Invoke(packet, senderEndPoint, receiverEndPoint);

                // Waits for filestream
                if (!_packetSaved.WaitOne(50))
                    StatusChanged?.Invoke("Missed writing one byte");
            }

            // Create and send ack
            _udp.SendPacket(new Packet(lastGoodReceivedPacketSequenceNumber, null, Packet.PacketType.Ack), senderEndPoint);
        }

        private void Rdt_PacketReceived(Packet packet, EndPoint senderEndPoint, EndPoint receiverEndPoint)
        {
            if (packet.TypeByte == Packet.PacketType.Data)
            {
                byte[] writeBuffer = packet.GetNonHeaderData();
                tempFileWriter.Write(writeBuffer, 0, writeBuffer.Length);
                _packetSaved.Set();
            }
            else if (packet.TypeByte == Packet.PacketType.FileStart)
            {
                StatusChanged?.Invoke($"Receiving {Encoding.ASCII.GetString(packet.GetNonHeaderData())}");
                tempFilePath = Path.GetTempFileName();
                tempFileWriter = File.OpenWrite(tempFilePath);
                _packetSaved.Set();
            }
            else if (packet.TypeByte == Packet.PacketType.FileEnd)
            {
                tempFileWriter.Flush();
                tempFileWriter.Close();

                string fileName = Encoding.ASCII.GetString(packet.GetNonHeaderData());

                _packetSaved.Set();
                FileReceived?.Invoke(new FileInfo(tempFilePath), fileName);
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

        private byte GetNext(byte sequenceNumber)
        {
            unchecked
            {
                sequenceNumber++;
            }
            return sequenceNumber;
        }

        private void Increment(ref byte sequenceNumber)
        {
            unchecked
            {
                sequenceNumber++;
            }
        }

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
