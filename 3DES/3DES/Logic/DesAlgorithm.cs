using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES
{
    public class DesAlgorithm
    {

        private string plainText;
        private readonly string key;
        private readonly bool isEncrypted;

        public DesAlgorithm(string plainText, string key, bool isEncrypted)
        {
            this.plainText = plainText;
            this.key = key;
            this.isEncrypted = isEncrypted;
        }



        #region Encode
        public static int[] EncodeBlock(int[] block, int[] key, bool isEncrypted)
        {
            // Generating 48-bits keys for all 16 rounds
            int[][] key_48 = generateRoundKeys(key);

            // Initial permutation on block of plain text
            int[] permutedBlock = Permute(block, Arrays.IPArray);

            // Dividing 64-bits block in 2 halves: LPT and RPT
            int[] leftHalf = new int[32];
            int[] rightHalf = new int[32];
            Array.Copy(permutedBlock, 0, leftHalf, 0, 32);
            Array.Copy(permutedBlock, 32, rightHalf, 0, 32);

            int[] temp = new int[32];
            int[] fOutput;

            // does all 16 rounds of DES
            for (int i = 0; i < 16; i++)
            {
                if (!isEncrypted)
                {
                    fOutput = f(rightHalf, key_48[i]);
                }
                else
                {
                    fOutput = f(rightHalf, key_48[15 - i]);
                }

                // 32-bits LPT is XORed with 32-bits P-Box output from P-Box Permutation
                leftHalf = Xor(leftHalf, fOutput);

                // swaps left and right halves
                Array.Copy(leftHalf, 0, temp, 0, 32);
                Array.Copy(rightHalf, 0, leftHalf, 0, 32);
                Array.Copy(temp, 0, rightHalf, 0, 32);
            }

            // Rejoin 2 halves after DES to 64-bits block
            int[] result = new int[64];
            Array.Copy(rightHalf, 0, result, 0, 32);
            Array.Copy(leftHalf, 0, result, 32, 32);

            // Final Permutation
            return Permute(result, Arrays.IPInverseArray);
        }


        #endregion

        #region Encrypt

        public string Encrypt()
        {
            List<int[]> blocks64bits = new List<int[]>();

            if (!isEncrypted)
            {
                int remainder = plainText.Length % 8;
                if (remainder != 0)
                {
                    for (int i = 0; i < (8 - remainder); i++)
                        plainText = "~" + plainText;
                }
            }

            byte[] bytePlainText;
            if (!isEncrypted)
            {
                bytePlainText = Encoding.UTF8.GetBytes(plainText);
            }
            else
            {
                bytePlainText = Converter.HexFormat(plainText);
            }

            for (int i = 0; i < bytePlainText.Length; i += 8)
            {
                byte[] oneBlock = new byte[8];
                Array.Copy(bytePlainText, i, oneBlock, 0, 8);
                blocks64bits.Add(Converter.ByteArrToIntArr(oneBlock));
            }

            byte[] byteKey = Converter.HexFormat(key);
            int[] binKey = Converter.ByteArrToIntArr(byteKey);

            for (int i = 0; i < blocks64bits.Count; i++)
            {
                blocks64bits[i] = EncodeBlock(blocks64bits[i], binKey, isEncrypted);
            }

            bool isFirstBlock = true;

            StringBuilder encrypted = new StringBuilder();
            for (int i = 0; i < blocks64bits.Count; i++)
            {
                byte[] bytes = new byte[8];
                for (int j = 0; j < 8; j++)
                {
                    StringBuilder oneByte = new StringBuilder();
                    for (int k = 0; k < 8; k++)
                    {
                        oneByte.Append(blocks64bits[i][(8 * j) + k]);
                    }
                    int decimalValue = Convert.ToInt32(oneByte.ToString(), 2);
                    bytes[j] = (byte)decimalValue;
                }
                if (!isEncrypted)
                {
                    encrypted.Append(Converter.ByteArrayToHex(bytes));
                }
                else
                {
                    if (isFirstBlock)
                    {
                        string[] s = Encoding.UTF8.GetString(bytes).Split("");
                        StringBuilder result = new StringBuilder();
                        foreach (string n in s)
                        {
                            if (n != "~")
                                result.Append(n);
                        }
                        encrypted.Append(result);
                        isFirstBlock = false;
                    }
                    else
                    {
                        encrypted.Append(Encoding.UTF8.GetString(bytes));
                    }
                }
            }

            return encrypted.ToString();
        }

        #endregion



        #region Helper methods

        public static int[][] generateRoundKeys(int[] key)
        {
            int[] permutedChoice1 = Permute(key, PC_Arrays.PC1);
            int[] leftHalf = new int[28];
            int[] rightHalf = new int[28];
            Array.Copy(permutedChoice1, 0, leftHalf, 0, 28);
            Array.Copy(permutedChoice1, 28, rightHalf, 0, 28);

            int[][] roundKeys = new int[16][];
            for (int i = 0; i < 16; i++)
            {
                leftHalf = RotateLeft(leftHalf, PC_Arrays.Shifts[i]);
                rightHalf = RotateLeft(rightHalf, PC_Arrays.Shifts[i]);
                int[] temp = new int[56];
                Array.Copy(leftHalf, 0, temp, 0, 28);
                Array.Copy(rightHalf, 0, temp, 28, 28);
                roundKeys[i] = Permute(temp, PC_Arrays.PC2);
            }

            return roundKeys;
        }



        /**
 * Generic Permutation
 *
 * @param input            An Array of ints values that will be permuted
 * @param permutationTable Permutation Table of byte values
 * @return Permuted Array with size of Permutation Table
 */
        public static int[] Permute(int[] input, byte[] permutationTable)
        {
            int[] result = new int[permutationTable.Length];
            for (int i = 0; i < permutationTable.Length; i++)
                result[i] = input[permutationTable[i] - 1];
            return result;
        }

        /**
         * Both arrays should be the same length
         *
         * @param t1 first Array of int values
         * @param t2 second Array of int values
         * @return First Array XORed with Second Array
         */
        private static int[] Xor(int[] t1, int[] t2)
        {
            int[] result = new int[t1.Length];
            for (int i = 0; i < t1.Length; i++)
            {
                result[i] = t1[i] ^ t2[i];
            }
            return result;
        }

        /**
         * S-Box Substituion
         *
         * @param RPT 48-bits
         * @return RPT 32-bits
         */
        private static int[] SBoxSubstitution(int[] RPT)
        {
            int[] result = new int[32];

            // dividing into 8 blocks, each with 6-bits size
            for (int i = 0; i < 8; i++)
            {
                // outer elements values equals to binary number of row of S-Box table
                int[] row = { RPT[6 * i], RPT[(6 * i) + 5] };

                // inner elements values equals to binary number of column of S-Box table
                int[] column = { RPT[(6 * i) + 1], RPT[(6 * i) + 2], RPT[(6 * i) + 3], RPT[(6 * i) + 4] };

                // Binary number of row and column of S-Box table
                string binRow = "" + row[0] + row[1];
                string binColumn = "" + column[0] + column[1] + column[2] + column[3];

                // Decimal number of row and column of S-Box table
                int decRow = Convert.ToInt32(binRow, 2);
                int decColumn = Convert.ToInt32(binColumn, 2);

                // Converting element from S-Box table to binary arr with eventually padding

                //POPRAWIĆ POTEM

                byte nr = 1;//Arrays.SBox[i][(decRow * 16) + decColumn];
                int[] bin = Converter.BinStringToIntArr(Converter.ByteToBin(nr, 4));

                // Update result array
                Array.Copy(bin, 0, result, (4 * i), 4);
            }

            return result;
        }


        /**
 * Circular shift
 *
 * @param input An 28-bits Array that will be shifted
 * @param n     Number of shifts to the left
 * @return Shifted Array of 28-bits
 */
        private static int[] RotateLeft(int[] input, int n)
        {
            int[] result = new int[input.Length];
            Array.Copy(input, result, input.Length);
            for (int i = 0; i < n; i++)
            {
                int firstBit = result[0];
                // left shift by 1 shift bit
                Array.Copy(result, 1, result, 0, input.Length - 1);
                result[input.Length - 1] = firstBit;
            }
            return result;
        }


        /**
  * Feistel cipher
  *
  * @param RPT 32-bits RPT of the given round
  * @param key 48-bits key of the given round
  * @return 32-bits output of P-Box Permutation
  */
        private static int[] f(int[] RPT, int[] key)
        {
            // Expansion Permutation: 32-bits RTP => 48-bits RTP
            int[] expandedRPT = Permute(RPT, Arrays.E_bit_selectionArrays);

            // 48-bits RPT XORed 48-bits key
            int[] xored = Xor(expandedRPT, key);

            // S-Box substitution: 48-bits RPT => 32-bits RPT (as same size as LPT size)
            int[] sboxed = SBoxSubstitution(xored);

            // P-Box Permutation
            return Permute(sboxed, Arrays.S_boxPermutation);
        }

        #endregion

        





    }

}
