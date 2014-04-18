using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class representing an ideal array of bits.
    /// </summary>
    public class BitArray
    {
        private List<bool> data = new List<bool>();

        public int length
        {
            get
            {
                return data.Count;
            }
        }

        public BitArray()
        {
            // nothing to see here
        }

        public BitArray(byte[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                add(    input[i] >> 7);
                add(1 & input[i] >> 6);
                add(1 & input[i] >> 5);
                add(1 & input[i] >> 4);
                add(1 & input[i] >> 3);
                add(1 & input[i] >> 2);
                add(1 & input[i] >> 1);
                add(1 & input[i]     );
            }
        }

        public void add(bool value)
        {
            data.Add(value);
        }

        public void add(int value)
        {
            data.Add(value != 0 ? true : false);
        }

        public void append(BitArray other)
        {
            for (int i = 0; i < other.length; i++)
                data.Add(other[i]);
        }

        public void reverse()
        {
            data.Reverse();
        }

        public byte[] octets()
        {
            List<byte> output = new List<byte>();
            byte next = 0; // next octet to output
            int j = 7; // bit index in next (LSB 0)
            for (int i = 0; i < length; i++)
            {
                next |= (byte)((data[i] ? 1 : 0) << j);
                if (--j < 0)
                {
                    output.Add(next);
                    next = 0;
                    j = 7;
                }
            }
            if (j >= 0)
                output.Add(next);
            return output.ToArray();
        }

        public bool this[int i]
        {
            get
            {
                return data[i];
            }
        }
    }
}
