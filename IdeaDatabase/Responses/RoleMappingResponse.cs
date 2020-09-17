using IdeaDatabase.DataContext;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RoleMappingResponse : ResponseBase
    {
        public List<RoleMapping> userRoles { get; set; }

    }
}