using IdeaDatabase.Enums;
using IdeaDatabase.Validation;
using Newtonsoft.Json;
using Responses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InnovationPortalService
{
    [JsonObject]
    public class RESTAPICustomerData : ValidableObject
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
        [RequiredValidation]
        public bool? EmailOffers { get; set; }

        [JsonProperty]
        [EnumValidation(typeof(PrimaryUseType))]
        public string PrimaryUse { get; set; }
        
        [JsonProperty]
        [StringValidation(0, 35, false)]
        public string City { get; set; }

        [JsonProperty]
        [StringValidation(0, 90, false)]
        public string Company { get; set; }

        [JsonProperty]
        [RequiredValidation]
        public bool? ActiveHealth { get; set; }

        public static explicit operator CustomerData(RESTAPICustomerData customer)
        {
            CustomerData ret = new CustomerData()
            {
                EmailAddress = null,
                EmailConsent = customer.EmailOffers.HasValue ? (customer.EmailOffers.Value ? EmailConsentType.Y.ToString() : EmailConsentType.N.ToString()) : EmailConsentType.N.ToString(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Language = customer.Language,
                Country = customer.Country,
                PrimaryUse = customer.PrimaryUse,
                City = customer.City,
                CompanyName = customer.Company,
                ActiveHealth = customer.ActiveHealth
            };
           
            return ret;
        }

        protected override void Validate(List<ValidationResult> results, ResponseBase r)
        {
            base.Validate(results, r);

            if(string.IsNullOrEmpty(Company))
            {
                if (!string.IsNullOrEmpty(PrimaryUse) && PrimaryUse != PrimaryUseType.Item002.ToString())
                {
                    r.ErrorList.Add(new RequiredValidationFault("Company"));
                }
                return;
            }
            
            if (Company.IndexOfAny(@".,;:-_*""".ToCharArray()) == 0)
            {
                r.ErrorList.Add(new FormatValidationFault("Company", "Cannot start with any of: .,;:-_*\""));
            }           
        }
    }
}