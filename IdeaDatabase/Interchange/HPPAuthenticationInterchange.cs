using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class UserAuthenticationInterchange
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        
        public int UserId { get; set; }
        public string TokenMD5 { get; set; }
        public string CallerId { get; set; }
        public string Token { get; set; }
        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }
        public Nullable<bool> IsHPID { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UseCaseGroup { get; set; }
        public string Platform { get; set; }
        public string ClientApplication { get; set; }
        public string ClientVersion { get; set; }
        public string ClientId { get; set; }
    
        public DateTime LoginDate { get; set; }

        public static explicit operator UserAuthentication(UserAuthenticationInterchange hppAuthInterchange)
        {
            if (hppAuthInterchange == null) return null;

            UserAuthentication hppAuth = new UserAuthentication();

            hppAuth.UserId = hppAuthInterchange.UserId;
            hppAuth.TokenMD5 = QueryUtils.GetMD5(hppAuthInterchange.Token);
         
            hppAuth.CallerId = hppAuthInterchange.CallerId;
            hppAuth.Token = hppAuthInterchange.Token;
            hppAuth.LanguageCode = hppAuthInterchange.LanguageCode;
            hppAuth.CountryCode = hppAuthInterchange.CountryCode;
            hppAuth.IsHPID = hppAuthInterchange.IsHPID;
            hppAuth.UseCaseGroup = hppAuthInterchange.UseCaseGroup;
            hppAuth.ModifiedDate = DateTime.UtcNow;
            hppAuth.ClientApplication = hppAuthInterchange.ClientApplication;
            hppAuth.ClientPlatform = hppAuthInterchange.Platform?.ToLower();
            hppAuth.ClientVersion = hppAuthInterchange.ClientVersion;
            hppAuth.ClientId = hppAuthInterchange.ClientId;
            hppAuthInterchange.LoginDate = DateTime.UtcNow;
            hppAuth.CreatedDate = hppAuthInterchange.LoginDate;
            return hppAuth;
        }

        public static string MapPlatformToClientApplication(string platform)
        {

            if (string.IsNullOrEmpty(platform))
                return null;

            RESTAPIPlatform enumPlatform;
            bool isEnum = Enum.TryParse(platform, out enumPlatform);

            if (!isEnum)
                return null;

            switch (enumPlatform)
            {
              
                case RESTAPIPlatform.web:
                    return "HP.InnovationPortal";                
                default:
                    return null;
            }
        }
    }
}