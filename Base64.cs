using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Base64 static methods Encode() and Decode().
    /// </summary>
    static class Base64
    {
        /// <summary>
        /// Forward map of 6-bit values to characters.
        /// </summary>
        private static string forward = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\n";

        /// <summary>
        /// Reverse map of characters to 6-bit values.
        /// </summary>
        private static Dictionary<char, int> reverse;

        /// <summary>
        /// Transforms a sequence of bits into a Base64 string.
        /// </summary>
        /// <param name="input">The bits to transform.</param>
        /// <param name="style">The flavour of Base64 encoding to use.</param>
        /// <returns>A string with the Base64 representation of the input.</returns>
        public static string Encode(DAABitArray input)
        {
            int n = input.NumBits;
            // simple estimate, wastes 6 chars in the worst case
            StringBuilder output = new StringBuilder(6 + n / 6);
            for (int i = 0; i < n / 6; i++) // walk 6 input bits at a time
                output.Append(forward[(int) input.GetBitRange(i * 6, i * 6 + 5)]);
            if (n % 6 > 0)
                output.Append(forward[(int) input.GetBitRange(n / 6 * 6, n - 1) << (6 - n % 6)]);
            return output.ToString();
        }

        /// <summary>
        /// Transforms a Base64 string into the bits that it represents.
        /// </summary>
        /// <param name="input">The Base64 string to decode.</param>
        /// <param name="style">The flavour of Base64 encoding to use.</param>
        /// <returns>The bits that were represented by the input.</returns>
        public static DAABitArray Decode(string input)
        {
            DAABitArray output = new DAABitArray();
            if (reverse == null)
            {
                reverse = new Dictionary<char, int>();
                for (int i = 0; i < forward.Length; i++)
                    reverse[forward[i]] = i;
            }
            for (int i = 0; i < input.Length; i++)
                if (reverse.ContainsKey(input[i]))
                    output.Append(reverse[input[i]], 6);
            return output;
        }
    }
}
