using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPISearchUserResponse : ResponseBase
    {
        public List<RESTAPIUserInterchange> UserSearchList;

        public RestAPISearchUserResponse()
        {
            UserSearchList = new List<RESTAPIUserInterchange>();
        }

    }
}