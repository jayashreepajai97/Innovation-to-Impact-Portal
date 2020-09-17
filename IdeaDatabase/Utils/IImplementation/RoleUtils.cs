using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdeaDatabase.Utils.IImplementation
{
    public class RoleUtils : IRoleUtils
    {
        public List<RoleMapping> InsertRoleMapping(ResponseBase response, int userId, int[] RoleID = null)
        {
            List<RoleMapping> userRoles = new List<RoleMapping>();
            List<RoleMapping> listroleMappings = null;


            if (RoleID == null || RoleID.Length == 0)
            {
                RoleID = new[] { (int)RoleTypes.SUBMITTER };
            }

            DatabaseWrapper.databaseOperation(response,
                            (context, query) =>
                            {
                                List<Role> roles = query.GetRoles(context, RoleID);

                                if (roles.Count == 0)
                                {
                                    response.ErrorList.Add(Faults.RoleIDNotExists);
                                    return;
                                }

                                List<RoleMapping> roleMappings = query.GetUserRoleMappings(context, userId);

                                listroleMappings = new List<RoleMapping>();


                                if (roleMappings.Count == 0)
                                {
                                    foreach (int role in RoleID)
                                    {
                                        listroleMappings.Add(new RoleMapping { RoleId = role, UserId = userId, CreatedDate = DateTime.UtcNow });
                                    }
                                    query.AddUserRoleMappings(context, listroleMappings);                                   
                                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
                                }
                                else
                                {
                                    var results = roleMappings.Where(role => RoleID.Contains(role.RoleId)).ToList();
                                    if (results.Count > 0)
                                    {
                                        string existRoles = string.Join(",", results.Select(s => s.RoleId).ToList());
                                        response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                                        response.ErrorList.Add(new Fault("RoleMappings", "RoleMappingsError", $"Roles {existRoles} exists for the userID"));
                                        return;
                                    }
                                    foreach (int role in RoleID)
                                    {
                                        listroleMappings.Add(new RoleMapping { RoleId = role, UserId = userId, CreatedDate = DateTime.UtcNow });
                                    }
                                    query.AddUserRoleMappings(context, listroleMappings);
                                }


                                context.SubmitChanges();
                            }
                            , readOnly: false
                        );



            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }

            return listroleMappings;
        }

        public RestAPIAddRoleResponse InsertRole(ResponseBase response, string RoleName)
        {
            Role role;
            RestAPIAddRoleResponse restAPIAddRoleResponse = new RestAPIAddRoleResponse();
            DatabaseWrapper.databaseOperation(response,
            (context, query) =>
            {
                bool isValid = query.GetRoleByName(context, RoleName);

                if (!isValid)
                {
                    role = new Role() { RoleName = RoleName, CreatedDate = DateTime.UtcNow, IsActive = true };
                    query.AddRole(context, role);
                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
                }
                else
                {
                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                    response.ErrorList.Add(Faults.RoleNameExists);
                    return;
                }

                context.SubmitChanges();
            }
            , readOnly: false
            );

            return restAPIAddRoleResponse;
        }
    }
}