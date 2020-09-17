using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPICatrgoryRequest : ValidableObject
    {
        [StringValidation(Required: true, MinimumLength: 1)]
        public string Category { get; set; }
        [NumberValidation(min: 1, required: true)]
        public int ID { get; set; }
    }
}