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
        public string map;
        public char? padding;
        public Dictionary<char, byte> inverse;

        /// <summary>
        /// Automatically generates an efficient inverse mapping.
        /// </summary>
        /// <param name="map">A string containing the symbols to represent values 0 to 63 in order.</param>
        /// <param name="padding">An optional character used for padding, such as '=', or null for none.</param>
        public Base64Style(string map, char? padding)
        {
            this.map = map;
            this.padding = padding;
            inverse = new Dictionary<char, byte>();
            for (byte i = 0; i < map.Length; i++)
                inverse[map[i]] = i;
        }
    }
    class Base64
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
        public static string encode(byte[] input, Base64Style style)
        {
            int n = input.Length;
            int six; // six output bits
            // simple estimate, wastes 4 chars in the worst case
            StringBuilder output = new StringBuilder(4 + n * 4 / 3);
            // walk 3 input bytes at a time
            for (int i = 0; i < n; i++)
            {
                // top 6 bits of octet 0 go to symbol 0
                six = input[i] >> 2;
                // we have symbol 0
                output.Append(style.map[six]);
                // bottom 2 bits of octet 0 go to symbol 1
                six = (input[i] & 3) << 4;
                // are we out of octets?
                if (++i == n)
                {
                    // we have symbol 1
                    output.Append(style.map[six]);
                    break;
                }
                // top 4 bits of octet 1 go to symbol 1
                six |= input[i] >> 4;
                // we have symbol 1
                output.Append(style.map[six]);
                // bottom 4 bits of octet 1 go to symbol 2
                six = (input[i] & 15) << 2;
                // are we out of octets?
                if (++i == n)
                {
                    // we have symbol 2
                    output.Append(style.map[six]);
                    break;
                }
                // top 2 bits of octet 2 go to symbol 2
                six |= input[i] >> 6;
                // we have symbol 2
                output.Append(style.map[six]);
                // bottom 6 bits of octet 2 go to symbol 3
                six = input[i] & 63;
                // we have symbol 3
                output.Append(style.map[six]);
            }
            // add padding bytes if the style requires it
            if (null != style.padding)
            {
                int count = (3 - n % 3) % 3;
                string pad = new String((char)style.padding, count);
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
        public static byte[] decode(string input, Base64Style style)
        {
            int n = input.Length;
            int octet; // eight output bits
            // simple generous estimate
            // may need to be truncated if padding or invalid input bytes exist
            byte[] output = new byte[n * 3 / 4];
            int i = -1; // input symbol index
            int j = 0; // output byte index
            while (true)
            {
                // find the next valid symbol
                while (++i < n)
                    if (style.inverse.ContainsKey(input[i]))
                        break;
                if (i == n)
                    break;
                // contribute the top 6 bits of octet 0
                octet = style.inverse[input[i]] << 2;
                // find the next valid symbol
                while (++i < n)
                    if (style.inverse.ContainsKey(input[i]))
                        break;
                if (i == n)
                    break;
                // contribute the bottom 2 bits of octet 0
                octet |= style.inverse[input[i]] >> 4;
                // octet 0 complete
                output[j++] = (byte)octet;
                // contribute the top 4 bits of octet 1
                octet = (style.inverse[input[i]] & 15) << 4;
                // find the next valid symbol
                while (++i < n)
                    if (style.inverse.ContainsKey(input[i]))
                        break;
                if (i == n)
                    break;
                // contribute the bottom 4 bits of octet 1
                octet |= style.inverse[input[i]] >> 2;
                // octet 1 complete
                output[j++] = (byte)octet;
                // contribute the top 2 bits of octet 2
                octet = (style.inverse[input[i]] & 3) << 6;
                // find the next valid symbol
                while (++i < n)
                    if (style.inverse.ContainsKey(input[i]))
                        break;
                if (i == n)
                    break;
                // contribute the bottom 6 bits of octet 2
                octet |= style.inverse[input[i]];
                // octet 2 complete
                output[j++] = (byte)octet;
            }
            // truncate the output byte array to the actual number of octets
            Array.Resize(ref output, j);
            return output;
        }
    }
}
