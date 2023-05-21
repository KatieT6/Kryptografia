using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class Converter
    {

        public static byte[] HexFormat(string str)
        {
            byte[] bytes = new byte[str.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        // Converts an Array of bytes to hex String
        // Checks if every hex value has length of 2
        // If not this element gets an extra 0xFF
        public static string ByteArrayToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        // Converts 1 byte to String form with complement to a given number <n>
        // e.g. nr = 8: (byte) 10 => (String) "00001010"
        public static string ByteToBin(byte oneByte, int nr)
        {
            var s = new StringBuilder(Convert.ToString(oneByte, 2));
            var length = nr - s.Length;
            for (int i = 0; i < length; i++)
            {
                s.Insert(0, "0");
            }
            return s.ToString();
        }

        // Converts 1 String of byte value to int array of binary values, e.g "10101010" => int[8]
        public static int[] BinStringToIntArr(string b)
        {
            return b.Select(c => int.Parse(c.ToString())).ToArray();
        }

        // Converts byte array to int array of binary values, e.g. byte[8] => int[64]
        public static int[] ByteArrToIntArr(byte[] input)
        {
            var result = new int[input.Length * 8];
            for (int i = 0; i < input.Length; i++)
            {
                var b = ByteToBin(input[i], 8).ToCharArray();
                for (int j = 0; j < 8; j++)
                {
                    result[(8 * i) + j] = int.Parse(b[j].ToString());
                }
            }
            return result;
        }
    }

    

}
