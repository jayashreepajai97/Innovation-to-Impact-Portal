using Credentials;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using IdeaDatabase.Validation;
using Newtonsoft.Json;
using Responses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace IdeaDatabase.Credentials
{
    public class RESTAPIAuthCredentials : ValidableObject
    {
       
        [JsonIgnore]
        public string DeviceToken { get; set; }
        public string Platform { get; set; }

        [StringValidation(Required: true)]
        public string AccessCode { get; set; }

        [StringValidation(Required: true)]
        public string RedirectUrl { get; set; }

        //[DbFieldValidation("UserAuthentications")]
        public string CallerId { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        //[DbFieldValidation("UserAuthentications")]
        public string ClientViewer { get; set; }

        [JsonIgnore]
        [StringValidation(Required: false, MinimumLength:1)]
        public string ClientId { get; set; }


        private const string _defaultLanguageCode = "en";
        private const string _defaultCountryCode = "US";

        private string _locale;

        [JsonIgnore]
        [EnumValidation(typeof(RESTAPILocale), false, false, true)]
        public string Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }

        [JsonIgnore]
        public string LanguageCode
        {
            get
            {
                if (string.IsNullOrEmpty(_locale))
                    return _defaultLanguageCode;
                else return _locale.Substring(0, 2);
            }
        }
        [JsonIgnore]
        public string CountryCode
        {
            get
            {
                if (string.IsNullOrEmpty(_locale))
                    return _defaultCountryCode;
                else return _locale.Substring(3, 2);
            }
        }


        protected override void Validate(List<ValidationResult> results, ResponseBase r)
        {
            if (string.IsNullOrEmpty(Platform) || string.IsNullOrEmpty(AccessCode) || string.IsNullOrEmpty(CallerId) || string.IsNullOrEmpty(RedirectUrl))
            {
                r.ErrorList.Add(Faults.InvalidCredentials);
            }
            else base.Validate(results, r);
        }
    }
}