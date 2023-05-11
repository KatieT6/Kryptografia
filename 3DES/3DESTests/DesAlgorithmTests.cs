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
        private byte[] _decryptedBuffer = Encoding.UTF8.GetBytes(_plainText);
        private byte[] _encryptedBuffer;


        [TestMethod()]
        public void EncryptTest()
        {
            DesAlgorithm des = new DesAlgorithm(_decryptedBuffer, _key, false);
            byte[] encrypted = des.Encrypt();
            Console.WriteLine(encrypted);
            Console.WriteLine(Encoding.UTF8.GetString(encrypted));
        }

        [TestMethod()]
        public void DecryptTest()
        {
            DesAlgorithm des = new DesAlgorithm(_decryptedBuffer, _key, false);
            byte[] encrypted = des.Encrypt();
            DesAlgorithm desEncrypt = new DesAlgorithm(encrypted, _key, true);
            _encryptedBuffer = desEncrypt.Encrypt();
            Console.WriteLine(_encryptedBuffer);
            Console.WriteLine(Encoding.UTF8.GetString(_encryptedBuffer));
            Assert.IsTrue(_encryptedBuffer.SequenceEqual(_decryptedBuffer));
        }
    }
}