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
        public static DAABitArray Deflate(string input, FrequencyTable table)
        {
            DAABitArray output = new DAABitArray();
            HuffmanTree tree = new HuffmanTree(table);
            for (int i = 0; i < input.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < tree.Leaves.Count; j++)
                {
                    if (input[i] == tree.Leaves[j].Symbol[0])
                    {
                        found = true;
                        output.Append(tree.Leaves[j].Code);
                    }
                }
                if (!found)
                    throw new KeyNotFoundException("Frequency table missing entry: " + input[i].ToString());
            }
            return output;
        }

        public static string Inflate(DAABitArray input, FrequencyTable table)
        {
            StringBuilder output = new StringBuilder();
            HuffmanTree tree = new HuffmanTree(table);
            HuffmanTreeNode node = tree.Head;
            for (int i = 0; i < input.NumBits; i++)
            {
                if (node.Symbol != null)
                {
                    output.Append(node.Symbol);
                    node = tree.Head;
                }
                if (input[i])
                {
                    node = node.Right;
                }
                else
                {
                    node = node.Left;
                }
            }
            return output.ToString();
        }
    }
}
