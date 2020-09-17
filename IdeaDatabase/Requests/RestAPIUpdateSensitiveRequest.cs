using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIUpdateSensitiveRequest : ValidableObject
    {
        [NumberValidation(1, required: true)]
        public int IdeaId { get; set; }
    }
}