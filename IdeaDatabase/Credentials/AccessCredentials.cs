using System;
using Responses;
using Newtonsoft.Json;
using IdeaDatabase.Utils;
using IdeaDatabase.Responses;
using IdeaDatabase.Validation;
using System.Collections.Generic;
using IdeaDatabase.DataContext;
using System.ComponentModel.DataAnnotations;

namespace Credentials
{
    public class AccessCredentials : ValidableObject
    {
        private string languageCode = "en";
        private string countryCode = "US";

        [JsonIgnore]
        public string LanguageCode
        {
            get
            {
                return languageCode;
            }
        }

        [JsonIgnore]
        public string CountryCode
        {
            get
            {
                return countryCode;
            }
        }

        [NumberValidation(min: 1)]
        public int UserID { get; set; }

        
        public List<int> RoleID { get; set; }
        public List<UserRoles> Roles { get; set; }


        /// <summary>
        /// After successful login this field should be returned by server and used later 
        /// to access other methods
        /// </summary>
        [StringValidation(MinimumLength: 1, Required: true)]
        public string SessionToken { get; set; }

        /// <summary>
        /// Identification of the calling point: serial number of the device from which user logs in or text 'portal' in case of web-login
        /// </summary>
        [DbFieldValidation("UserAuthentications")]
        public string CallerId { get; set; }

        /// <summary>
        /// Real HPID token, hidden and not shown in response
        /// </summary>
        [JsonIgnore]
        public string Token { get; set; }

        public string Platform { get; set; }

        [VersionStringValidation(false)]
        public string AppVersion { get; set; }

        /// <summary>
        /// UseCaseGroup is set by reading it from DB, It's hidden and not shown in response
        /// </summary>
        [JsonIgnore]
        public string UseCaseGroup { get; set; }
        /// <summary>
        /// LoginDate is set by reading it from DB, It's hidden and not shown in response and will be use for calculating session timeout.
        /// </summary>
        [JsonIgnore]
        public DateTime? LoginDate { get; set; }

        protected override void Validate(List<ValidationResult> results, ResponseBase r)
        {
            base.Validate(results, r);
            // Try to get the token only if access credential members are valid
            if (r.ErrorList.Count == 0)
            {
                DatabaseWrapper.databaseOperation(r,
                    (context, query) =>
                    {
                        UserAuthentication auth = query.GetHPPToken(context, UserID, SessionToken, CallerId);
                        if (auth == null)
                        {
                            r.ErrorList.Add(Faults.InvalidCredentials);
                            return;
                        }
                        Token = auth.Token;
                        //UseCaseGroup = auth.UseCaseGroup;
                        LoginDate = auth.CreatedDate;
                        Platform = auth.ClientPlatform;
                        if (auth.LanguageCode != null)
                        {
                            languageCode = auth.LanguageCode;
                        }
                        if (auth.CountryCode != null)
                        {
                            countryCode = auth.CountryCode;
                        }
                    },
                    readOnly: true
                );
            }
        }
    }

    [JsonObject]
    public class UserRoles
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }        
    }
}