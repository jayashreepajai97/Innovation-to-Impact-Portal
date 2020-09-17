using Newtonsoft.Json;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{

    public class IdeaCatrgoryResponse
    {
        public string Category { get; set; }
        public int Id { get; set; }

    }

    [JsonObject]
    public class RestAPIGetIdeaCategoryResponse : ResponseBase
    {
        public List<IdeaCatrgoryResponse> categories { get; set; }

        public RestAPIGetIdeaCategoryResponse()
        {
            categories = new List<IdeaCatrgoryResponse>();
        }
    }

}