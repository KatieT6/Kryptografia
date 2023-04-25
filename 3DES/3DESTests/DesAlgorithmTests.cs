using Microsoft.VisualStudio.TestTools.UnitTesting;
using _3DES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES.Tests
{
    [TestClass()]
    public class DesAlgorithmTests
    {
        private static string _key = "0E329232EA6D0D73";

        private static string _plainText = "Kasia ma kota";


        [TestMethod()]
        public void EncryptTest()
        {
            DesAlgorithm des = new DesAlgorithm(_plainText, _key, false);
            string encrypted = des.Encrypt();
            Console.WriteLine(encrypted);
        }

        [TestMethod()]
        public void DecryptTest()
        {
            DesAlgorithm des = new DesAlgorithm(_plainText, _key, false);
            string encrypted = des.Encrypt();
            DesAlgorithm desEncrypt = new DesAlgorithm(encrypted, _key, true);
            Console.WriteLine(desEncrypt.Encrypt());

        }
    }
}