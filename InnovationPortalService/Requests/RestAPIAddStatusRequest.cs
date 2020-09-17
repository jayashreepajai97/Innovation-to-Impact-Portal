using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnovationPortalService.Requests
{
    public class RestAPIAddStatusRequest : ValidableObject
    {
        public string Status { get; set; }
    }
}