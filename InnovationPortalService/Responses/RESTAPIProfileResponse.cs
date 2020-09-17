using IdeaDatabase.Enums;
using InnovationPortalService.HPID;
using Newtonsoft.Json;
using Responses;
using System.Collections.Generic;

namespace InnovationPortalService.Responses
{
    [JsonObject]
    public class RESTAPIProfileResponse : ProfileObject
    {
        public RESTAPIProfileResponse(GetProfileResponse profileResponse) : 
            base(profileResponse.ErrorList.Count == 0 ? profileResponse?.CustomerProfileObject : null)
        {
            ErrorList = profileResponse.ErrorList;
        }
    }
}