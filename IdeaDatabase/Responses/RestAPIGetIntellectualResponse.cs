using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPIGetIntellectualResponse : ResponseBase
    {
        public List<RESTAPIIntellectualInterchange> IdeaIntellectList;

        public RestAPIGetIntellectualResponse()
        {
            IdeaIntellectList = new List<RESTAPIIntellectualInterchange>();
        }
    }
}