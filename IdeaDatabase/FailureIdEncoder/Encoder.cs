using System;
using Responses;
using System.Text;
using IdeaDatabase.Responses;
using IdeaDatabase.Interchange;

namespace IdeaDatabase.FailureIdEncoder
{
    public class Encoder : IEncode
    {
        public FailureIdEncoderResponse FailureIdEncoder(string HpSerialNumber, string FailureCode, DateTime StartDate, DateTime TestDate)
        {
            FailureIdEncoderResponse response = new FailureIdEncoderResponse();
            FailureIdInterchange fi = new FailureIdInterchange();

             
            if (string.IsNullOrEmpty(FailureCode))
            {
                response.ErrorList.Add(Faults.EmptyFailureCode);
                return response;
            }

            if (StartDate == null || StartDate == DateTime.MinValue)
            {
                response.ErrorList.Add(Faults.InvalidStartDate);
                return response;
            }

            if (TestDate == null || TestDate == DateTime.MinValue)
            {
                response.ErrorList.Add(Faults.InvalidTestDate);
                return response;
            }

            fi.FailureIdStartDate = StartDate;
            fi.TestDate = TestDate;
            fi.FailureIdVersion = FailureIdConstant.Version3.ToString();
            fi.FailureCode = FailureCode;
            fi.HpSerialNumber = HpSerialNumber;
            EncodeWarrantyInfo(fi);

            if (!string.IsNullOrEmpty(fi.FailureIdTag))
                response.FailureId = fi.FailureIdTag;
            else
                response.ErrorList.Add(Faults.FailedToGenerateId);

            return response;
        }

        private void EncodeWarrantyInfo(FailureIdInterchange wi)
        {
            byte[] ENCArray = new byte[13];

            char[] HPSN = wi.HpSerialNumber.ToCharArray();
            int[] HPSNuEncoded = new int[HPSN.Length];

            for (int i = 0; i < HPSN.Length; i++)
            {
                HPSNuEncoded[i] = FailureIdConstant.alphaRandomV2[HPSN[i]];
            }

            // Split and encode Warranty Start Date
            int[] WSTRTEncoded = new int[2];
            WSTRTEncoded[0] = wi.FailureIdStartDate.Year - FailureIdConstant.YEAR_DEFAULT;
            WSTRTEncoded[1] = wi.FailureIdStartDate.DayOfYear;

            // Split and encode Test Date
            int[] TSTDTEncoded = new int[2];
            TSTDTEncoded[0] = wi.TestDate.Year - FailureIdConstant.YEAR_DEFAULT;
            TSTDTEncoded[1] = wi.TestDate.DayOfYear;

            // Split and encode Failure Code
            //int[] FAILREncoded = new int[1];
            //int iFCode = Convert.ToInt32(wi.FailureCode);
            //int FAILREncoded = Convert.ToInt32(wi.FailureCode);
            int FAILREncoded = int.Parse(wi.FailureCode, System.Globalization.NumberStyles.HexNumber);

            int iFRUBefore = (FAILREncoded & 0xFF00);
            int iFRU = iFRUBefore >> 8;
            int iError = (FAILREncoded & 0x00FF);

            byte CHECKSum = FailureIdConstant.GetChecksum(HPSNuEncoded, WSTRTEncoded, TSTDTEncoded, FAILREncoded);

            byte[] WTYTag = new byte[24];

            WTYTag[0] = (byte)(HPSNuEncoded[5] >> 1);
            WTYTag[1] = (byte)(((HPSNuEncoded[5] << 4) & 0x10) | (HPSNuEncoded[6] >> 2));
            WTYTag[2] = (byte)(((HPSNuEncoded[6] << 3) & 0x18) | (HPSNuEncoded[7] >> 3));
            WTYTag[3] = (byte)(((HPSNuEncoded[7] << 2) & 0x1C) | (HPSNuEncoded[8] >> 4));
            WTYTag[4] = (byte)(((HPSNuEncoded[8] << 1) & 0x1E) | (HPSNuEncoded[9] >> 5));
            WTYTag[5] = (byte)(HPSNuEncoded[9] & 0x1F);
            WTYTag[6] = (byte)(WSTRTEncoded[0] >> 1);
            WTYTag[7] = (byte)(((WSTRTEncoded[0] << 4) & 0x10) | (WSTRTEncoded[1] >> 5));
            WTYTag[8] = (byte)(WSTRTEncoded[1] & 0x1F);
            WTYTag[9] = (byte)(TSTDTEncoded[0] >> 1);
            WTYTag[10] = (byte)(((TSTDTEncoded[0] << 4) & 0x10) | (TSTDTEncoded[1] >> 5));
            WTYTag[11] = (byte)(TSTDTEncoded[1] & 0x1F);
            WTYTag[12] = (byte)(HPSNuEncoded[0] >> 1);
            WTYTag[13] = (byte)(((HPSNuEncoded[0] << 4) & 0x10) | (HPSNuEncoded[1] >> 2));
            WTYTag[14] = (byte)(((HPSNuEncoded[1] << 3) & 0x18) | (HPSNuEncoded[2] >> 3));
            WTYTag[15] = (byte)(((HPSNuEncoded[2] << 2) & 0x1C) | (HPSNuEncoded[3] >> 4));
            WTYTag[16] = (byte)(((HPSNuEncoded[3] << 1) & 0x1E) | (HPSNuEncoded[4] >> 5));
            WTYTag[17] = (byte)(HPSNuEncoded[4] & 0x1F);
            WTYTag[18] = (byte)(FAILREncoded >> 7);
            WTYTag[19] = (byte)((FAILREncoded >> 2) & 0x1F);
            WTYTag[20] = (byte)(((FAILREncoded << 3) & 0x18) | (CHECKSum >> 5));
            WTYTag[21] = (byte)(CHECKSum & 0x1F);
            WTYTag[22] = (byte)(FailureIdConstant.Version3 >> 5);
            WTYTag[23] = (byte)(FailureIdConstant.Version3 & 0x1f);


            StringBuilder sbTag = new StringBuilder();
            // Building first 4 blocks (5 characters)
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    sbTag.Append(FailureIdConstant.alphaDefaultV2[WTYTag[j + count]].ToString());
                }
                if (i < 3)
                {
                    sbTag.Append("-");
                    count += 6;
                }
            }

            wi.FailureIdTag = sbTag.ToString();
        }
    }
}