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
        private byte[] newKey = new byte[8];
        private byte[][] subKeys = new byte[16][];

        public Key()
        {
            this.newKey = GenerateKey(newKey);
            this.SubKeys = GenerateSubKeys(newKey);
        }

        public byte[] NewKey { get => newKey; }
        public byte[][] SubKeys { get => subKeys; set => subKeys = value; }

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

        
        

        //First returned value - permuted Key, second is generated SubKey
        public static byte[][] GenerateSubKeys(byte[] key)
        {
            // Permute the key using the PC-1 table
            byte[] permutedKey = Permutation.Permute(key, PC_Arrays.PC1);

            // Split the permuted key into two 28-bit halves
            byte[] c = new byte[4];
            byte[] d = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                if (i < 2)
                { 
                    c[i] = permutedKey[i]; 
                }
                
                else
                { 
                    d[i] = permutedKey[i]; 
                }
                
            }

            // Generate the 16 subkeys by rotating the halves and permuting using the PC-2 table
            byte[][] subKeys = new byte[16][];
            for (int i = 0; i < 16; i++)
            {
                // Rotate the halves by the appropriate amount
                int rotateAmount = PC_Arrays.Shifts[i];
                byte[] cRotated = RotateLeft(c, rotateAmount);
                byte[] dRotated = RotateLeft(d, rotateAmount);

                // Concatenate the rotated halves and permute using the PC-2 table
                byte[] cd = Concatenate(cRotated, dRotated);
                subKeys[i] = Permutation.Permute(cd, PC_Arrays.PC2);

                c = cRotated;
                d = dRotated;
            }

            return subKeys;
        }

        // Helper method to rotate a byte array to the left by a given number of bits
        private static byte[] RotateLeft(byte[] value, int amount)
        {
            byte[] rotated = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                int leftShift = amount % 8;
                int rightShift = 8 - leftShift;
                rotated[i] = (byte)(((value[i] << leftShift) & 0xFF) | ((value[(i + 1) % value.Length] >> rightShift) & 0xFF));
            }
            return rotated;
        }

        // Helper method to concatenate two byte arrays
        private static byte[] Concatenate(byte[] a, byte[] b)
        {
            byte[] concatenated = new byte[a.Length + b.Length];
            Array.Copy(a, 0, concatenated, 0, a.Length);
            Array.Copy(b, 0, concatenated, a.Length, b.Length);
            return concatenated;
        }

        //pierwsza próba 
        /* public static byte[][] GenerateSubkeys(byte[] key)
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
                 int shiftCount = c;

                 //jak nei zadział to zmienićfunkcję Shifts tak zeby zwrracała nowe wartości 
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
         }*/


    }
}
