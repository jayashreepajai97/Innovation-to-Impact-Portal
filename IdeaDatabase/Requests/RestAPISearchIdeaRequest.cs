using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPISearchIdeaRequest : ValidableObject
    {
        public string SearchText { get; set; }
     }
}