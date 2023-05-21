using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _3DES.Logic
{
    public class DesTripleAlgorithm
    {
        private readonly string key1;
        private readonly string key2;
        private readonly string key3;
        private byte[] plainText;

        public DesTripleAlgorithm(string key1, string key2, string key3, byte[] plainText)
        {
            this.key1 = key1;
            this.key2 = key2;
            this.key3 = key3;
            this.plainText = plainText; 
        }

        public byte[] encrypt()
        {
            DesAlgorithm des1 = new DesAlgorithm(plainText, key1, false);
            byte[] plainTextBuffer = des1.Encrypt();

            DesAlgorithm des2 = new DesAlgorithm(plainTextBuffer, key2, true);
            plainTextBuffer = des2.Encrypt();

            DesAlgorithm des3 = new DesAlgorithm (plainTextBuffer, key3, false);
            plainTextBuffer = des3.Encrypt();

            return plainTextBuffer;
        }


        public byte[] decrypt()
        {
            DesAlgorithm des1 = new DesAlgorithm(plainText, key3, true);
            byte[] plainTextBuffer = des1.Encrypt();

            DesAlgorithm des2 = new DesAlgorithm(plainTextBuffer, key2, false);
            plainTextBuffer = des2.Encrypt();

            DesAlgorithm des3 = new DesAlgorithm(plainTextBuffer, key1, true);
            plainTextBuffer = des3.Encrypt();

            return plainTextBuffer;
        }
    }
}
