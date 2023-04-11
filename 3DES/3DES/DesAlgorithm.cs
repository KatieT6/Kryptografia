using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES
{
    public class DesAlgorithm
    {

        //Pierwsza permutacja bloku z IPArray
        public static byte[] InitialPermutation(byte[] input)
        {
            byte[] output = new byte[8];

            // Permute the input block according to the IP table
            for (int i = 0; i < 64; i++)
            {
                int bitIndex = Arrays.IPArray[i] - 1;  // The bit index in the input block
                int byteIndex = bitIndex / 8;   // The byte index in the input block
                int bitMask = 1 << (bitIndex % 8); // The bit mask for the corresponding bit

                // If the bit is set in the input block, set it in the output block as well
                if ((input[byteIndex] & bitMask) != 0)
                {
                    int outputByteIndex = i / 8;
                    int outputBitIndex = i % 8;
                    output[outputByteIndex] |= (byte)(1 << outputBitIndex);
                }
            }

            return output;
        }
    }
}
