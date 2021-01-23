using System.Text;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;

namespace Client
{
    /// <summary>
    /// Static implementation of a UDP socket capable of transmitting and receiving text
    /// </summary>
    public static class UDP_Client_Static
    {
        /// <summary>
        /// Sends message and waits for a response
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        public static void Send(IPAddress ip, int port, string message)
        {
            // Try catch used to make sure that any internal errors are caught and the program doesn't crash
            try
            {
                // The using statement allows an instance of the Socket class to be easily disposed when the method returns
                using (var _soc = new Socket(SocketType.Dgram, ProtocolType.Udp) { SendBufferSize = 131072, ReceiveBufferSize = 131072 })
                {
                    // Defines the endpoint to over which to broadcast the data
                    var _endPoint = new IPEndPoint(ip, port);

                    // Encodes the string message as a byte array and sends it via the endpoint
                    _soc.SendTo(Encoding.ASCII.GetBytes(message), _endPoint);

                    // Waits for a message
                    RecieveMessage(_soc, _endPoint);
                }
            }
            catch (Exception ex)
            {
                // For use debugging
                Console.WriteLine(ex);
            }
        }

        public static void SendFile(IPAddress ip, int port, string filePath)
        {
            // Try catch used to make sure that any internal errors are caught and the program doesn't crash
            try
            {
                // The using statement allows an instance of the Socket class to be easily disposed when the method returns
                using (var _soc = new Socket(SocketType.Dgram, ProtocolType.Udp) { SendBufferSize = 131072, ReceiveBufferSize = 131072 })
                {
                    // Defines the endpoint to over which to broadcast the data
                    var _endPoint = new IPEndPoint(ip, port);

                    // Gets the bare name and extention of the file
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    var fileExtention = Path.GetExtension(filePath);

                    // Sends header before file "#FILE <FileName> <FileExtention>"
                    _soc.SendTo(Encoding.ASCII.GetBytes($"#FILE*{fileName}*{fileExtention}"), _endPoint);

                    // Wait to make sure the first message arrives
                    System.Threading.Thread.Sleep(250);

                    // Reads in the file to a char[]
                    var fileData = File.ReadAllBytes(filePath);

                    // Sends the file data
                    _soc.SendTo(fileData, _endPoint);

                    // Waits for a message
                    RecieveMessage(_soc, _endPoint);
                }
            }
            catch (Exception ex)
            {
                // For use debugging
                Console.WriteLine(ex);
            }
        }

        private static void RecieveMessage(Socket soc, IPEndPoint endPoint)
        {
            // Defines a buffer in which to store incoming data
            var _buff = new byte[1024];

            // Retrieves the number of new bytes in the buffer
            var _bytes = soc.Receive(_buff);

            // Raises the MessageReceived event in order to notify the UI of an incomming message
            MessageReceived.Invoke(endPoint.ToString(), Encoding.ASCII.GetString(_buff, 0, _bytes));
        }

        // Defines the delegate and event used to handle incomming messages
        public delegate void MessageReceivedDelegate(string endPoint, string message);
        public static event MessageReceivedDelegate MessageReceived;
    }
}
