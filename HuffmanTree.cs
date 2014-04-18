using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class representing a node of any kind in a Huffman tree.
    /// </summary>
    class HuffmanTreeNode : IComparable<HuffmanTreeNode>
    {
        public double frequency;
        public byte[] symbol;
        public BitArray sequence;
        public HuffmanTreeNode parent, left, right;

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
        public HuffmanTreeNode head;
        public List<HuffmanTreeNode> leaves = new List<HuffmanTreeNode>();

        MinHeap<HuffmanTreeNode> pq = new MinHeap<HuffmanTreeNode>();

        public HuffmanTree(FrequencyTable table)
        {
            // first insert all leaf nodes into the min-heap
            foreach (var pair in table.freq)
            {
                HuffmanTreeNode node = new HuffmanTreeNode() {
                    frequency = pair.Value,
                    symbol = pair.Key
                };
                pq.insert(node);
                leaves.Add(node);
            }
            // then insert (n - 1) internal nodes
            for (int i = 1; i < leaves.Count; i++)
            {
                HuffmanTreeNode node = new HuffmanTreeNode();
                node.left = pq.remove();
                node.right = pq.remove();
                node.frequency = node.left.frequency + node.right.frequency;
                node.left.parent = node;
                node.right.parent = node;
                pq.insert(node);
            }
            // precalculate the bit sequences for each symbol
            foreach (HuffmanTreeNode leaf in leaves)
            {
                HuffmanTreeNode node = leaf;
                leaf.sequence = new BitArray();
                while (node.parent != null)
                {
                    if (node == node.parent.left)
                        leaf.sequence.add(false);
                    if (node == node.parent.right)
                        leaf.sequence.add(true);
                    node = node.parent;
                }
            }
            // now the min node is the root of the tree
            head = pq.remove();
        }
    }
}
