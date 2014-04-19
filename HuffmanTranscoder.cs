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
                int j = tree.leaves.Count;
                while (--j >= 0)
                {
                    if (symbol.SequenceEqual(tree.leaves[j].symbol))
                    {
                        output.append(tree.leaves[j].sequence);
                        break; // do not remove this
                    }
                }
                if (j < 0)
                    throw new KeyNotFoundException();
            }
            return output.octets();
        }

        public static byte[] deflateUTF8(byte[] input, FrequencyTable table)
        {
            BitArray output = new BitArray();
            HuffmanTree tree = new HuffmanTree(table);
            UTF8Encoding validator = new UTF8Encoding(
                false,  // whether or not to use byte order marks
                true    // whether or not to throw an exception upon invalid input
            );
            validator.GetString(input);
            // validating that the octet sequence is correct UTF-8 prior to using it
            // allows us to make assumptions that would normally be unsafe
            int i = 0; // index in input
            while (i < input.Length)
            {
                int j = 0; // index in symbol
                // the maximum sequence length is 4 octets, given RFC 3629
                byte[] symbol = new byte[4];
                byte head = input[i]; // octet to be examined
                if ((head & 0xF8) == 0xF0)  // 4 octet sequence
                {
                    symbol[j++] = input[i++];
                    symbol[j++] = input[i++];
                    symbol[j++] = input[i++];
                    symbol[j++] = input[i++];
                }
                if ((head & 0xF0) == 0xE0)  // 3 octet sequence
                {
                    symbol[j++] = input[i++];
                    symbol[j++] = input[i++];
                    symbol[j++] = input[i++];
                }
                if ((head & 0xE0) == 0xC0)  // 2 octet sequence
                {
                    symbol[j++] = input[i++];
                    symbol[j++] = input[i++];
                }
                if ((head & 0x80) == 0x00)  // 1 octet sequence
                {
                    symbol[j++] = input[i++];
                }
                // truncate the symbol to the actual number of octets
                Array.Resize(ref symbol, j);
                int k = tree.leaves.Count;
                while (--k >= 0)
                {
                    if (symbol.SequenceEqual(tree.leaves[k].symbol))
                    {
                        output.append(tree.leaves[k].sequence);
                        break; // do not remove this
                    }
                }
                if (k < 0)
                    throw new KeyNotFoundException();
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
