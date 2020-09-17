using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIUpdateIdeaDraftRequest : ValidableObject
    {
        public Nullable<int> CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDraft { get; set; }
        public string IdeaContributors { get; set; }
        public string BusinessImpact { get; set; }
        public Nullable<int> ChallengeId { get; set; }
        public string Solution { get; set; }
        [RequiredValidation]
        public int IdeaId { get; set; }
    }
}