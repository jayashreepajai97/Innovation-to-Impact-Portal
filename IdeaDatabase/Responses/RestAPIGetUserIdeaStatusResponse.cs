using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPIGetUserIdeaStatusResponse : ResponseBase
    {
        public List<RESTAPIIdeaStatusInterchange> IdeaList;
        public RestAPIGetUserIdeaStatusResponse()
        {
            IdeaList = new List<RESTAPIIdeaStatusInterchange>();
        }
    }
}