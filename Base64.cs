using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class representing a particular flavour of Base64 encoding.
    /// </summary>
    class Base64Style
    {
        public string Map;
        public char? Padding;
        public Dictionary<char, byte> Inverse;

        /// <summary>
        /// Automatically generates an efficient inverse mapping.
        /// </summary>
        /// <param name="map">A string containing the symbols to represent values 0 to 63 in order.</param>
        /// <param name="padding">An optional character used for padding, such as '=', or null for none.</param>
        public Base64Style(string map, char? padding)
        {
            Map = map;
            Padding = padding;
            Inverse = new Dictionary<char, byte>();
            for (byte i = 0; i < map.Length; i++)
                Inverse[map[i]] = i;
        }
    }

    /// <summary>
    /// Base64 static methods Encode() and Decode() plus common flavours.
    /// </summary>
    static class Base64
    {
        /// <summary>
        /// Standard Base64 as used by MIME and defined in RFC 2045, for testing.
        /// </summary>
        public static Base64Style RFC2045 = new Base64Style
        (
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/", '='
        );

        /// <summary>
        /// Hannes' Base64 as defined in the assignment specification.
        /// </summary>
        public static Base64Style Hannes = new Base64Style
        (
            " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\n", null
        );

        /// <summary>
        /// Transforms an array of bytes into a Base64 string.
        /// </summary>
        /// <param name="input">The array of input bytes to transform.</param>
        /// <param name="style">The flavour of Base64 encoding to use.</param>
        /// <returns>A string with the Base64 representation of the input.</returns>
        public static string Encode(byte[] input, Base64Style style)
        {
            int n = input.Length;
            int symbol; // six output bits
            // simple estimate, wastes 4 chars in the worst case
            StringBuilder output = new StringBuilder(4 + n * 4 / 3);
            for (int i = 0; i < n; i++) // walk 3 input bytes at a time
            {
                symbol = input[i] >> 2;                 // octet 0 [7..2] -> symbol 0 [5..0]
                output.Append(style.Map[symbol]);       // symbol 0 complete
                symbol = (input[i] & 3) << 4;           // octet 0 [1..0] -> symbol 1 [5..4]
                if (++i == n)                           // are we out of octets?
                {
                    output.Append(style.Map[symbol]);   // symbol 1 complete
                    break;
                }
                symbol |= input[i] >> 4;                // octet 1 [7..4] -> symbol 1 [3..0]
                output.Append(style.Map[symbol]);       // symbol 1 complete
                symbol = (input[i] & 15) << 2;          // octet 1 [3..0] -> symbol 2 [5..2]
                if (++i == n)                           // are we out of octets?
                {
                    output.Append(style.Map[symbol]);   // symbol 2 complete
                    break;
                }
                symbol |= input[i] >> 6;                // octet 2 [7..6] -> symbol 2 [1..0]
                output.Append(style.Map[symbol]);       // symbol 2 complete
                symbol = input[i] & 63;                 // octet 2 [5..0] -> symbol 3 [5..0]
                output.Append(style.Map[symbol]);       // symbol 3 complete
            }
            if (null != style.Padding)
            {
                // add padding bytes if the style requires it
                int count = (3 - n % 3) % 3;
                string pad = new String((char)style.Padding, count);
                output.Append(pad);
            }
            return output.ToString();
        }

        /// <summary>
        /// Transforms a Base64 string into the array of bytes that it represents.
        /// Postel's law: invalid and padding characters are accepted and ignored.
        /// </summary>
        /// <param name="input">The Base64 string to decode.</param>
        /// <param name="style">The flavour of Base64 encoding to use.</param>
        /// <returns>A new array with bytes that were represented by the input.</returns>
        public static byte[] Decode(string input, Base64Style style)
        {
            int n = input.Length;
            int octet; // eight output bits
            // simple estimate, needs to be truncated if any octets are padding or invalid
            byte[] output = new byte[n * 3 / 4];
            int i = -1; // input index
            int j = 0; // output index
            while (true)
            {
                while (++i < n)
                    if (style.Inverse.ContainsKey(input[i]))
                        break;
                if (i == n)
                    break;
                octet = style.Inverse[input[i]] << 2;           // symbol 0 [5..0] -> octet 0 [7..2]
                while (++i < n)
                    if (style.Inverse.ContainsKey(input[i]))
                        break;
                if (i == n)
                    break;
                octet |= style.Inverse[input[i]] >> 4;          // symbol 1 [5..4] -> octet 0 [1..0]
                output[j++] = (byte)octet;                      // octet 0 complete
                octet = (style.Inverse[input[i]] & 15) << 4;    // symbol 1 [3..0] -> octet 1 [7..4]
                while (++i < n)
                    if (style.Inverse.ContainsKey(input[i]))
                        break;
                if (i == n)
                    break;
                octet |= style.Inverse[input[i]] >> 2;          // symbol 2 [5..2] -> octet 1 [3..0]
                output[j++] = (byte)octet;                      // octet 1 complete
                octet = (style.Inverse[input[i]] & 3) << 6;     // symbol 2 [1..0] -> octet 2 [7..6]
                while (++i < n)
                    if (style.Inverse.ContainsKey(input[i]))
                        break;
                if (i == n)
                    break;
                octet |= style.Inverse[input[i]];               // symbol 3 [5..0] -> octet 2 [5..0]
                output[j++] = (byte)octet;                      // octet 2 complete
            }
            // truncate the output byte array to the actual number of octets
            Array.Resize(ref output, j);
            return output;
        }
    }
}
