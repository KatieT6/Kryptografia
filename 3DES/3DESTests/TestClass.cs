
using _3DES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DESTests
{
    public class TestClass
    {
        public static byte[] input = new byte[] { 0x13, 0x34, 0x57, 0x79, 0x9B, 0xBC, 0xDF, 0xF1 };
        public static byte[] expectedOutput = new byte[] { 0xF8, 0x33, 0x2A, 0x5F, 0x5A, 0xB9, 0x4F };
        public static byte[][] subKeys = Key.GenerateSubKeys(input);

        public TestClass()
        {
        }
    }
}
