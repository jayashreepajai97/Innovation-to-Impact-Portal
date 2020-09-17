using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RESTGetUserIdeaDetailsResponse : ResponseBase
    {
        public RESTAPIIdeaDetailsInterchange IdeaDetails;


        //public RESTGetUserIdeaDetailsResponse(Idea idea)
        //{
        //    IdeaList = new RESTAPIIdeaDetailsInterchange(idea);
        //}
    }
  
    

}