using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Static class with Unicode utility methods.
    /// </summary>
    static class UnicodeUtils
    {
        /// <summary>
        /// Determines whether or not the given octet sequence is valid UTF-8.
        /// </summary>
        public static bool validate(byte[] input)
        {
            UTF8Encoding validator = new UTF8Encoding(
                false,  // whether or not to use byte order marks
                true    // whether or not to throw an exception upon invalid input
            );
            try
            {
                validator.GetString(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the next sequence of UTF-8 octets representing a single Unicode code point.
        /// The given index is used as a starting point, and is updated on completion.
        /// Avoid using this method on input that has not been validated.
        /// </summary>
        public static byte[] step(byte[] input, ref int i)
        {
            while ((input[i] & 0xC0) == 0x80)
                i++; // skip continuation octets left over
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
            return symbol;
        }
    }
}
