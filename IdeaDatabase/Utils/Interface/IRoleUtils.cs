using IdeaDatabase.DataContext;
using IdeaDatabase.Responses;
using Responses;
using System.Collections.Generic;
 

namespace IdeaDatabase.Utils.Interface
{
    public interface IRoleUtils
    {
        List<RoleMapping> InsertRoleMapping(ResponseBase response, int userId, int[] RoleID = null);
        RestAPIAddRoleResponse InsertRole(ResponseBase response, string RoleName);
    }    
}