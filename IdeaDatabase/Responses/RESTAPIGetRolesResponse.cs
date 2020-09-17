using IdeaDatabase.Interchange;
using Responses;
using System.Collections.Generic;

namespace IdeaDatabase.Responses
{
    public class RESTAPIGetRolesResponse : ResponseBase
    {
        public List<RESTAPIRolesInterchange> RolesList;
        public RESTAPIGetRolesResponse()
        {
            RolesList = new List<RESTAPIRolesInterchange>();
        }
    }
}