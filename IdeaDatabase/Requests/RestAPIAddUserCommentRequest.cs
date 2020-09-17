using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIAddUserCommentRequest : ValidableObject
    {
        [NumberValidation(1, required: true)]
        public int IdeaID { get; set; }
        [StringValidation(MinimumLength: 1, Required: true)]
        public string CommentDescription { get; set; }
    }
}