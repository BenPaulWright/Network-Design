using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_Design_Lib
{
    class PacketWindow : Queue<Packet>
    {
        public PacketWindow(uint size = 10)
        {
            Size = size;
        }

        public uint Size
        {
            get;
            private set;
        }

        public uint Base
        {
            get;
            set;
        } = 0;

        public uint NextSequenceNum
        {
            get;
            set;
        } = 0;

        public uint EndOfWindow
        {
            get
            {
                unchecked
                {
                    return Base + Size;
                }
            }
        }
    }
}
