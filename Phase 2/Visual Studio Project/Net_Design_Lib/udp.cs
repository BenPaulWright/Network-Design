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
        /// <summary>
        /// Sends a single packet to the given endpoint
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="destination"></param>
        public void SendPacket(Packet packet, EndPoint destination)
        {
            try
            {
                using (var _soc = new Socket(SocketType.Dgram, ProtocolType.Udp))
                {
                    _soc.SendTo(packet.Raw, destination);
                }
            }
            catch (Exception ex)
            {
                UdpError?.Invoke(ex);
            }
        }

        // Class which stores an endpoint, buffer, and socket to be sent within IAsyncResult to the OnPacketReceived function
        private class StateObject
        {
            public StateObject(EndPoint remoteEndpoint, byte[] buffer, Socket socket)
            {
                this.RemoteEndpoint = remoteEndpoint;
                this.Buffer = buffer;
                this.Socket = socket;
            }
            public EndPoint RemoteEndpoint;
            public byte[] Buffer;
            public Socket Socket;
        }

        // Socket instance for receiving
        private Socket _soc = new Socket(SocketType.Dgram, ProtocolType.Udp);

        /// <summary>
        /// Begins receiving packets asynchronously
        /// </summary>
        /// <param name="source"></param>
        public void BeginReceivePackets(EndPoint source)
        {
            try
            {
                _soc.Dispose();
                _soc = new Socket(SocketType.Dgram, ProtocolType.Udp);
                var _buff = new byte[10000];
                _soc.Bind(source);
                _soc.BeginReceiveFrom(_buff,
                                       0,
                                       _buff.Length,
                                       SocketFlags.None,
                                       ref source,
                                       OnPacketReceived,
                                       new StateObject(source, _buff, _soc));
            }
            catch (Exception ex)
            {
                UdpError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Async callback when a packet is received
        /// </summary>
        /// <param name="result"></param>
        private void OnPacketReceived(IAsyncResult result)
        {
            try
            {
                // Recalls the state
                var _state = result.AsyncState as StateObject;

                // Grabs the numbber of bytes received and ends the receive
                var bytes = _state.Socket.EndReceiveFrom(result, ref _state.RemoteEndpoint);

                // Creates a temp byte array to put the received data
                var packetData = new byte[bytes];

                // Coppies the buffer passed in the stateobject to the temp byte array instantiated above
                Array.Copy(_state.Buffer, 0, packetData, 0, packetData.Length);

                // Fires the packet received event for Tcp.cs to handle
                PacketReceived?.Invoke(new Packet(packetData));

                // Restarts the async receive loop
                _state.Socket.BeginReceiveFrom(_state.Buffer,
                                                0,
                                                _state.Buffer.Length,
                                                SocketFlags.None,
                                                ref _state.RemoteEndpoint,
                                                OnPacketReceived,
                                                new StateObject(_state.RemoteEndpoint, _state.Buffer, _state.Socket));
            }
            catch (Exception ex)
            {
                UdpError?.Invoke(ex);
            }
        }

        // Fires when a packet is received
        public delegate void PacketReceivedDelegate(Packet packet);
        public event PacketReceivedDelegate PacketReceived;

        // Fires when a try catch fails
        public delegate void UdpErrorDelegate(Exception exception);
        public event UdpErrorDelegate UdpError;
    }
}