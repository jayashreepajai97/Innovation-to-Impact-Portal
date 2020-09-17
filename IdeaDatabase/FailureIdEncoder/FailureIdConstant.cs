using System;
using System.Collections.Generic;

namespace IdeaDatabase.FailureIdEncoder
{
    internal static class FailureIdConstant
    {
        public const char INVALID_CHAR = ' ';

        private static char[] alphaDefaultV1 =  {
                                                    '0','1','2','3','4','5','6','7',
                                                    '8','9','A','B','C','D','E','F',
                                                    'G','H','I','J','K','L','M','N',
                                                    'O','P','Q','R','S','T','U','V'
                                                };

        private static char[] alphaRandomV1 =       {
                                                    INVALID_CHAR,'J',INVALID_CHAR,'N',INVALID_CHAR,'Q',INVALID_CHAR,'S',
                                                    'F','W','8',INVALID_CHAR,'3',INVALID_CHAR,'V',INVALID_CHAR,
                                                    '5',INVALID_CHAR,INVALID_CHAR,'K',INVALID_CHAR,'O',INVALID_CHAR,'R',
                                                    'C',INVALID_CHAR,'G','X','1',INVALID_CHAR,'2',INVALID_CHAR,
                                                    INVALID_CHAR,'4',INVALID_CHAR,'U',INVALID_CHAR,'L',INVALID_CHAR,'P',
                                                    'A',INVALID_CHAR,'D',INVALID_CHAR,'H','Y','6',INVALID_CHAR,
                                                    INVALID_CHAR,INVALID_CHAR,'7',INVALID_CHAR,'9',INVALID_CHAR,'Z','M',
                                                    'T',INVALID_CHAR,'B','0','E',INVALID_CHAR,'I',INVALID_CHAR
                                                };

        public const int YEAR_DEFAULT = 2000;

        /// <summary>
        /// Get default alphanumeric characters
        /// </summary>
        public static char[] AlphanumericDefault
        {
            get { return alphaDefaultV1; }
        }

        /// <summary>
        /// Get random alphanumeric characters
        /// </summary>
        public static char[] AlphanumericRandom
        {
            get { return alphaRandomV1; }
        }


        public static char[] alphaDefaultV2 =   {
                                                    '0','1','2','3','4','5','6','7',
                                                    '8','9','A','B','C','D','E','F',
                                                    'G','H','J','K','L','M','N','P',
                                                    'Q','R','S','T','U','V','W','X'
                                        };

        public static Dictionary<char, byte> alphaRandomV2 = new Dictionary<char, byte>()
            {
                {'0', 1}, {'1', 18}, {'2', 48}, {'3', 47}, {'4', 19},  {'5', 42},
                {'6', 51}, {'7', 12}, {'8', 33}, {'9', 56}, {'A', 39}, {'B', 30},
                {'C', 62}, {'D', 63}, {'E', 60},  {'F', 0}, {'G', 4}, {'H', 8},
                {'I', 16}, {'J', 15}, {'K', 21}, {'L', 3}, {'M', 61}, {'N', 52},
                {'O', 7},  {'P', 57}, {'Q', 49}, {'R', 58}, {'S', 45}, {'T', 41},
                {'U', 55}, {'V', 35}, {'W', 59}, {'X', 31}, {'Y', 2}, {'Z', 32}
            };

        public static int Version3 = 3;

        public static byte CHECKSUM_MASK = 0x66;

        public static byte GetIndexFromChar(char[] chArray, char cAlph)
        {
            int idx = -1;
            for (int i = 0; i < chArray.Length; i++)
            {
                if (cAlph == chArray[i])
                {
                    idx = i;
                    break;
                }
            }
            return (byte)idx;
        }

        public static byte GetChecksum(int[] HPSNuEncoded, int[] WSTRTEncoded, int[] TSTDTEncoded, int FAILREncoded)
        {
            byte[] ENCArray = new byte[13];
            ENCArray[0] = (byte)((HPSNuEncoded[5] << 2) | (HPSNuEncoded[6] >> 4));
            ENCArray[1] = (byte)(((HPSNuEncoded[6] << 4) & 0xF0) | (HPSNuEncoded[7] >> 2));
            ENCArray[2] = (byte)(((HPSNuEncoded[7] << 6) & 0xC0) | (HPSNuEncoded[8]));
            ENCArray[3] = (byte)((HPSNuEncoded[9] << 2) | (WSTRTEncoded[0]) >> 4);
            ENCArray[4] = (byte)(((WSTRTEncoded[0] << 4) & 0xF0) | (WSTRTEncoded[1]) >> 5);
            ENCArray[5] = (byte)(((WSTRTEncoded[1] << 3) & 0xF8) | (TSTDTEncoded[0]) >> 3);
            ENCArray[6] = (byte)(((TSTDTEncoded[0] << 5) & 0xE0) | (TSTDTEncoded[1]) >> 4);
            ENCArray[7] = (byte)(((TSTDTEncoded[1] << 4) & 0xF0) | (HPSNuEncoded[0]) >> 2);
            ENCArray[8] = (byte)(((HPSNuEncoded[0] << 6) & 0xC0) | (HPSNuEncoded[1]));
            ENCArray[9] = (byte)((HPSNuEncoded[2] << 2) | (HPSNuEncoded[3] >> 4));
            ENCArray[10] = (byte)(((HPSNuEncoded[3] << 4) & 0xF0) | (HPSNuEncoded[4] >> 2));
            ENCArray[11] = (byte)(((HPSNuEncoded[4] << 6) & 0xC0) | (FAILREncoded >> 6));
            ENCArray[12] = (byte)((FAILREncoded & 0x3F) << 2);

            byte CHECKSum = CHECKSUM_MASK;
            for (byte i = 0; i < ENCArray.Length; i++)
            {
                CHECKSum ^= ENCArray[i];
            }

            return CHECKSum;
        }
    }
}