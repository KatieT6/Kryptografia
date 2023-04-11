using Microsoft.VisualStudio.TestTools.UnitTesting;
using _3DES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _3DES.Tests
{
    [TestClass()]
    public class KeyTests
    {
        private static readonly byte[] testKey = new byte[] { 0x13, 0x34, 0x57, 0x79, 0x9B, 0xBC, 0xDF, 0xF1 };
        private static readonly byte[] expectedKey = new byte[] { 0x1F, 0x1F, 0x1F, 0x1F, 0x0E, 0x0E, 0x0E };
        private static readonly byte[] permutedKey = Key.PermutationPC1(testKey);


        [TestMethod()]
        public void PermutationPC1Test()
        {
            Assert.AreEqual(expectedKey.Length, permutedKey.Length, "Arrays have different lengths");
            Console.WriteLine("TESTED key: " + BitConverter.ToString(testKey));
            Console.WriteLine("Expected key: " + BitConverter.ToString(expectedKey));
            Console.WriteLine("Permuted key: " + BitConverter.ToString(permutedKey));
            CollectionAssert.AreEqual(expectedKey, permutedKey);
        }
    }

}
