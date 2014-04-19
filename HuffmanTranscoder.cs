using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Static class containing Huffman (de)compression methods.
    /// </summary>
    static class HuffmanTranscoder
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
        public static byte[] inflateBinaryData(byte[] input, FrequencyTable table)
        {
            BitArray bits = new BitArray(input);
            List<byte> output = new List<byte>();
            HuffmanTree tree = new HuffmanTree(table);
            HuffmanTreeNode node = tree.head;
            for (int i = 0; i < bits.length; i++)
            {
                if (bits[i] == false && node.left != null)
                {
                    node = node.left;
                }
                else if (bits[i] == true && node.right != null)
                {
                    node = node.right;
                }
                else
                {
                    foreach (byte octet in node.symbol)
                        output.Add(octet);
                    node = tree.head;
                    i--;
                }
            }
            return output.ToArray();
        }
    }
}
