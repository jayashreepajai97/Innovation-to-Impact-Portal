using Newtonsoft.Json;
using System.Collections.Generic;

namespace Responses
{
    [JsonObject]
    public class ResponseBase
    {
        [JsonProperty]
        public HashSet<Fault> ErrorList = new HashSet<Fault>();

        public override string ToString()
        {
            string ret="";
            foreach(Fault f in ErrorList)
            {
                ret += $"[code:({f.ReturnCode}) origin:({f.Origin}) text:({f.DebugStatusText}) debug:({f.DebugMessage}) stack:({f.DebugStackTrace})] ";
            }
            return ret;
        }

        [JsonProperty]
        public string Status { get; set; } = "Failure";
    }

}
