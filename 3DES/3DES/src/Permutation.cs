using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES
{
    public class Permutation
    {
        // Helper method to permute a byte array using a given permutation table
        public static byte[] Permute(byte[] value, byte[] table)
        {
            byte[] permuted = new byte[table.Length / 8];
            for (int i = 0; i < table.Length; i++)
            {
                int bitIndex = table[i] - 1;  // The bit index in the value
                int byteIndex = bitIndex / 8;   // The byte index in the value
                int bitMask = 1 << (bitIndex % 8); // The bit mask for the corresponding bit

                // If the bit is set in the value, set it in the permuted value as well
                if ((value[byteIndex] & bitMask) != 0)
                {
                    int permutedByteIndex = i / 8;
                    int permutedBitIndex = i % 8;
                    permuted[permutedByteIndex] |= (byte)(1 << (7 - permutedBitIndex));
                }
            }
            return permuted;
        }


        //Firs permutation IPArray
        public static byte[] InitialPermutation(byte[] input)
        {
            return Permute(input, Arrays.IPArray);
        }
    }
}
