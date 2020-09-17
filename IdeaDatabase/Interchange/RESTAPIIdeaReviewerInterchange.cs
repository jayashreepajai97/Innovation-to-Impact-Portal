using IdeaDatabase.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeaReviewerInterchange
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }

        public string CreatedDate { get; set; }
        public RESTAPIIdeaReviewerInterchange(Idea idea)
        {
            Username = string.Concat(idea.User.FirstName, " ", idea.User.LastName);
        }

        public RESTAPIIdeaReviewerInterchange()
        {

        }
    }
}