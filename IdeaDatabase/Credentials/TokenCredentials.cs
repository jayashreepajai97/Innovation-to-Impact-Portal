using IdeaDatabase.Enums;
using IdeaDatabase.Validation;
using Responses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Credentials
{
    public class TokenCredentials : ValidableObject
    {
        [StringValidation(MinimumLength: 1, Required: true)]
        public string SessionToken { get; set; }

        //[DbFieldValidation("UserAuthentications")]
        public string CallerId { get; set; }

        [EnumValidation(typeof(RESTAPILocale), false, false, true)]
        public string Locale { get; set; }
        public string Platform { get; set; }

        protected override void Validate(List<ValidationResult> results, ResponseBase r)
        {
            base.Validate(results, r);
        }
    }
}