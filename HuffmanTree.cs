using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class representing a node in a Huffman tree.
    /// </summary>
    class HuffmanTreeNode : IComparable<HuffmanTreeNode>
    {
        public byte[] symbol;
        public double frequency;
        HuffmanTreeNode parent;

        public int CompareTo(HuffmanTreeNode other)
        {
            int retval = 0;
            if (frequency < other.frequency)
                retval = -1;
            if (frequency > other.frequency)
                retval = 1;
            return retval;
        }
    }

    /// <summary>
    /// Class representing a Huffman tree.
    /// </summary>
    class HuffmanTree
    {
        MinHeap<HuffmanTreeNode> pq = new MinHeap<HuffmanTreeNode>();

        public HuffmanTree(FrequencyTable table)
        {
            foreach (var pair in table.freq)
            {
                HuffmanTreeNode node = new HuffmanTreeNode() {
                    symbol = pair.Key,
                    frequency = pair.Value
                };
                pq.insert(node);
            }
        }
    }
}
