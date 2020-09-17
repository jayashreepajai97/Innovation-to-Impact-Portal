using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPISearchIdeaResponse : ResponseBase
    {
        public List<RESTAPIIdeaInterchange> IdeaSearchList;
        public RestAPISearchIdeaResponse()
        {
            IdeaSearchList = new List<RESTAPIIdeaInterchange>();
        }

       
    }
}