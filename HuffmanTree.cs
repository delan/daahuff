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
        public double Frequency;
        public string Symbol;
        public DAABitArray Code;
        public HuffmanTreeNode Parent, Left, Right;

        public int CompareTo(HuffmanTreeNode other)
        {
            int output = 0;
            if (Frequency < other.Frequency)
                output = -1;
            if (Frequency > other.Frequency)
                output = 1;
            return output;
        }
    }

    /// <summary>
    /// Class representing a Huffman tree.
    /// </summary>
    class HuffmanTree
    {
        public HuffmanTreeNode Head;
        public List<HuffmanTreeNode> Leaves;
        MinHeap<HuffmanTreeNode> PriorityQueue;

        public HuffmanTree(FrequencyTable table)
        {
            if (table.Freq.Count < 2)
                throw new ArgumentException("Huffman trees require at least two symbols");

            Leaves = new List<HuffmanTreeNode>();
            PriorityQueue = new MinHeap<HuffmanTreeNode>();

            // first insert all leaf nodes into the min-heap
            foreach (var pair in table.Freq)
            {
                HuffmanTreeNode node = new HuffmanTreeNode() {
                    Frequency = pair.Value,
                    Symbol = pair.Key
                };
                PriorityQueue.Insert(node);
                Leaves.Add(node);
            }

            // then insert (n - 1) internal nodes
            for (int i = 1; i < Leaves.Count; i++)
            {
                HuffmanTreeNode node = new HuffmanTreeNode();
                node.Left = PriorityQueue.Remove();
                node.Right = PriorityQueue.Remove();
                node.Frequency = node.Left.Frequency + node.Right.Frequency;
                node.Left.Parent = node;
                node.Right.Parent = node;
                PriorityQueue.Insert(node);
            }

            // precalculate the bit sequences for each symbol
            foreach (HuffmanTreeNode leaf in Leaves)
            {
                HuffmanTreeNode node = leaf;
                leaf.Code = new DAABitArray();
                while (node.Parent != null)
                {
                    if (node == node.Parent.Left)
                        leaf.Code.Append(false);
                    if (node == node.Parent.Right)
                        leaf.Code.Append(true);
                    node = node.Parent;
                }
                leaf.Code.Reverse();
            }

            // now the min node is the root of the tree
            Head = PriorityQueue.Remove();
        }
    }
}
