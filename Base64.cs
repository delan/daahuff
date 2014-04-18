using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    class Base64Style
    {
        public string map;
        public char? padding;
    }
    class Base64
    {
        public static Base64Style RFC2045 = new Base64Style
        {
            map = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/",
            padding = '=',
        };
        public static Base64Style Hannes = new Base64Style
        {
            map = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\n",
            padding = null,
        };
        public static string encode(byte[] input, Base64Style style)
        {
            int n = input.Length;
            int six; // six-bit integer value
            StringBuilder output = new StringBuilder(4 + n * 4 / 3);
            // walk 3 input bytes at a time
            for (int i = 0; i < n; i += 3)
            {
                six = input[i] >> 2;
                output.Append(style.map[six]);
                six = (input[i] & 3) << 4;
                if (i + 1 == n)
                {
                    output.Append(style.map[six]);
                    break;
                }
                six |= input[i + 1] >> 4;
                output.Append(style.map[six]);
                six = (input[i + 1] & 15) << 2;
                if (i + 2 == n)
                {
                    output.Append(style.map[six]);
                    break;
                }
                six |= input[i + 2] >> 6;
                output.Append(style.map[six]);
                six = input[i + 2] & 63;
                output.Append(style.map[six]);
            }
            // add padding bytes if the style requires it
            if (null != style.padding)
                output.Append("==".Substring((n % 3 + 2) % 3));
            return output.ToString();
        }
        public static byte[] decode(string input)
        {
            return new byte[0];
        }
    }
}
