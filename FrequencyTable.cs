using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
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
            freq.Clear();
            if (!UnicodeUtils.validate(input))
                return;
            int i = 0; // index in input
            while (i < input.Length)
            {
                byte[] symbol = UnicodeUtils.step(input, ref i);
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
