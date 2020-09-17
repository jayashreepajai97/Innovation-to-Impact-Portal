using Newtonsoft.Json;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnovationPortalService.Responses
{
    public class RESTAPIGetLocaleResponse : ResponseBase
    {
        [JsonProperty]
        public string Methone { get; set; }
        [JsonProperty]
        public string HPID { get; set; }
        [JsonProperty]
        public string Redirector { get; set; }
    }
}