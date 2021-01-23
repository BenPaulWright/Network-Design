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
    public class Rdt
    {
        #region Private variables

        // Instance of the Udp class used to send and receive raw data
        private Udp _udpInstance = new Udp();

        // Stores the incomming data packets
        private List<Packet> packetsBuffer = new List<Packet>();

        // Holds the current sequence number
        private byte currentSequenceNumber = 0;

        // Used to measure the ammount of time spent sending a file
        private Stopwatch transferTimeTimer = new Stopwatch();

        // Used to notify the Send() of a receied ack packet
        private ManualResetEventSlim receivedAckResetEvent = new ManualResetEventSlim();

        // Used to close the socket
        private CancellationTokenSource closeSocketTokenSource = new CancellationTokenSource();

        // Stores the last received packet
        private volatile Packet packet;

        // Packet corruption chance and enable as well as the Random instance used to corrupt with a certain percent chance
        Random rand = new Random();
        private double dataPacketCorruptionChance = 0;
        private bool dataPacketCorruptionEnabled = false;
        private double ackPacketCorruptionChance = 0;
        private bool ackPacketCorruptionEnabled = false;

        #endregion

        #region Send

        /// <summary>
        /// Sends a single packet with ack checking
        /// </summary>
        /// <param name="packet"></param>
        private void SendPacket(Packet packet, EndPoint remoteEndPoint)
        {
            Packet confirmationPacket;
            while (!closeSocketTokenSource.IsCancellationRequested)
            {
                // Sends packet
                receivedAckResetEvent.Reset();

                // Corrupt Data
                if (dataPacketCorruptionEnabled && PercentToBoolRand(dataPacketCorruptionChance))
                {
                    Packet corruptPacket = packet;
                    corruptPacket.Raw[Packet.headerSize] = (byte)~corruptPacket.Raw[Packet.headerSize];
                    _udpInstance.SendPacket(corruptPacket, remoteEndPoint);
                }
                else
                    _udpInstance.SendPacket(packet, remoteEndPoint);

                // Wait for ack for a max of one second
                if (receivedAckResetEvent.Wait(TimeSpan.FromSeconds(1)))
                    confirmationPacket = this.packet;
                else
                    continue;

                // If the packet is not valid, goto top
                if (confirmationPacket == null || !confirmationPacket.IsValidChecksum() || confirmationPacket.SequenceNumber != packet.SequenceNumber)
                    continue;

                // If all three work, exit while loop
                break;
            }

            // Increments currentSequenceNumber with overflow wrap
            unchecked
            {
                currentSequenceNumber++;
            }
        }

        /// <summary>
        /// Breaks a byte array into packets of size <packetSize> and sends them
        /// </summary>
        /// <param name="data"></param>
        /// <param name="packetSize"></param>
        private void Send(byte[] data, EndPoint remoteEndPoint, int packetSize = 1024)
        {
            // Converts data to packets starting with the current sequence number
            var packets = PacketHandler.ToPackets(data, packetSize, currentSequenceNumber);
            double currentProgress = 0;
            double currentProgressLowRes = 0;

            for (int packetIndex = 0; packetIndex < packets.Length; packetIndex++)
            {
                SendPacket(packets[packetIndex], remoteEndPoint);

                // Update UI of send percentage
                // If statement makes sure the UI is only updated 100 times, not each time a packet is sent (this could slow down the ui drastically)

                currentProgress = ((double)packetIndex + 1) / (double)packets.Length;

                if (currentProgress >= currentProgressLowRes + .01)
                {
                    currentProgressLowRes = currentProgress;
                    SendProgressChanged?.Invoke(currentProgressLowRes);
                }
            }
        }

        /// <summary>
        /// Reads in a file, breaks it into packets, and sequentially sends them to the given endpoint
        /// </summary>
        /// <param name="data"></param>
        /// <param name="packetSize"></param>
        /// <param name="destination"></param>
        public void SendFile(string filePath, EndPoint remoteEndPoint, int packetSize = 1024)
        {
            Task.Run(() =>
            {
                try
                {
                    // Starts timer
                    transferTimeTimer.Restart();

                    StatusChanged?.Invoke($"Sending {Path.GetFileName(filePath)}");

                    // Sends a file start packet
                    Packet FileStartPacket = new Packet(currentSequenceNumber, null, Packet.PacketType.FileStart);
                    SendPacket(FileStartPacket, remoteEndPoint);

                    // Converts the file into bytes and calls the send function
                    var fileData = File.ReadAllBytes(filePath);
                    Send(fileData, remoteEndPoint, packetSize);

                    // Sends a file end packet
                    Packet FileEndPacket = new Packet(currentSequenceNumber, null, Packet.PacketType.FileEnd);
                    SendPacket(FileEndPacket, remoteEndPoint);

                    // Stops timer
                    transferTimeTimer.Stop();
                    FileTransferComplete?.Invoke(fileData.Length, transferTimeTimer.Elapsed);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    RdtExceptionThrown?.Invoke(ex);
                }
            });
        }


        #endregion

        #region Receive

        /// <summary>
        /// Receives a single packet and sends an ack
        /// </summary>
        /// <param name="expectedSequenceNumber"></param>
        /// <returns></returns>
        private Packet ReceivePacket(byte lastGoodSequenceNumber, EndPoint remoteEndPoint)
        {
            while (!closeSocketTokenSource.IsCancellationRequested)
            {
                // Wait for packet
                packet = _udpInstance.ReceivePacket();

                // Checks if packet is an Ack
                if (packet.TypeByte == Packet.PacketType.Ack)
                {
                    // Corrupt ack
                    if (ackPacketCorruptionEnabled && PercentToBoolRand(ackPacketCorruptionChance))
                        packet.Raw[Packet.headerSize] = (byte)~packet.Raw[Packet.headerSize];

                    // Unblock the sender thread
                    receivedAckResetEvent.Set();

                    continue;
                }

                // Calculates the expected sequence number (with wrap)
                byte expectedSequenceNumber = lastGoodSequenceNumber;
                unchecked
                {
                    expectedSequenceNumber++;
                }

                // If invalid, send Ack for last good packet then goto top
                if (packet == null || !packet.IsValidChecksum() || packet.SequenceNumber != expectedSequenceNumber)
                {
                    _udpInstance.SendAck(lastGoodSequenceNumber, remoteEndPoint);
                    continue;
                }

                // If all three work, send Ack and exit while loop
                _udpInstance.SendAck(packet.SequenceNumber, remoteEndPoint);
                break;
            }

            return packet;
        }

        /// <summary>
        /// Tells the socket where to listen for a recievePacket function call
        /// </summary>
        /// <param name="localEndPoint"></param>
        public void BindSocket(EndPoint localEndPoint)
        {
            _udpInstance.Dispose();
            _udpInstance = new Udp();
            _udpInstance.BindSocket(localEndPoint);
        }

        /// <summary>
        /// Opens a socket on which packets are received
        /// </summary>
        public void OpenSocket(EndPoint remoteEndPoint)
        {
            Task.Run(() =>
            {
                try
                {
                    byte lastGoodSequenceNumber = 255;

                    closeSocketTokenSource = new CancellationTokenSource();
                    while (!closeSocketTokenSource.IsCancellationRequested)
                    {
                        // Wait for packet
                        Packet packet = ReceivePacket(lastGoodSequenceNumber, remoteEndPoint);

                        lastGoodSequenceNumber = packet.SequenceNumber;

                        // Check type
                        if (packet.TypeByte == Packet.PacketType.Data)
                            packetsBuffer.Add(packet);
                        else if (packet.TypeByte == Packet.PacketType.FileStart)
                            ;
                        else if (packet.TypeByte == Packet.PacketType.FileEnd)
                        {
                            byte[] fileData = PacketHandler.FromPackets(packetsBuffer.ToArray());
                            packetsBuffer.Clear();
                            StatusChanged?.Invoke($"Received {fileData.Length} bytes");
                            FileReceived?.Invoke(fileData);
                        }
                        else if (packet.TypeByte == Packet.PacketType.StringStart)
                            ;
                        else if (packet.TypeByte == Packet.PacketType.StringEnd)
                        {
                            string receivedString = PacketHandler.PacketsToString(packetsBuffer.ToArray());
                            packetsBuffer.Clear();
                            StringReceived?.Invoke(receivedString);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    RdtExceptionThrown?.Invoke(ex);
                }
            });
        }

        /// <summary>
        /// Closes the socket and releases its' resources
        /// </summary>
        public void CloseSocket()
        {
            closeSocketTokenSource.Cancel();
            _udpInstance.Dispose();
        }

        #endregion

        public enum RdtErrors
        {
            AckWasNull,
            AckChecksumFailed,
            AckWasNegative,
            AckSequenceNumMismatch,

            PacketWasNull,
            PacketChecksumFailed,
            PacketAckTimedOut,
            DuplicatePacketReceived,
        }

        /// <summary>
        /// Sets the corruption options of Rdt
        /// </summary>
        /// <param name="dataPacketCorruptionEnabled"></param>
        /// <param name="dataPacketCorruptionChance"></param>
        /// <param name="ackPacketCorruptionEnabled"></param>
        /// <param name="ackPacketCorruptionChance"></param>
        public void SetCorruptionOptions(bool dataPacketCorruptionEnabled, double dataPacketCorruptionChance, bool ackPacketCorruptionEnabled, double ackPacketCorruptionChance)
        {
            this.dataPacketCorruptionEnabled = dataPacketCorruptionEnabled;
            this.dataPacketCorruptionChance = dataPacketCorruptionChance;
            this.ackPacketCorruptionEnabled = ackPacketCorruptionEnabled;
            this.ackPacketCorruptionChance = ackPacketCorruptionChance;
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

        #region Events

        // Fires when a file is received
        public delegate void FileReceivedDelegate(byte[] fileData);
        public event FileReceivedDelegate FileReceived;

        // Fires when a file transfer completes
        public delegate void FileTransferCompleteDelegate(int numBytes, TimeSpan timeToCompletion);
        public event FileTransferCompleteDelegate FileTransferComplete;

        // Fires when a string is received
        public delegate void StringReceivedDelegate(string str);
        public event StringReceivedDelegate StringReceived;

        // Fires when the status changes
        public delegate void StatusChangedDelegate(string status);
        public event StatusChangedDelegate StatusChanged;

        // Fires when a packet error occures
        public delegate void RdtErrorDelegate(RdtErrors error, Packet packet);
        public event RdtErrorDelegate RdtErrorThrown;

        // Fires when a try catch fails
        public delegate void RdtExceptionDelegate(Exception exception);
        public event RdtExceptionDelegate RdtExceptionThrown;

        // Updates UI with send progress
        public delegate void SendProgressChangedDelegate(double percentComplete);
        public event SendProgressChangedDelegate SendProgressChanged;

        #endregion
    }
}
