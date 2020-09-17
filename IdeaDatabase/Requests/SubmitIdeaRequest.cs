using IdeaDatabase.DataContext;
using IdeaDatabase.Validation;
using System.Collections.Generic;

namespace IdeaDatabase.Requests
{
    public class SubmitIdeaRequest : ValidableObject
    {
        [RequiredValidation]
        public int CategoryId { get; set; }
        [StringValidation(Required: true)]
        public string Title { get; set; }
        [RequiredValidation]
        public string Description { get; set; }
        [RequiredValidation]
        public bool IsDraft { get; set; } = false;
        public string IdeaContributors { get; set; }
        [RequiredValidation]
        public string BusinessImpact { get; set; }
        public int ChallengeId { get; set; }
        [RequiredValidation]
        public string Solution { get; set; }
    }
}