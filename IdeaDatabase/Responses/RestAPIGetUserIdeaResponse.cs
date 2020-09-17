using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPIGetUserIdeaResponse : ResponseBase
    {
        public List<RESTAPIIdeaInterchange> IdeaList;
        public RestAPIGetUserIdeaResponse()
        {
            IdeaList = new List<RESTAPIIdeaInterchange>();
        }
    }
}