using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_Design_Lib
{
    /// <summary>
    /// Class for constructing packets
    /// </summary>
    public class Packet
    {
        /// <summary>
        /// Creates a packet from raw data
        /// </summary>
        /// <param name="raw"></param>
        public Packet(byte[] raw)
        {
            this.Raw = raw;
        }

        /// <summary>
        /// Creates a packet from data, packet num, and total packets
        /// </summary>
        /// <param name="packetNumber"></param>
        /// <param name="totalPackets"></param>
        /// <param name="packetContent"></param>
        public Packet(uint packetNumber, uint totalPackets, byte[] packetContent)
        {
            // Instantiate Raw
            Raw = new byte[packetContent.Length + 8];

            // Converts uints to byte[4]
            var indexArray = BitConverter.GetBytes(packetNumber);
            var totalArray = BitConverter.GetBytes(totalPackets);

            // Copy over index
            Array.Copy(indexArray, 0, Raw, 0, 4);

            // Copy over total
            Array.Copy(totalArray, 0, Raw, 4, 4);

            // Copy over data
            Array.Copy(packetContent, 0, Raw, 8, packetContent.Length);
        }

        // Stores the packet data including index, total packets, and content
        public byte[] Raw;

        // Gets the first four bytes and converts them to an int32
        public uint PacketNumber
        {
            get
            {
                return BitConverter.ToUInt32(Raw, 0);
            }
        }

        // Gets the second four bytes and converts them to an int32
        public uint TotalPackets
        {
            get
            {
                return BitConverter.ToUInt32(Raw, 4);
            }
        }
    }
}
