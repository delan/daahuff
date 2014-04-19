using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class that allows byte[] to be compared as a Dictionary key.
    /// </summary>
    class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public int GetHashCode(byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException();
            return key.Sum(b => b);
        }
        public bool Equals(byte[] left, byte[] right)
        {
            if (left == null || right == null)
            {
                return left == right;
            }
            return left.SequenceEqual(right);
        }
    }
}
