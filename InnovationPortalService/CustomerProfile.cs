using Newtonsoft.Json;
using IdeaDatabase.Validation;
//using InnovationPortalService.HPPService;
using IdeaDatabase.Enums;

namespace InnovationPortalService
{
    [JsonObject]
    public class CustomerProfile : CustomerData
    {
        [JsonProperty]
        [StringValidation(8, 256, true, PasswordCheck: true, MatchRegexp: "(?=.{8,})((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])|(?=.*\\d)(?=.*[a-zA-Z])(?=.*[\\W_])|(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_])).*")]
        public string Password { get; set; }

        [StringValidation(5, 60, true, ForbiddenChars: "&*|\"")]
        public string UserName { get; set; }

        [JsonIgnore]
        public bool IsNewCustomer { get; set; }

        [JsonProperty]
        public bool SmartFriend { get; set; }

        [JsonIgnore]
        public string TimeOut { get; set; }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public bool ShouldSerializePassword()
        {
            return false;
        }

        //public static implicit operator CustomerProfile(getUserResultType customer)
        //{
        //    CustomerProfile ret = new CustomerProfile()
        //    {
        //        FirstName = customer.profileCore.firstName,
        //        LastName = customer.profileCore.lastName,
        //        Language = customer.profileCore.langCode.MapFromHPPLangCode(),
        //        EmailAddress = customer.profileCore.email,
        //        EmailConsent = customer.profileCore.contactPrefEmail,
        //        UserName = customer.profileIdentity.userId
        //    };

        //    string segmentName = customer.profileCore.segmentName;
        //    if (segmentName != null)
        //    {
        //        if (segmentName.CompareTo("002") == 0)
        //        {
        //            ret.City = customer.profileExtended.homeCity;
        //            ret.Country = customer.profileExtended.homeCountryCode;
        //        }
        //        else
        //        {
        //            ret.City = customer.profileExtended.busCity;
        //            ret.Country = customer.profileExtended.busCountryCode;
        //            ret.CompanyName = customer.profileExtended.busCompanyName;
        //        }
        //        ret.PrimaryUse = "Item" + segmentName;
        //    }
        //    else
        //        ret.PrimaryUse = null;

        //    return ret;
        //}
    }
}