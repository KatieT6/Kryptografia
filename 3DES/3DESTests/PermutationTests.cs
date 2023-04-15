using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3DES.src;

namespace _3DES.Tests
{
    [TestClass()]
    public class PermutationTests
    {
        [TestMethod()]
        public void TestPermuteWithPC1()
        {
            byte[] input = new byte[8] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            int[] pc1Table = new int[56]
            {
            57,   49,    41,   33,    25,    17,    9,
            1,   58,    50,   42,    34,    26,   18,
            10,    2,    59,   51,    43,    35,   27,
            19,   11,     3,   60,    52,    44,   36,
            63,   55,    47,   39,    31,    23,   15,
            7,   62,    54,   46,    38,    30,   22,
            14,    6,    61,   53,    45,    37,   29,
            21,   13,     5,   28,    20,    12,    4
            };
            byte[] expectedOutput = new byte[7] { 0x48, 0x14, 0x7D, 0xC4, 0x8D, 0x91, 0xC1 };

            byte[] output = Permutation.Permute(input, pc1Table);

            Assert.AreEqual(expectedOutput, output);
        }

    }
}