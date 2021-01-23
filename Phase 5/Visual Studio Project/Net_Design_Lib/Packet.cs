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
        // Packet Structure
        // [ checksum bytes ] [ sequence number bytes ] [ type bytes ] [ --- data --- ] 

        // Stores the checksum byte length and index
        public static int checksumSize = 1;
        public static int checksumIndex = 0;

        // Stores the sequence number byte length and index
        public static int sequenceNumberSize = 4;
        public static int sequenceNumberIndex = 1;

        // Stores the type byte length and index
        public static int typeByteSize = 1;
        public static int typeByteIndex = 5;

        // Stores the total header size
        public static int headerSize = checksumSize + sequenceNumberSize + typeByteSize;

        public bool HasBeenAcked
        {
            get;
            set;
        } = false;

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
        public Packet(uint sequenceNumber, byte[] packetContent, PacketType packetType = PacketType.Data)
        {
            if (packetContent == null)
                packetContent = new byte[1];

            Raw = new byte[headerSize + packetContent.Length];

            var seqArray = BitConverter.GetBytes(sequenceNumber);

            for (int i = 0; i < seqArray.Length; i++)
                Raw[sequenceNumberIndex + i] = seqArray[i];

            Raw[typeByteIndex] = (byte)packetType;

            Array.Copy(packetContent, 0, Raw, headerSize, packetContent.Length);

            Raw[checksumIndex] = GetCheckSum();
        }

        // Holds the raw data for the packet
        public byte[] Raw;

        // Returns the packet's sequence number
        public uint SequenceNumber
        {
            get
            {
                return BitConverter.ToUInt32(Raw, sequenceNumberIndex);
            }
            set
            {
                var seqArray = BitConverter.GetBytes(value); ;

                for (int i = 0; i < seqArray.Length; i++)
                    Raw[sequenceNumberIndex + i] = seqArray[i];
            }
        }

        // Returns the packet's type
        public PacketType TypeByte
        {
            get
            {
                return (PacketType)Raw[typeByteIndex];
            }
            private set
            {
                Raw[typeByteIndex] = (byte)value;
            }
        }

        /// <summary>
        /// Calculates the packet's checksum
        /// </summary>
        /// <returns></returns>
        public byte GetCheckSum()
        {
            byte sum = 0;

            unchecked
            {
                for (int byteNumber = checksumIndex + 1; byteNumber < Raw.Length; byteNumber++)
                    sum += Raw[byteNumber];
            }

            return (byte)~sum;
        }

        /// <summary>
        /// Returns true if the current checksum is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValidChecksum()
        {
            byte sum = 0;

            unchecked
            {
                for (int byteNumber = checksumIndex; byteNumber < Raw.Length; byteNumber++)
                    sum += Raw[byteNumber];
            }

            return sum == 0b_1111_1111;
        }

        /// <summary>
        /// Returns the raw packet data (not including the header)
        /// </summary>
        /// <returns></returns>
        public byte[] GetNonHeaderData()
        {
            byte[] returnBytes = new byte[Raw.Length - headerSize];
            Array.Copy(Raw, headerSize, returnBytes, 0, returnBytes.Length);
            return returnBytes;
        }

        /// <summary>
        /// Returns a corrupted copy of the packet
        /// </summary>
        /// <returns></returns>
        public Packet AsCorrupt()
        {
            Packet corruptPacket = this;
            corruptPacket.Raw[headerSize] = (byte)~corruptPacket.Raw[headerSize];
            return corruptPacket;
        }

        // Possible packet types
        public enum PacketType
        {
            Ack,
            FileStart,
            FileEnd,
            StringStart,
            StringEnd,
            Data,
        }
    }
}
