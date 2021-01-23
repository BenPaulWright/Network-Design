using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Microsoft.Win32;

namespace Net_Design_Lib
{
    /// <summary>
    /// Class which handles converting various types into packets and sending them via Udp.cs
    /// </summary>
    public class Tcp
    {
        // Instance of the Udp class used to send and receive raw data
        Udp _instance = new Udp();

        /// <summary>
        /// Breaks the given byte array into packets and sequentially sends them to the given endpoint
        /// </summary>
        /// <param name="data"></param>
        /// <param name="packetSize"></param>
        /// <param name="destination"></param>
        public void Send(byte[] data, uint packetSize, EndPoint destination)
        {
            try
            {
                var packets = PacketHandler.ToPackets(data, packetSize);

                foreach (var packet in packets)
                    _instance.SendPacket(packet, destination);
            }
            catch (Exception ex)
            {
                TcpError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Reads in a file, breaks it into packets, and sequentially sends them to the given endpoint
        /// </summary>
        /// <param name="data"></param>
        /// <param name="packetSize"></param>
        /// <param name="destination"></param>
        public void SendFile(string filePath, uint packetSize, EndPoint destination)
        {
            try
            {
                // Converts the file into bytes and calls the send function
                Send(File.ReadAllBytes(filePath), packetSize, destination);
            }
            catch (Exception ex)
            {
                TcpError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Encodes a string as a byte array, breaks it into packets, and sequentially sends them to the given endpoint
        /// </summary>
        /// <param name="message"></param>
        /// <param name="packetSize"></param>
        /// <param name="destination"></param>
        public void SendMessage(string message, uint packetSize, EndPoint destination)
        {
            try
            {
                // Converts the string into bytes and calls the send function
                Send(Encoding.ASCII.GetBytes(message), packetSize, destination);
            }
            catch (Exception ex)
            {
                TcpError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Begins a packet listener targeting the given endpoint
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        public void BeginReceive(EndPoint remoteEndPoint)
        {
            try
            {
                _instance.BeginReceivePackets(remoteEndPoint);
                _instance.PacketReceived += OnPacketReceived;
            }
            catch (Exception ex)
            {
                TcpError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Closes the packet listener
        /// </summary>
        public void EndReceive()
        {
            try
            {
                _instance.PacketReceived -= OnPacketReceived;
            }
            catch (Exception ex)
            {
                TcpError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Fires when a packet is received - Processes packets
        /// </summary>
        /// <param name="packet"></param>
        private void OnPacketReceived(Packet packet)
        {
            try
            {
                PacketReceived?.Invoke(packet);
                if (packet.PacketNumber + 1 != packet.TotalPackets)
                    tempPackets.Add(packet);
                else
                {
                    var fileData = PacketHandler.FromPackets(tempPackets.ToArray());

                    FileReceived?.Invoke(fileData);

                    tempPackets.Clear();
                }
            }
            catch (Exception ex)
            {
                TcpError?.Invoke(ex);
            }
        }

        // Holds the incoming packets until they're ready to be processed
        private List<Packet> tempPackets = new List<Packet>();

        // Fires when a packet is received
        public delegate void PacketReceivedDelegate(Packet packet);
        public event PacketReceivedDelegate PacketReceived;

        // Fires when a set of packets is ready to be processed
        public delegate void FileReceivedDelegate(byte[] fileData);
        public event FileReceivedDelegate FileReceived;

        // Fires when a try catch fails
        public delegate void TcpErrorDelegate(Exception exception);
        public event TcpErrorDelegate TcpError;
    }
}
