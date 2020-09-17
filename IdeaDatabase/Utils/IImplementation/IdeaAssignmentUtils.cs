using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class IdeaAssignmentUtils : IIdeaAssignmentUtils
    {
        public void SubmitIdeaAssignments(RestAPIAddIdeaStateResponse response, int UserId, int IdeaId, int ideaState)
        {
            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            Role roles = null;
                            RoleMapping roleMapping = null;
                            IdeaAssignment assignment = null;

                            if (ideaState == (int)IdeaStatusTypes.ReviewPending)
                            {
                                roles = query.GetAllRoleMappings(context, RoleTypes.REVIEWER.ToString());
                            }
                            else if (ideaState == (int)IdeaStatusTypes.SponsorPending)
                            {
                                roles = query.GetAllRoleMappings(context, RoleTypes.SPONSOR.ToString());
                            }
                            else if (ideaState == (int)IdeaStatusTypes.Sponsored)
                            {
                                roles = query.GetAllRoleMappings(context, RoleTypes.SALES.ToString());
                            }

                            if (roles == null || roles.RoleMappings.Count == 0)
                            {
                                response.ErrorList.Add(Faults.ReviewerNotExists);
                                return;
                            }

                            roleMapping = query.GetUserRoleMappingByRoleId(context, roles.RoleId);
                            assignment = query.GetAssignmentByIdeaId(context, IdeaId);

                            if (assignment != null)
                            {
                                assignment.ReviewByUserId = roleMapping.UserId;
                            }
                            else
                            {
                                IdeaAssignment ideaassignment = new IdeaAssignment() { IdeaId = IdeaId, ReviewByUserId = roleMapping.UserId, IsActive = true, CreatedDate = DateTime.UtcNow };
                                query.AddIdeaAssignment(context, ideaassignment);
                            }

                            context.SubmitChanges();
                        }
                        , readOnly: false
                    );
        }
    }
}