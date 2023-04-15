using Microsoft.VisualStudio.TestTools.UnitTesting;
using _3DES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using _3DESTests;

namespace _3DES.Tests
{
    [TestClass()]
    public class KeyTests : TestClass
    {

        [TestMethod()]
        public void PermutationPC1Test()
        {
            Console.WriteLine("TESTED key: " + BitConverter.ToString(input));
            
            for (int i = 0; i < subKeys.Length; i++) 
            {
                Assert.AreEqual(6, subKeys[i].Length, "Arrays have different lengths");
                Console.WriteLine("PERMUTED key: " + BitConverter.ToString(subKeys[i]));
            }

        }
    }

}
