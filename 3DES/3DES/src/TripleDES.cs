using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES
{
    public class TripleDES
    {
        private Key Key1;
        private Key Key2;
        private Key Key3;

        public TripleDES()
        {
            Key11 = new Key();
            Key21 = new Key();
            Key31 = new Key();
        }

        public Key Key11 { get => Key1; set => Key1 = value; }
        public Key Key21 { get => Key2; set => Key2 = value; }
        public Key Key31 { get => Key3; set => Key3 = value; }

        public byte[] AlgorithmBase(byte[] input, byte[][] subKeys1, byte[][] subKeys2, byte[][] subKeys3, bool isEncryption)
        {
            byte[] output;

            //encode
            if (isEncryption)
            {
                output = DesAlgorithm.Encrypt(input, subKeys1);
                output = DesAlgorithm.Decrypt(output, subKeys2);
                output = DesAlgorithm.Encrypt(output, subKeys3);
            }
            //decode
            else
            {
                output = DesAlgorithm.Decrypt(input, subKeys3);
                output = DesAlgorithm.Encrypt(output, subKeys2);
                output = DesAlgorithm.Decrypt(output, subKeys1);
            }

            return output;
        }
    }

}
