using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Static class containing Huffman (de)compression methods.
    /// </summary>
    static class HuffmanEncoder
    {
        public static byte[] deflate(byte[] input, FrequencyTable table)
        {
            BitArray output = new BitArray();
            HuffmanTree tree = new HuffmanTree(table);
            return output.octets();
        }
    }
}
