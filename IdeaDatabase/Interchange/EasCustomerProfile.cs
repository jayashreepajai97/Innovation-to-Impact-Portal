using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class EasCustomerProfile
    {
        public int UserID { get; set; }
        public string CallerId { get; set; }
        public string TokenMD5 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string CountryCode { get; set; }
        public string LanguageCode { get; set; }
        public string Locale { get; set; }

        public EasCustomerProfile(int customerId)
        {
            UserID = customerId;
            CallerId = string.Empty;
            TokenMD5 = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            EmailAddress = string.Empty;
            CountryCode = string.Empty;
            LanguageCode = string.Empty;
            Locale = string.Empty;
        }
    }
}