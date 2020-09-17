using Credentials;
using IdeaDatabase.Enums;
using IdeaDatabase.Validation;
using Newtonsoft.Json;

namespace IdeaDatabase.Credentials
{
    [JsonObject]
    public class LocaleCredentials : LoginCredentials
    {
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
    }
}