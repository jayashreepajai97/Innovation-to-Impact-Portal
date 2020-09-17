using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIUpdateIdeaRequest : ValidableObject
    {
        public string GitRepo { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BusinessImpact { get; set; }
        public string Solution { get; set; }
        public Nullable<int> ChallengeId { get; set; }
        public Nullable<int> CategoryId { get; set; }

    }
}