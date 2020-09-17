using IdeaDatabase.Interchange;
using Responses;
using System.Collections.Generic;

namespace IdeaDatabase.Responses
{
    public class RestAPIGetStatusResponse : ResponseBase
    {
        public List<RESTAPIStatusInterchange> IdeaStateList;
        public RestAPIGetStatusResponse()
        {
            IdeaStateList = new List<RESTAPIStatusInterchange>();
        }
    }
}