using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_Design_Lib
{
    /// <summary>
    /// Class for parsing data into packets and vice versa
    /// </summary>
    public static class PacketHandler
    {
        // Packet Structure
        // [ 4 byte index ][ 4 byte total ][ --- data --- ]

        /// <summary>
        /// Reads in a file and returns a packet array representing it
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="packetSize"></param>
        /// <returns></returns>
        public static Packet[] FileToPackets(string filePath, uint packetSize)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return ToPackets(bytes, packetSize);
        }

        /// <summary>
        /// Takes a packet array representing a file and saves it to the given path
        /// </summary>
        /// <param name="packets"></param>
        /// <param name="filePath"></param>
        public static void PacketsToFile(Packet[] packets, string filePath)
        {
            byte[] bytes = FromPackets(packets);
            System.IO.File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Converts a byte array into packets
        /// </summary>
        /// <param name="data"></param>
        /// <param name="packetSize"></param>
        /// <returns></returns>
        public static Packet[] ToPackets(byte[] data, uint packetSize)
        {
            // Number of packets requried to hold all the given data
            var totalPackets = (uint)Math.Ceiling((double)(data.Length / packetSize));

            // Space left in each packet after removing the index and total
            var dataSpace = packetSize - 8;

            // Array to store the packets
            Packet[] packets = new Packet[totalPackets];

            // Splits data into packets
            for (uint packetNumber = 0; packetNumber < totalPackets; packetNumber++)
            {
                // Temp array to copy data into
                var tempData = new byte[dataSpace];

                // Copies data into temp array
                Array.Copy(data, packetNumber * dataSpace, tempData, 0, dataSpace);

                // Creates a packet and puts it in the return array
                packets[packetNumber] = new Packet(packetNumber, totalPackets, tempData);
            }

            return packets;
        }

        /// <summary>
        /// Converts a packet array into bytes
        /// </summary>
        /// <param name="packets"></param>
        /// <returns></returns>
        public static byte[] FromPackets(Packet[] packets)
        {
            // Number of bytes per packet which store raw data
            var dataPerPacket = packets[0].Raw.Length - 8;

            // Number of bytes stored in all the packets
            var totalData = dataPerPacket * packets.Length;

            // Array which stores the data to return
            byte[] data = new byte[totalData];

            // Copies the data from each packet into the data array
            for (int index = 0; index < packets.Length; index++)
                Array.Copy(packets[index].Raw, 8, data, dataPerPacket * index, dataPerPacket);

            return data;
        }
    }
}
