using IdeaDatabase.DataContext;
using Newtonsoft.Json;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class ChallengeResponse
    {
        public int ChallengeID { get; set; }
        public string ChallengeName { get; set; }
    }


    [JsonObject]
    public class RestAPIGetIdeaChallengeResponse : ResponseBase
    {
        public List<ChallengeResponse> ChallengeList { get; set; }

        public RestAPIGetIdeaChallengeResponse()
        {
            ChallengeList = new List<ChallengeResponse>();
        }
    }
}