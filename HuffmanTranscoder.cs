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
        public static byte[] Deflate(byte[] input, FrequencyTable table)
        {
            BitArray output = new BitArray();
            HuffmanTree tree = new HuffmanTree(table);
            for (int i = 0; i < input.Length; i++)
            {
                byte[] symbol = new byte[] { input[i] };
                int j = tree.Leaves.Count;
                while (--j >= 0)
                {
                    if (symbol.SequenceEqual(tree.Leaves[j].Symbol))
                    {
                        output.Append(tree.Leaves[j].Code);
                        break; // do not remove this
                    }
                }
                if (j < 0)
                    throw new KeyNotFoundException();
            }
            return output.GetBytes();
        }

        public static byte[] DeflateUTF8(byte[] input, FrequencyTable table)
        {
            BitArray output = new BitArray();
            HuffmanTree tree = new HuffmanTree(table);
            if (!UnicodeUtils.ValidateUTF8(input))
                throw new ArgumentException();
            int i = 0; // index in input
            while (i < input.Length)
            {
                byte[] symbol = UnicodeUtils.StepUTF8(input, ref i);
                int j = tree.Leaves.Count;
                while (--j >= 0)
                {
                    if (symbol.SequenceEqual(tree.Leaves[j].Symbol))
                    {
                        output.Append(tree.Leaves[j].Code);
                        break; // do not remove this
                    }
                }
                if (j < 0)
                    throw new KeyNotFoundException();
            }
            return output.GetBytes();
        }

        public static byte[] Inflate(byte[] input, FrequencyTable table)
        {
            BitArray bits = new BitArray(input);
            List<byte> output = new List<byte>();
            HuffmanTree tree = new HuffmanTree(table);
            HuffmanTreeNode node = tree.Head;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == false && node.Left != null)
                {
                    node = node.Left;
                }
                else if (bits[i] == true && node.Right != null)
                {
                    node = node.Right;
                }
                else
                {
                    foreach (byte octet in node.Symbol)
                        output.Add(octet);
                    node = tree.Head;
                    i--;
                }
            }
            return output.ToArray();
        }
    }
}
