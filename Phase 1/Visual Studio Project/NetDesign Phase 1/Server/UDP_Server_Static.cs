using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server
{
    /// <summary>
    /// Static implementation of a UDP socket capable of binding to an endpoint and echoing messages from clients
    /// </summary>
    public static class UDP_Server_Static
    {
        // Stores all sockets opened by OpenSocket in order to close all previous when OpenSocket is called
        private static Queue<Socket> _openSockets = new Queue<Socket>();

        // Opens a socket with the given Ip and Port and closes all other sockets associated with this method
        public static void OpenSocket(IPAddress ip, int port)
        {
            // Try catch used to make sure that any internal errors are caught and the program doesn't crash
            try
            {
                // Task.Run in conjunction with a lambda function allows for asynchronous in-line codeblocks
                Task.Run(() =>
                {
                    // The using statement allows an instance of the Socket class to be easily disposed when the method returns
                    using (var _soc = new Socket(SocketType.Dgram, ProtocolType.Udp) { SendBufferSize = 131072, ReceiveBufferSize = 131072 })
                    {
                        // Queues the newly created socket
                        _openSockets.Enqueue(_soc);

                        // Closes all excess sockets created by this method
                        while (_openSockets.Count > 1)
                        {
                            // Grabs the first socket in the queue
                            var _closeSoc = _openSockets.Dequeue();

                            // Raises the StatusUpdated event in order to notify the UI of a socket closing
                            StatusUpdated?.Invoke("Closed " + _closeSoc.LocalEndPoint.ToString());

                            // Closes the socket
                            _closeSoc.Close();
                            _closeSoc = null;
                        }

                        // Creates an endpoint which is either derived from the given IP or any ip if the given ip is null
                        var _localEndPoint = (ip == null) ? new IPEndPoint(IPAddress.Any, port) : new IPEndPoint(ip, port);

                        // Creates an endpoint to store the address of whatever client sends a message
                        EndPoint _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                        // Binds the socket to the local end point in order to retrieve messages
                        _soc.Bind(_localEndPoint);

                        // Defines a buffer in which to store incoming data
                        var _buff = new byte[131072];

                        // Raises the StatusUpdated event in order to notify the UI of a socket opening
                        StatusUpdated?.Invoke("Opened " + _localEndPoint.ToString());

                        // Loops message recieve and send to echo back client messages
                        while (_soc != null)
                        {
                            // Retrieves the number of new bytes in the buffer
                            var _bytes = _soc.ReceiveFrom(_buff, 0, _buff.Length, SocketFlags.None, ref _remoteEndPoint);

                            // Converts the buffer to a string message
                            var _msg = Encoding.ASCII.GetString(_buff, 0, _bytes);

                            // Vars to store the file parameters
                            var _fileName = "";
                            var _fileExtension = "";
                            var _fileSize = 0;

                            // Header before file "#FILE <FileName> <FileExtention>"
                            if (_msg.Split('*')[0] == "#FILE")
                            {
                                // Grabs the file parameters from the header message
                                _fileName = _msg.Split('*')[1];
                                _fileExtension = _msg.Split('*')[2];

                                // Notifies the UI that a file is incoming
                                MessageReceived?.Invoke(_remoteEndPoint.ToString(), $"Incoming File: {_fileName}{_fileExtension}");

                                // Waits for the num bytes and file data from the client
                                _fileSize = _soc.ReceiveFrom(_buff, 0, _buff.Length, SocketFlags.None, ref _remoteEndPoint);

                                // Notifies the UI of a new file
                                FileReceived?.Invoke(_remoteEndPoint.ToString(), _fileSize, _buff, _fileName, _fileExtension);

                                _soc.SendTo(Encoding.ASCII.GetBytes($"Successfully Recieved: {_fileName}{_fileExtension}"), SocketFlags.None, _remoteEndPoint);

                                // Skips the message recieved call
                                continue;
                            }

                            // Notifies the UI of a new message
                            MessageReceived.Invoke(_remoteEndPoint.ToString(), _msg);

                            // Creates the echo message string
                            var _echoMsg = "Echo: " + _msg;

                            // Sends the echo message string to the client
                            _soc.SendTo(Encoding.ASCII.GetBytes(_echoMsg), SocketFlags.None, _remoteEndPoint);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // For use debugging
                Console.WriteLine(ex);
            }
        }

        // Defines the delegate and event used to handle incomming messages
        public delegate void MessageReceivedDelegate(string endPoint, string message);
        public static event MessageReceivedDelegate MessageReceived;

        // Defines the delegate and event used to handle incomming files
        public delegate void FileReceivedDelegate(string endPoint, int numBytes, byte[] fileBuffer, string fileName, string fileExtension);
        public static event FileReceivedDelegate FileReceived;

        // Defines the delegate and event used to update the UI with connection and closing messages
        public delegate void StatusUpdatedDelegate(string message);
        public static event StatusUpdatedDelegate StatusUpdated;
    }
}