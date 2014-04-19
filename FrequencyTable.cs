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
        public Dictionary<byte[], double> Freq = new Dictionary<byte[], double>(
            new ByteArrayComparer()
        );

        /// <summary>
        /// Generate frequency information from raw binary data.
        /// The symbols yielded are single octets.
        /// </summary>
        public void Generate(byte[] input)
        {
            Freq.Clear();
            for (int i = 0; i < input.Length; i++)
            {
                byte[] symbol = new byte[] { input[i] };
                if (Freq.ContainsKey(symbol))
                    Freq[symbol] += 1.0;
                else
                    Freq[symbol] = 1.0;
            }
        }

        /// <summary>
        /// Generate frequency information from a UTF-8 string.
        /// The symbols yielded are sequences of octets that form Unicode code points.
        /// </summary>
        public void GenerateUTF8(byte[] input)
        {
            Freq.Clear();
            if (!UnicodeUtils.ValidateUTF8(input))
                return;
            int i = 0; // index in input
            while (i < input.Length)
            {
                byte[] symbol = UnicodeUtils.StepUTF8(input, ref i);
                if (Freq.ContainsKey(symbol))
                    Freq[symbol] += 1.0;
                else
                    Freq[symbol] = 1.0;
            }
        }

        /// <summary>
        /// Load frequency information from a string in the UI format.
        /// </summary>
        public void LoadUIString(string input)
        {
            Freq.Clear();
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
                    byte[] symbol = Encoding.UTF8.GetBytes(text);
                    if (Freq.ContainsKey(symbol))
                        Freq[symbol] += frequency;
                    else
                        Freq[symbol] = frequency;
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
        public string ToUIString()
        {
            StringBuilder output = new StringBuilder();
            foreach (byte[] symbol in Freq.Keys)
            {
                output.Append(Encoding.UTF8.GetString(symbol));
                output.Append(":");
                output.Append(Freq[symbol].ToString());
                output.Append("\n");
            }
            return output.ToString();
        }
    }
}
