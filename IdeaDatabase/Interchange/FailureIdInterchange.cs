using System;

namespace IdeaDatabase.Interchange
{
    public class FailureIdInterchange
    {
        #region Variables

        private string hpSerialNumber;
        private DateTime failureIdStartDate;
        private DateTime testDate;
        private string failureCode;
        private string failureIdVersion;
        private string failureIdTag;
        private bool _checksumOK;
        private string _fru;
        private string _errorcode;
        private string _errormsg;
        private string _device;
        private string _deviceinc;

        #endregion

        #region Properties

        /// <summary>
        /// Get and Set HP Serial Number
        /// </summary>
        public string HpSerialNumber
        {
            get { return hpSerialNumber; }
            set { hpSerialNumber = value; }
        }

        /// <summary>
        /// Get and Set Warranty Start Date
        /// </summary>
        public DateTime FailureIdStartDate
        {
            get { return failureIdStartDate; }
            set { failureIdStartDate = value; }
        }

        /// <summary>
        /// Get and Set Test Date
        /// </summary>
        public DateTime TestDate
        {
            get { return testDate; }
            set { testDate = value; }
        }

        /// <summary>
        /// Get and Set Failure Code
        /// </summary>
        public string FailureCode
        {
            get { return failureCode; }
            set { failureCode = value; }
        }

        /// <summary>
        /// Get and Set Warranty Id Version
        /// </summary>
        public string FailureIdVersion
        {
            get { return failureIdVersion; }
            set { failureIdVersion = value; }
        }

        /// <summary>
        /// Get and Set WarrantyIdTag
        /// </summary>
        public string FailureIdTag
        {
            get { return failureIdTag; }
            set { failureIdTag = value; }
        }

        public bool ChecksumOK
        {
            get { return _checksumOK; }
            set { _checksumOK = value; }
        }

        public string FRU
        {
            get { return _fru; }
            set { _fru = value; }
        }

        public string ErrorCode
        {
            get { return _errorcode; }
            set { _errorcode = value; }
        }

        public string ErrorMessage
        {
            get { return _errormsg; }
            set { _errormsg = value; }
        }

        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }

        public string DeviceIncludes
        {
            get { return _deviceinc; }
            set { _deviceinc = value; }
        }

        #endregion
    }
}