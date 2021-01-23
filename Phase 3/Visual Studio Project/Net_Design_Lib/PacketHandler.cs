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
        /// <summary>
        /// Reads in a file and returns a packet array representing it
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="packetSize"></param>
        /// <returns></returns>
        public static Packet[] FileToPackets(string filePath, int packetSize, byte currentSequenceNumber)
        {
            return ToPackets(System.IO.File.ReadAllBytes(filePath), packetSize, currentSequenceNumber);
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
        /// Converts a string to a packet array
        /// </summary>
        /// <param name="str"></param>
        /// <param name="packetSize"></param>
        /// <param name="currentSequenceNumber"></param>
        /// <returns></returns>
        public static Packet[] StringToPackets(string str, int packetSize, ref byte currentSequenceNumber)
        {
            return ToPackets(Encoding.ASCII.GetBytes(str), packetSize, currentSequenceNumber);
        }

        /// <summary>
        /// Converts a packet array to a string
        /// </summary>
        /// <param name="packets"></param>
        /// <returns></returns>
        public static string PacketsToString(Packet[] packets)
        {
            return Encoding.ASCII.GetString(FromPackets(packets));
        }

        /// <summary>
        /// Converts a byte array into packets
        /// </summary>
        /// <param name="data">Byte array which contains the data you want to send</param>
        /// <param name="packetSize">Packet size in bytes</param>
        /// <returns></returns>
        public static Packet[] ToPackets(byte[] data, int packetSize, byte currentSequenceNumber)
        {
            // Number of packets requried to hold all the given data
            var packetCount = (int)Math.Ceiling((double)(data.Length / packetSize));

            // Space left in each packet after removing the index and total
            var dataSpace = packetSize - Packet.headerSize;

            // Array to store the packets
            Packet[] packets = new Packet[packetCount];

            // Splits data into packets
            for (int packetIterator = 0; packetIterator < packetCount; packetIterator++)
            {
                // Temp array to copy data into
                var tempData = new byte[dataSpace];

                // Copies data into temp array
                Array.Copy(data, packetIterator * dataSpace, tempData, 0, dataSpace);

                // Creates a packet and puts it in the return array
                packets[packetIterator] = new Packet(currentSequenceNumber, tempData);

                unchecked
                {
                    currentSequenceNumber++;
                }
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
            var dataPerPacket = packets[0].Raw.Length - Packet.headerSize;

            // Number of bytes stored in all the packets
            var totalData = dataPerPacket * packets.Length;

            // Array which stores the data to return
            byte[] data = new byte[totalData];

            // Copies the data from each packet into the data array
            for (int index = 0; index < packets.Length; index++)
                Array.Copy(packets[index].Raw, Packet.headerSize, data, dataPerPacket * index, dataPerPacket);

            return data;
        }

    }
}
