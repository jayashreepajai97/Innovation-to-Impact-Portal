using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIAddRolesRequest : ValidableObject
    {
        public string RoleName { get; set; }
    }
}