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
        public Dictionary<string, double> Freq = new Dictionary<string, double>();

        /// <summary>
        /// Generate frequency information from raw binary data.
        /// The symbols yielded are single octets.
        /// </summary>
        public void Generate(string input)
        {
            Freq.Clear();
            for (int i = 0; i < input.Length; i++)
            {
                string symbol = input[i].ToString();
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
            string[] lines = input.Split('\n');
            foreach (string line in lines)
            {
                double frequency = double.NaN;
                string[] tokens = line.Split(':');
                string text = string.Join(":", tokens, 0, tokens.Length - 1);
                if (text.Length == 0)
                    continue;
                try
                {
                    string symbol;
                    frequency = double.Parse(tokens[tokens.Length - 1]);
                    if (text == "\\n")
                        symbol = "\n";
                    else if (text == "\\r")
                        symbol = "\r";
                    else if (text == "\\t")
                        symbol = "\t";
                    else if (text == "\\\\")
                        symbol = "\\";
                    else
                        symbol = text;
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
            foreach (string symbol in Freq.Keys)
            {
                if (symbol == "\n")
                    output.Append("\\n");
                else if (symbol == "\r")
                    output.Append("\\r");
                else if (symbol == "\t")
                    output.Append("\\t");
                else if (symbol == "\\")
                    output.Append("\\\\");
                else
                    output.Append(symbol);
                output.Append(":");
                output.Append(Freq[symbol].ToString());
                output.Append("\n");
            }
            return output.ToString();
        }
    }
}
