using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Net_Design_Lib
{
    /// <summary>
    /// Class which handles low level socket communication
    /// </summary>
    public class Udp
    {
        private Socket _udpSocket;
        private byte[] _receiveBuffer = new byte[10000];

        public void BeginReceive(ushort port)
        {
            try
            {
                // Resets the socket
                if (_udpSocket != null)
                    _udpSocket.Dispose();
                _udpSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
                _udpSocket.DualMode = true;

                // Binds the socket to a specified port
                _udpSocket.Bind(new IPEndPoint(IPAddress.Any, (int)port));

                // Creates a new empty endpoint
                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Waits for a packet asynchronously
                _udpSocket.BeginReceiveFrom(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ref senderEndPoint, PacketReceivedCallback, _udpSocket);
            }
            catch (Exception ex)
            {
                ExceptionThrown?.Invoke(this, ex);
            }
        }

        private void PacketReceivedCallback(IAsyncResult result)
        {
            try
            {
                // Gets the passed socket
                Socket refSocket = (Socket)result.AsyncState;

                // Gets the sender's endpoint
                EndPoint dataSource = new IPEndPoint(IPAddress.Any, 0);

                // Gets the number of transfered bytes
                int numReceivedBytes = refSocket.EndReceiveFrom(result, ref dataSource);

                // Gets the packet from the buffer
                Packet receivedPacket = new Packet(_receiveBuffer.Take(numReceivedBytes).ToArray());

                // Creates a new empty endpoint
                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Waits for a packet asynchronously
                _udpSocket.BeginReceiveFrom(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ref senderEndPoint, PacketReceivedCallback, _udpSocket);

                // Passes the packet up the chain
                PacketReceived?.Invoke(receivedPacket, dataSource, refSocket.LocalEndPoint);
            }
            catch (Exception ex)
            {
                ExceptionThrown?.Invoke(this, ex);
            }
        }

        public void SendPacket(Packet packet, EndPoint destination)
        {
            try
            {
                _udpSocket.SendTo(packet.Raw, destination);
            }
            catch (Exception ex)
            {
                ExceptionThrown?.Invoke(this, ex);
            }
        }

        public delegate void PacketReceivedDelegate(Packet packet, EndPoint senderEndPoint, EndPoint receiverEndPoint);
        public event PacketReceivedDelegate PacketReceived;

        public delegate void ExceptionThrownDelegate(object sender, Exception ex);
        public event ExceptionThrownDelegate ExceptionThrown;
    }
}