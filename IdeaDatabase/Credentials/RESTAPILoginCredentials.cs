using IdeaDatabase.Responses;
using IdeaDatabase.Validation;
using Newtonsoft.Json;
using Responses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdeaDatabase.Credentials
{
    public class RESTAPILoginCredentials : LocaleCredentials
    {
        // [DbFieldValidation("MobileDevices")]
        [JsonIgnore]
        public string DeviceToken { get; set; }

        public string Platform { get; set; }

        [JsonIgnore]
        public string ClientViewer { get; set; }

        protected override void Validate(List<ValidationResult> results, ResponseBase r)
        {
            if (string.IsNullOrEmpty(Platform))
            {
                r.ErrorList.Add(Faults.InvalidCredentials);
            }
            else
            {
                base.Validate(results, r);
            }
        }
    }
}