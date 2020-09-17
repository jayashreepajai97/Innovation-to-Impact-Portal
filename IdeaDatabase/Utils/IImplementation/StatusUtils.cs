using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class StatusUtils : IStatusUtils
    {
        private readonly IIdeaAssignmentUtils ideaAssignmentUtils = DependencyInjector.Get<IIdeaAssignmentUtils, IdeaAssignmentUtils>();

        public void GetRoles(RESTAPIGetRolesResponse response)
        {
            List<RESTAPIRolesInterchange> roleInterchangeList = null;
            List<Role> rolelist = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             roleInterchangeList = new List<RESTAPIRolesInterchange>();
                             rolelist = new List<Role>();

                             rolelist = query.GetRole(context);

                             if (rolelist != null)
                             {
                                 foreach (var role in rolelist)
                                 {
                                     RESTAPIRolesInterchange rolesInterchange = new RESTAPIRolesInterchange(role);
                                     roleInterchangeList.Add(rolesInterchange);
                                 }
                                 response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
                             }
                             else
                             {
                                 response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                             }

                         }
                         , readOnly: true
                     );

            if (roleInterchangeList != null && roleInterchangeList.Count > 0)
                response.RolesList.AddRange(roleInterchangeList);
        }

        public void InsertStatus(RestAPIAddIdeaStateResponse response, int IdeaID, int ideaState, int UserId)
        {
            DatabaseWrapper.databaseOperation(response,
                          (context, query) =>
                          {
                              IdeaStatusLog ideaStatusLog = null;
                              Idea idea = query.GetIdeaById(context, IdeaID);
                              idea.IdeaStatusLogs.ToList().ForEach(x => x.IsActive = false);

                              ideaStatusLog = new IdeaStatusLog() { IdeaId = IdeaID, IdeaState = ideaState, ModifiedByUserId = UserId, CreatedDate = DateTime.UtcNow, IsActive = true };
                              query.AddIdeaStatusHistory(context, ideaStatusLog);

                              ideaAssignmentUtils.SubmitIdeaAssignments(response, UserId, IdeaID, ideaState);
                              context.SubmitChanges();

                          }
                          , readOnly: false
                      );
        }
    }
}