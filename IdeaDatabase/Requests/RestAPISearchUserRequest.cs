using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPISearchUserRequest : ValidableObject
    {
        public string SearchName { get; set; }
    }
}