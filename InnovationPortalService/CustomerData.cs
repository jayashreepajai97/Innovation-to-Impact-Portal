using IdeaDatabase.Enums;
using IdeaDatabase.Validation;
using InnovationPortalService.HPID;
//using InnovationPortalService.HPPService;
using Newtonsoft.Json;
using Responses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InnovationPortalService
{
    public class CustomerData : ValidableObject
    {
        private string _country;
        private string _language;

        [JsonProperty]
        [EnumValidation(typeof(CountryCodeType), true)]
        public string Country
        {
            get
            {
                if (string.IsNullOrEmpty(_country))
                    return null;
                return _country.ToUpper();
            }
            set { _country = value; }
        }

        [StringValidation(1, 255, true, "|")]
        public string FirstName { get; set; }

        [JsonProperty]
        [StringValidation(1, 255, true, "|")]
        public string LastName { get; set; }

        [JsonProperty]
        [EnumValidation(typeof(LanguageCodeType), true)]
        public string Language
        {
            get
            {
                if (string.IsNullOrEmpty(_language))
                    return null;
                return _language.ToLower();
            }
            set { _language = value; }
        }

        [JsonProperty]
        [EnumValidation(typeof(EmailConsentType), true)]
        public string EmailConsent { get; set; }

        [JsonProperty]
        [EnumValidation(typeof(PrimaryUseType), true)]
        public string PrimaryUse { get; set; }

        [JsonProperty]
        [EmailAddressValidation(1, 60, true, Unicode: false)]
        public string EmailAddress { get; set; }

        [JsonProperty]
        [StringValidation(0, 35, false)]
        public string City { get; set; }

        [JsonProperty]
        public string CompanyName { get; set; }

        [JsonProperty]
        public bool? ActiveHealth { get; set; }

        [JsonIgnore]
        public bool UpdateCity
        {
            get
            {
                if (this.City == null)
                    return false;
                return true;
            }
        }

        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public string DisplayName { get; set; }
        [JsonIgnore]
        public string Locale { get; set; }
        [JsonIgnore]
        public string Gender { get; set; }
        [JsonIgnore]
        public List<Address> Addresses { get; set; }
        [JsonIgnore]
        public List<PhoneNumber> PhoneNumbers { get; set; }

        protected override void Validate(List<ValidationResult> results, ResponseBase r)
        {
            base.Validate(results, r);
            if (!string.IsNullOrEmpty(PrimaryUse) && PrimaryUse != PrimaryUseType.Item002.ToString())
            {
                if (CompanyName != null)
                {
                    if (CompanyName.Length < 1 || CompanyName.Length > 90)
                        r.ErrorList.Add(new LengthValidationFault("CompanyName", 1, 90));
                    else if (CompanyName.IndexOfAny(@".,;:-_*""".ToCharArray()) == 0)
                        r.ErrorList.Add(new FormatValidationFault("CompanyName", "Cannot start with any of: .,;:-_*\""));
                }
                else
                    r.ErrorList.Add(new RequiredValidationFault("CompanyName"));
            }
        }

        //public static implicit operator modifyUserRequestType(CustomerData customer)
        //{
        //    modifyUserRequestType ret = new modifyUserRequestType()
        //    {
        //        profileCore = new profileCoreType()
        //        {
        //            email = customer.EmailAddress,
        //            firstName = customer.FirstName,
        //            lastName = customer.LastName,
        //            langCode = customer.Language.MapToHPPLangCode(),
        //            residentCountryCode = customer.Country,
        //            contactPrefEmail = customer.EmailConsent,
        //            segmentName = customer.PrimaryUse.HPPValue()
        //        },
        //        profileExtended = new profileExtendedType(),
        //    };
        //    if (customer.PrimaryUse == PrimaryUseType.Item002.ToString())
        //    {
        //        ret.profileExtended.homeCity = customer.City;
        //        ret.profileExtended.homeCountryCode = customer.Country;
        //    }
        //    else
        //    {
        //        ret.profileExtended.busCountryCode = customer.Country;
        //        ret.profileExtended.busCity = customer.City;
        //        ret.profileExtended.busCompanyName = customer.CompanyName;
        //    }
        //    return ret;
        //}
    }
}