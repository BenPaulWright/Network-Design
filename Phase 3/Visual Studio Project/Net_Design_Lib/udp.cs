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
    public class Udp : IDisposable
    {
        // Instance of the socket class
        private Socket _socketInstance = new Socket(SocketType.Dgram, ProtocolType.Udp);

        // Receive buffer for the socket instance
        private byte[] _receiveBuffer = new byte[10000];

        public void BindSocket(EndPoint localEndPoint)
        {
            _socketInstance.Bind(localEndPoint);
        }
        /// <summary>
        /// Sends a single packet to the set endpoint
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="destination"></param>
        public void SendPacket(Packet packet, EndPoint remoteEndPoint)
        {
            try
            {
                if (remoteEndPoint == null)
                    UdpExceptionThrown?.Invoke(new NullReferenceException("remoteEndPoint was null"));
                else
                    _socketInstance.SendTo(packet.Raw, remoteEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                UdpExceptionThrown?.Invoke(ex);
            }
        }

        /// <summary>
        /// Waits for a single packet from the set endpoint
        /// </summary>
        /// <returns></returns>
        public Packet ReceivePacket()
        {
            try
            {
                //Max buff size of 10KB
                int bytes = _socketInstance.Receive(_receiveBuffer);
                return new Packet(_receiveBuffer.Take(bytes).ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                UdpExceptionThrown?.Invoke(ex);
                return null;
            }
        }

        /// <summary>
        /// Sends empty Ack packet
        /// </summary>
        /// <param name="sequenceNumber"></param>
        public void SendAck(byte sequenceNumber, EndPoint remoteEndPoint)
        {
            SendPacket(new Packet(sequenceNumber, null, Packet.PacketType.Ack), remoteEndPoint);
        }

        /// <summary>
        /// Allows Udp to implement IDisposable
        /// </summary>
        public void Dispose()
        {
            _socketInstance.Close();
            _socketInstance.Dispose();
        }

        // Fires when a try catch fails
        public delegate void UdpExceptionThrownDelegate(Exception exception);
        public event UdpExceptionThrownDelegate UdpExceptionThrown;
    }
}