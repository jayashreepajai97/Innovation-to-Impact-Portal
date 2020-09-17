using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPIGetUserCommentsResponse : ResponseBase
    {
        public List<RESTAPIIdeaCommentInterchange> UserCommentList;
        public RestAPIGetUserCommentsResponse()
        {
            UserCommentList = new List<RESTAPIIdeaCommentInterchange>();

        }
    }
}