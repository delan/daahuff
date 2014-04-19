using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class representing an array of bits.
    /// </summary>
    public class BitArray
    {
        private List<bool> data = new List<bool>();

        public int Length
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
                Add(    input[i] >> 7);
                Add(1 & input[i] >> 6);
                Add(1 & input[i] >> 5);
                Add(1 & input[i] >> 4);
                Add(1 & input[i] >> 3);
                Add(1 & input[i] >> 2);
                Add(1 & input[i] >> 1);
                Add(1 & input[i]     );
            }
        }

        public void Add(bool value)
        {
            data.Add(value);
        }

        public void Add(int value)
        {
            data.Add(value != 0 ? true : false);
        }

        public void Append(BitArray other)
        {
            for (int i = 0; i < other.Length; i++)
                Add(other[i]);
        }

        public void Reverse()
        {
            data.Reverse();
        }

        public byte[] GetBytes()
        {
            List<byte> output = new List<byte>();
            byte next = 0; // next octet to output
            int j = 7; // bit index in next (LSB 0)
            for (int i = 0; i < Length; i++)
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
