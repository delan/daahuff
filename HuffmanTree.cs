using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class representing a generic node in a Huffman tree.
    /// This class must be inherited from, not instantiated.
    /// </summary>
    abstract class HuffmanTreeNode : IComparable<HuffmanTreeNode>
    {
        public double frequency;
        public HuffmanTreeInternalNode parent;

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
    /// Class representing a leaf node in a Huffman tree.
    /// </summary>
    class HuffmanTreeLeafNode : HuffmanTreeNode
    {
        public byte[] symbol;
        public BitArray sequence = new BitArray();
    }

    /// <summary>
    /// Class representing an internal node in a Huffman tree.
    /// </summary>
    class HuffmanTreeInternalNode : HuffmanTreeNode
    {
        public HuffmanTreeNode left, right;
    }

    /// <summary>
    /// Class representing a Huffman tree.
    /// </summary>
    class HuffmanTree
    {
        public HuffmanTreeNode head;
        public List<HuffmanTreeLeafNode> leaves = new List<HuffmanTreeLeafNode>();

        MinHeap<HuffmanTreeNode> pq = new MinHeap<HuffmanTreeNode>();

        public HuffmanTree(FrequencyTable table)
        {
            // first insert all leaf nodes into the min-heap
            foreach (var pair in table.freq)
            {
                HuffmanTreeLeafNode node = new HuffmanTreeLeafNode() {
                    frequency = pair.Value,
                    symbol = pair.Key
                };
                pq.insert(node);
                leaves.Add(node);
            }
            // then insert (n - 1) internal nodes
            for (int i = 0; i < pq.length; i++)
            {
                HuffmanTreeInternalNode node = new HuffmanTreeInternalNode();
                node.left = pq.remove();
                node.right = pq.remove();
                node.frequency = node.left.frequency + node.right.frequency;
                node.left.parent = node;
                node.right.parent = node;
                pq.insert(node);
            }
            // precalculate the bit sequences for each symbol
            foreach (HuffmanTreeLeafNode leaf in leaves)
            {
                HuffmanTreeNode node = leaf;
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
