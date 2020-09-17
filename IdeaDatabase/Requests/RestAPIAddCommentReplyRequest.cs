using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIAddCommentReplyRequest : ValidableObject
    {
        [NumberValidation(1, required: true)]
        public int IdeaCommentID { get; set; }
        [StringValidation(MinimumLength: 1, Required: true)]
        public string DiscussionDescription { get; set; }
    }
}