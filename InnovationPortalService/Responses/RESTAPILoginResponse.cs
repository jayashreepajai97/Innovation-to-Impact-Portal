using Credentials;
using Newtonsoft.Json;
using Responses;
using System.Collections.Generic;

namespace InnovationPortalService.Responses
{
    [JsonObject]
    public class RESTAPILoginResponse : ResponseBase
    {
        [JsonProperty]
        public int UserID { get; set; }

        [JsonProperty]
        public string SessionToken { get; set; }

        [JsonProperty]
        public string CallerId { get; set; }        

        [JsonProperty]
        public bool ActiveHealth { get; set; }

        [JsonProperty]
        public string Locale { get; set; }

        [JsonProperty]
        public bool IsNewCustomer { get; set; }

        [JsonProperty]
        public string TimeOut { get; set; }

        [JsonProperty]
        public string FirstName { get; set; }
        [JsonProperty]
        public string LastName { get; set; }

        [JsonProperty]
        public string Emailaddress { get; set; }

        [JsonProperty]
        public List<UserRoles> Roles { get; set; }
    }
    
}