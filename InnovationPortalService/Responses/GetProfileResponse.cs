using Credentials;
using Newtonsoft.Json;
using Responses;
using System;

namespace InnovationPortalService.Responses
{
    [JsonObject]
    public class GetProfileResponse : ResponseBase
    {

        [JsonProperty]
        public AccessCredentials Credentials;

        [JsonProperty]
        public CustomerProfile CustomerProfileObject;

        [JsonIgnore]
        public DateTime? LoginDate;
    }
}