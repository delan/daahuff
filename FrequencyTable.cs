using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class that allows byte[] to be compared as a Dictionary key.
    /// </summary>
    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public int GetHashCode(byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return key.Sum(b => b);
        }
        public bool Equals(byte[] left, byte[] right)
        {
            if (left == null || right == null)
            {
                return left == right;
            }
            return left.SequenceEqual(right);
        }
    }

    /// <summary>
    /// Class representing a Huffman symbol frequency table.
    /// </summary>
    class FrequencyTable
    {
        public Dictionary<byte[], double> freq = new Dictionary<byte[], double>(
            new ByteArrayComparer()
        );

        /// <summary>
        /// Generate frequency information from raw binary data.
        /// The symbols yielded are single octets.
        /// </summary>
        public void generateFromBinaryData(byte[] input)
        {
            freq.Clear();
            for (int i = 0; i < input.Length; i++)
            {
                byte[] symbol = new byte[] { input[i] };
                if (freq.ContainsKey(symbol))
                    freq[symbol] += 1.0;
                else
                    freq[symbol] = 1.0;
            }
        }

        /// <summary>
        /// Generate frequency information from a UTF-8 string.
        /// The symbols yielded are sequences of octets that form Unicode code points.
        /// </summary>
        public void generateFromUTF8(byte[] input)
        {
            UTF8Encoding validator = new UTF8Encoding(
                false,  // whether or not to use byte order marks
                true    // whether or not to throw an exception upon invalid input
            );
            freq.Clear();
            try
            {
                validator.GetString(input);
            }
            catch
            {
                return;
            }
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
                // now for the actual frequency stuff
                if (freq.ContainsKey(symbol))
                    freq[symbol] += 1.0;
                else
                    freq[symbol] = 1.0;
            }
        }

        /// <summary>
        /// Load frequency information from a string in the UI format.
        /// </summary>
        public void loadUIString(string input)
        {
            freq.Clear();
            string[] lines = input.Split(new char[] { '\n', '\r' });
            foreach (string line in lines)
            {
                double frequency = double.NaN;
                string[] tokens = line.Split(':');
                string text = string.Join(":", tokens, 0, tokens.Length - 1);
                if (text.Length == 0)
                    continue;
                try
                {
                    frequency = double.Parse(tokens[tokens.Length - 1]);
                    // now for the actual frequency stuff
                    byte[] symbol = Encoding.UTF8.GetBytes(text);
                    if (freq.ContainsKey(symbol))
                        freq[symbol] += frequency;
                    else
                        freq[symbol] = frequency;
                }
                catch
                {
                    // probably an empty line
                }
            }
        }

        /// <summary>
        /// Returns a string representation suitable for UI use.
        /// Assumes that each symbol is a UTF-8 octet sequence.
        /// </summary>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            foreach (byte[] symbol in freq.Keys)
            {
                output.Append(Encoding.UTF8.GetString(symbol));
                output.Append(":");
                output.Append(freq[symbol].ToString());
                output.Append("\n");
            }
            return output.ToString();
        }
    }
}
