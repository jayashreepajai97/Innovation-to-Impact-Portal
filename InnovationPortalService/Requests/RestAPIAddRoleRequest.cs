using IdeaDatabase.Requests;
using IdeaDatabase.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnovationPortalService.Requests
{
    public class RestAPIAddRoleRequest : ValidableObject
    {
        public int[] RoleId { get; set; }
    }
}