using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPIGetArchiveIdeaResponse : ResponseBase
    {
        public List<RESTAPIIdeaInterchange> ArchiveIdeaList;
        public RestAPIGetArchiveIdeaResponse()
        {
            ArchiveIdeaList = new List<RESTAPIIdeaInterchange>();
        }
    }
}