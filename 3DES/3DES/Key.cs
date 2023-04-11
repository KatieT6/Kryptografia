using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace _3DES
{
    public class Key
    {
        private byte[] key_1 = new byte[8];

        public Key(byte[] key_1)
        {
            this.key_1 = GenerateKey(key_1);
        }

        public byte[] Key_1 { get => key_1;}

        //----------------------------------------------
        public static byte[] GenerateKey(byte[] key)
        {
            Random rnd = new Random();
            rnd.NextBytes(key);
            /*Console.WriteLine("The Random bytes are: ");
            for (int i = 0; i <= key.GetUpperBound(0); i++)
                Console.WriteLine("{0}: {1}", i, key[i]);*/
            string hexKey = BitConverter.ToString(key).Replace("-", "");
            Console.WriteLine("Key in hex format: " + hexKey);
            return key;
        }

        public static byte[] GenerateTESTKey()
        {
            string tmp = "1101001100110100010111110110100110001011101111001101010011010011";

            int numOfBytes = 8;
            byte[] test = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; i++)
            {
                test[i] = Convert.ToByte(tmp.Substring(8 * i, 8), 2);
            }
            
            string hexKey = BitConverter.ToString(test).Replace("-", "");
            Console.WriteLine("Key in hex format: " + hexKey);
            Console.WriteLine("FROM CALCULATOR: D3345F698BBCD4D3\n");
            return test;
        }


        public static byte[][] GenerateSubkeys(byte[] key)
        {
            byte[] permutedKey = new byte[7];
            for (int i = 0; i < 56; i++)
            {
                int bitIndex = PC_Arrays.PC1[i] - 1;  // The bit index in the input key
                int byteIndex = bitIndex / 8;   // The byte index in the input key
                int bitMask = 1 << (bitIndex % 8); // The bit mask for the corresponding bit

                // If the bit is set in the input key, set it in the permuted key as well
                if ((key[byteIndex] & bitMask) != 0)
                {
                    int permutedByteIndex = i / 8;
                    int permutedBitIndex = i % 8;
                    permutedKey[permutedByteIndex] |= (byte)(1 << permutedBitIndex);
                }
            }

            // Split the 56-bit intermediate key into two 28-bit halves
            byte[] c = new byte[4];
            byte[] d = new byte[4];
            Array.Copy(permutedKey, 0, c, 0, 4);
            Array.Copy(permutedKey, 4, d, 0, 4);

            byte[][] subkeys = new byte[16][];
            for (int i = 0; i < 16; i++)
            {
                // Shift the two halves of the key according to the shift schedule
                int shiftCount = PC_Arrays.Shifts[i];
                
                //jak nei zadziałą to zmienićfunkcję Shifts tak zeby zwrracała nowe wartości 
                ShiftLeft(c, shiftCount);
                ShiftLeft(d, shiftCount);

                // Combine the two halves and permute the result according to the PC2 table
                byte[] combined = new byte[7];
                Array.Copy(c, 0, combined, 0, 4);
                Array.Copy(d, 0, combined, 4, 4);

                byte[] subkey = new byte[6];
                for (int j = 0; j < 48; j++)
                {
                    int bitIndex = PC_Arrays.PC2[j] - 1;  // The bit index in the combined key
                    int byteIndex = bitIndex / 8;   // The byte index in the combined key
                    int bitMask = 1 << (bitIndex % 8); // The bit mask for the corresponding bit

                    // If the bit is set in the the combined key, set it in the subkey as well
                    if ((combined[byteIndex] & bitMask) != 0)
                    {
                        int subkeyByteIndex = j / 8;
                        int subkeyBitIndex = j % 8;
                        subkey[subkeyByteIndex] |= (byte)(1 << subkeyBitIndex);
                    }
                }

                subkeys[i] = subkey;
            }

            return subkeys;
        }


        // Helper method to shift the bits in an array of bytes to the left by a given count
        private static void ShiftLeft(byte[] array, int count)
        {
            // Calculate the number of whole bytes to shift
            int byteCount = count / 8;
            int bitCount = count % 8;

            // Shift the whole bytes
            for (int i = 0; i < array.Length - byteCount; i++)
            {
                array[i] = array[i + byteCount];
            }

            // Shift the remaining bits in the last byte
            byte lastByte = array[array.Length - byteCount - 1];
            for (int i = 0; i < bitCount; i++)
            {
                lastByte <<= 1;
                lastByte |= (byte)((lastByte >> 8) & 1);
            }
            array[array.Length - 1] = lastByte;
        }


     }
}
