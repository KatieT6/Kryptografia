using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES
{
    public class DesAlgorithm
    {

        #region Helper methods

        // Helper method to expand a 32-bit value to a 48-bit value using the E table
        private static byte[] Expand(byte[] value)
        {
            byte[] expanded = new byte[6];

            for (int i = 0; i < 48; i++)
            {
                int bitIndex = Arrays.E_bit_selectionArrays[i] - 1;   // The bit index in the expanded value
                int byteIndex = bitIndex / 8;   // The byte index in the original value
                int bitMask = 1 << (bitIndex % 8); // The bit mask for the corresponding bit

                // If the bit is set in the original value, set it in the expanded value as well
                if ((value[byteIndex] & bitMask) != 0)
                {
                    int expandedByteIndex = i / 8;
                    int expandedBitIndex = i % 8;
                    expanded[expandedByteIndex] |= (byte)(1 << expandedBitIndex);
                }
            }

            return expanded;
        }


        // Helper method to XOR two byte arrays
        private static byte[] Xor(byte[] a, byte[] b)
        {
            byte[] result = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = (byte)(a[i] ^ b[i]);
            }
            return result;
        }

        // Helper method to apply the S-boxes to a 48-bit value
        private static byte[] SBox(byte[] value)
        {
            byte[] sboxed = new byte[4];
            for (int i = 0; i < 8; i++)
            {
                // Extract the 6-bit value for this S-box
                int sboxInput = 0;
                for (int j = 0; j < 6; j++)
                {
                    int bitIndex = i * 6 + j;
                    int byteIndex = bitIndex / 8;
                    int bitMask = 1 << (bitIndex % 8);
                    if ((value[byteIndex] & bitMask) != 0)
                    {
                        sboxInput |= (1 << (5 - j));
                    }
                }

                // Apply the S-box and store the result in the sboxed array
                int sboxOutput = Arrays.S_boxArrays[i, sboxInput];
                for (int j = 0; j < 4; j++)
                {
                    int bitIndex = i * 4 + j;
                    int byteIndex = bitIndex / 8;
                    int bitMask = 1 << (bitIndex % 8);
                    if ((sboxOutput & (1 << (3 - j))) != 0)
                    {
                        sboxed[byteIndex] |= (byte)bitMask;
                    }
                }
            }
            return sboxed;
        }

        #endregion

        public static byte[] Encrypt(byte[] permutedMessage, byte[][] subkeys)
        {
            byte[] l = new byte[4];
            byte[] r = new byte[4];
            Array.Copy(permutedMessage, 0, l, 0, 4);
            Array.Copy(permutedMessage, 4, r, 0, 4);

            // Perform 16 rounds of encryption
            for (int i = 0; i < 16; i++)
            {
                // Save the previous values of L and R
                byte[] previousL = l;
                byte[] previousR = r;

                // Compute the new value of R using the expansion table, the subkey, and the S-boxes
                byte[] expanded = Expand(r);
                byte[] subkey = subkeys[i];
                byte[] xored = Xor(expanded, subkey);
                byte[] sboxed = SBox(xored);
                byte[] permutedSboxed = Permutation.Permute(sboxed, Arrays.S_boxPermutation);

                // Compute the new value of L by XORing it with the permuted and S-boxed value of R
                l = previousR;
                r = Xor(previousL, permutedSboxed);
            }

            // Concatenate the final values of L and R and permute them using the IP^-1 table
            byte[] combined = new byte[8];
            Array.Copy(r, 0, combined, 0, 4);
            Array.Copy(l, 0, combined, 4, 4);

            byte[] encrypted = new byte[8];
            for (int i = 0; i < 64; i++)
            {
                int bitIndex = Arrays.IPInverseArray[i] - 1;  // The bit index in the combined message
                int byteIndex = bitIndex / 8;   // The byte index in the combined message
                int bitMask = 1 << (bitIndex % 8); // The bit mask for the corresponding bit

                // If the bit is set in the combined message, set it in the encrypted message as well
                if ((combined[byteIndex] & bitMask) != 0)
                {
                    int encryptedByteIndex = i / 8;
                    int encryptedBitIndex = i % 8;
                    encrypted[encryptedByteIndex] |= (byte)(1 << encryptedBitIndex);
                }
            }

            return encrypted;
        }

        public static byte[] Decrypt(byte[] encryptedMessage, byte[][] subkeys)
        {
            // Permute the encrypted message using the initial permutation table
            byte[] permutedMessage = Permutation.Permute(encryptedMessage, Arrays.IPArray);

            // Split the permuted message into L and R halves
            byte[] l = new byte[4];
            byte[] r = new byte[4];
            Array.Copy(permutedMessage, 0, l, 0, 4);
            Array.Copy(permutedMessage, 4, r, 0, 4);

            // Perform 16 rounds of decryption in reverse order
            for (int i = 15; i >= 0; i--)
            {
                // Save the previous values of L and R
                byte[] previousL = l;
                byte[] previousR = r;

                // Compute the new value of L using the permuted and S-boxed value of R
                byte[] expanded = Expand(previousL);
                byte[] subkey = subkeys[i];
                byte[] xored = Xor(expanded, subkey);
                byte[] sboxed = SBox(xored);
                byte[] permutedSboxed = Permutation.Permute(sboxed, Arrays.S_boxPermutation);
                l = Xor(previousR, permutedSboxed);

                // Compute the new value of R by XORing it with the permuted and S-boxed value of L
                r = previousL;
            }

            // Concatenate the final values of L and R and permute them using the IP^-1 table
            byte[] combined = new byte[8];
            Array.Copy(r, 0, combined, 0, 4);
            Array.Copy(l, 0, combined, 4, 4);

            byte[] decrypted = new byte[8];
            for (int i = 0; i < 64; i++)
            {
                int bitIndex = Arrays.IPInverseArray[i] - 1;  // The bit index in the combined message
                int byteIndex = bitIndex / 8;   // The byte index in the combined message
                int bitMask = 1 << (bitIndex % 8); // The bit mask for the corresponding bit

                // If the bit is set in the combined message, set it in the decrypted message as well
                if ((combined[byteIndex] & bitMask) != 0)
                {
                    int decryptedByteIndex = i / 8;
                    int decryptedBitIndex = i % 8;
                    decrypted[decryptedByteIndex] |= (byte)(1 << decryptedBitIndex);
                }
            }

            return decrypted;
        }







    }

}
