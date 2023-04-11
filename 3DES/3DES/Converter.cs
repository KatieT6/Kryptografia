using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES
{
    public class Converter
    {
        public Converter() { }

        public static byte[] StringToBytesUTF8(string str)
        {
            byte[] bytes = new byte[str.Length * 2];
            int position = 0;

            foreach (char character in str)
            {
                bytes[position++] = (byte)((character & 0xFF00) >> 8);
                bytes[position++] = (byte)(character & 0x00FF);
            }

            return bytes;
        }

        public static string BytesToStringUTF8(byte[] bytes)
        {
            char[] buffer = new char[bytes.Length / 2];

            for (int i = 0; i < buffer.Length; i++)
            {
                int bpos = i * 2;
                char c = (char)(((bytes[bpos] & 0x00FF) << 8) + (bytes[bpos + 1] & 0x00FF));
                buffer[i] = c;
            }

            return new string(buffer);
        }
    }
}
