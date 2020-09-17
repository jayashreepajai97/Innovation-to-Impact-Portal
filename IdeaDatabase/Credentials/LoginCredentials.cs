using IdeaDatabase.Responses;
using IdeaDatabase.Validation;
using Newtonsoft.Json;
using Responses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Credentials
{
    [JsonObject]
    public class LoginCredentials : ValidableObject
    {
        [StringValidation(MinimumLength: 1, Required: true)]
        public string UserName { get; set; }

        [StringValidation(MinimumLength: 1, Required: true)]
        public string Password { get; set; }

        [DbFieldValidation("UserAuthentications")]        
        public string CallerId { get; set; }        

        protected override void Validate(List<ValidationResult> results, ResponseBase r)
        {
            base.Validate(results, r);
        }
    }
}