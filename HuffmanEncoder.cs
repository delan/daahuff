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
        public static byte[] deflateBinaryData(byte[] input, FrequencyTable table)
        {
            BitArray output = new BitArray();
            HuffmanTree tree = new HuffmanTree(table);
            for (int i = 0; i < input.Length; i++)
            {
                byte[] symbol = new byte[] { input[i] };
                for (int j = tree.leaves.Count - 1; j >= 0; j--)
                    if (symbol.SequenceEqual(tree.leaves[j].symbol))
                        output.append(tree.leaves[j].sequence);
            }
            return output.octets();
        }
    }
}
