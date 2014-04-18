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

        public void add(bool value)
        {
            data.Add(value);
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

        public bool this[int i]
        {
            get
            {
                return data[i];
            }
        }
    }
}
