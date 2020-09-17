using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class SubmitIdeaUtil : ISubmitIdeaUtil
    {
        string Failure = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
        string Success = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
        private readonly ITagsUtils tagUtil = DependencyInjector.Get<ITagsUtils, TagsUtils>();
        private readonly IStatusUtils statusUtils = DependencyInjector.Get<IStatusUtils, StatusUtils>();
        private readonly IIdeaUtils ideaUtils = DependencyInjector.Get<IIdeaUtils, IdeaUtils>();

        public void SubmitIdeaRequest(RestAPISubmitIdeaResponse response, SubmitIdeaRequest submitIdeaRequest, int UserId)
        {
            DatabaseWrapper.databaseOperation(response,
                       (context, query) =>
                       {
                           Idea idea = new Idea();
                           IdeaCategory category = query.GetIdeaFromCategoryID(context, submitIdeaRequest.CategoryId);
                           IdeaChallenge ideaChallenge = query.GetChallengeByID(context, submitIdeaRequest.ChallengeId);

                           // insert category
                           if (category != null)
                               idea.IdeaCategory = category;

                           //insert challenge
                           if (ideaChallenge != null)
                               idea.ChallengeId = ideaChallenge.IdeaChallengeId;

                           //insert Ideacontributors
                           if (!string.IsNullOrEmpty(submitIdeaRequest.IdeaContributors))
                           {
                               List<string> contributors = GetContributorList(submitIdeaRequest.IdeaContributors);
                               List<User> userList = query.GetUserByNames(context, contributors);

                               foreach (var user in userList)
                               {
                                   idea.IdeaContributors.Add(new IdeaContributor() { IdeaId = idea.IdeaId, UserId = user.UserId, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow });
                               }
                           }

                           //insert idea status
                           IdeaStatusLog ideaStatusLog = new IdeaStatusLog()
                           {
                               CreatedDate = DateTime.UtcNow,
                               ModifiedByUserId = UserId,
                               IsActive = true,
                               IdeaState = submitIdeaRequest.IsDraft ? (int)IdeaStatusTypes.SubmitPending : (int)IdeaStatusTypes.ReviewPending
                           };

                           idea.IdeaStatusLogs.Add(ideaStatusLog);

                           idea.Title = submitIdeaRequest.Title;
                           idea.Description = submitIdeaRequest.Description;
                           idea.IsActive = true;
                           idea.CreatedDate = DateTime.UtcNow;
                           idea.ModifiedDate = DateTime.UtcNow;
                           idea.BusinessImpact = submitIdeaRequest.BusinessImpact;
                           idea.UserId = UserId;
                           idea.IsSensitive = true;
                           idea.Solution = submitIdeaRequest.Solution;
                           idea.IsDraft = submitIdeaRequest.IsDraft;

                           query.AddIdea(context, idea);
                           context.SubmitChanges();
                           response.IdeaId = idea.IdeaId;
                           response.Status = Success;
                       }
                       , readOnly: false
                   );

            if (response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
        }


        public RestAPIAddIdeaStateResponse InsertIdeaStatus(RestAPIAddIdeaStateResponse response, int IdeaID, int UserId)
        {
            IdeaStatusLog ideaStatusLog = null;
            List<RoleMapping> roleMappings = new List<RoleMapping>();
            int ideaState = 0;
            bool result = false;

            RestAPIAddIdeaStateResponse restapiaddidearesponse = new RestAPIAddIdeaStateResponse();

            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            ideaStatusLog = query.GetIdeaStatusByIdeaId(context, IdeaID);
                            roleMappings = query.GetUserRoleMappings(context, UserId);

                            if (roleMappings.Count > 0)
                            {
                                foreach (var role in roleMappings)
                                {
                                    if (ideaStatusLog.IdeaState == (int)IdeaStatusTypes.ReviewPending)
                                    {
                                        if (role.RoleId == 3)
                                        {
                                            result = true;
                                        }
                                    }
                                    if (ideaStatusLog.IdeaState == (int)IdeaStatusTypes.SponsorPending)
                                    {
                                        if (role.RoleId == 4)
                                        {
                                            result = true;
                                        }
                                    }
                                }
                            }
                            if (result == false)
                            {
                                response.ErrorList.Add(Faults.InsufficientPermissions);
                                return;
                            }

                            if (ideaStatusLog.IdeaState == (int)IdeaStatusTypes.ReviewPending)
                            {
                                ideaState = (int)IdeaStatusTypes.SponsorPending;
                                restapiaddidearesponse.IdeaState = EnumDescriptor.GetEnumDescription(IdeaStatusTypes.SponsorPending);
                            }
                            else if (ideaStatusLog.IdeaState == (int)IdeaStatusTypes.SponsorPending)
                            {
                                ideaState = (int)IdeaStatusTypes.Sponsored;
                                restapiaddidearesponse.IdeaState = EnumDescriptor.GetEnumDescription(IdeaStatusTypes.Sponsored);
                            }
                            else
                            {
                                response.ErrorList.Add(Faults.InvalidIdeaStatus);
                                response.Status = Failure;
                                return;
                            }

                            statusUtils.InsertStatus(response, IdeaID, ideaState, UserId);
                            context.SubmitChanges();

                            response.Status = Success;
                        }
                        , readOnly: false
                    );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
            return response;
        }

        public IdeaComment InsertIdeaComment(ResponseBase response, int IdeaID, string CommentDescription, int UserId)
        {
            IdeaComment ideaComment = null;


            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            Idea idea = query.GetIdeaById(context, IdeaID);

                            if (idea != null)
                            {

                                ideaComment = new IdeaComment() { IdeaId = IdeaID, CommentDescription = CommentDescription, CommentByUserId = UserId, CreatedDate = DateTime.UtcNow };
                                query.AddIdeaComment(context, ideaComment);
                                response.Status = Success;
                            }
                            else
                            {
                                response.ErrorList.Add(Faults.IdeaNotFound);
                                response.Status = Failure;
                                return;
                            }
                            context.SubmitChanges();
                        }
                        , readOnly: false
                    );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }


            return ideaComment;
        }

        public void GetUserComments(RestAPIGetUserCommentsResponse response, int IdeaId)
        {
            List<RESTAPIIdeaCommentInterchange> userCommentInterchangeList = null;
            List<IdeaComment> commentlist = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             userCommentInterchangeList = new List<RESTAPIIdeaCommentInterchange>();
                             commentlist = new List<IdeaComment>();

                             commentlist = query.GetIdeaComment(context, IdeaId);
                             if (commentlist.Count > 0)
                             {
                                 foreach (var comment in commentlist)
                                 {
                                     RESTAPIIdeaCommentInterchange commentInterchange = new RESTAPIIdeaCommentInterchange(comment);
                                     userCommentInterchangeList.Add(commentInterchange);
                                 }
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );

            if (userCommentInterchangeList != null && userCommentInterchangeList.Count > 0)
                response.UserCommentList.AddRange(userCommentInterchangeList);
        }

        public RestAPIDeleteIdeaResponse DeleteIdeaComment(ResponseBase response, int IdeaCommentId)
        {
            RestAPIDeleteIdeaResponse restAPIDeleteIdeaResponse = new RestAPIDeleteIdeaResponse();

            DatabaseWrapper.databaseOperation(response,
           (context, query) =>
           {

               IdeaComment ideaComment = query.GetIdeaCommentById(context, IdeaCommentId);

               if (ideaComment != null)
               {
                   query.DeleteIdeaComment(context, ideaComment);
                   response.Status = Success;
               }
               else
               {
                   response.ErrorList.Add(Faults.IdeaCommentNotFound);
                   response.Status = Failure;
                   return;
               }

               context.SubmitChanges();
           }
            , readOnly: false
            );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
                restAPIDeleteIdeaResponse.ErrorList = response.ErrorList;
            }

            return restAPIDeleteIdeaResponse;
        }

        public Idea UpdateSensitive(ResponseBase response, int UserId, int IdeaID, bool isSensitive)
        {
            Idea idea = null;

            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            idea = query.GetIdeaById(context, IdeaID);

                            if (idea != null)
                            {
                                idea.IsSensitive = isSensitive;
                                idea.ModifiedDate = DateTime.UtcNow;
                                response.Status = Success;
                            }
                            else
                            {
                                response.ErrorList.Add(Faults.IdeaNotFound);
                                response.Status = Failure;
                                return;
                            }
                            context.SubmitChanges();
                        }
                        , readOnly: false
                    );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
            return idea;
        }

        private List<string> GetTagList(string tags)
        {
            List<string> tagslist = new List<string>();
            if (tags.Contains(","))
                tagslist = tags.TrimEnd(',').Split(',').ToList();
            else
                tagslist.Add(tags);

            return tagslist;
        }

        private List<string> GetContributorList(string contributors)
        {
            List<string> contributorlist = new List<string>();
            if (contributors.Contains(","))
                contributorlist = contributors.TrimEnd(',').Split(',').ToList();
            else
                contributorlist.Add(contributors);

            return contributorlist;
        }

        public void GetIdeas(RestAPIGetUserIdeaResponse response, int UserId, bool IsDraft = false)
        {
            List<RESTAPIIdeaInterchange> ideaInterchangeList = null;
            List<Idea> ideaList = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             ideaInterchangeList = new List<RESTAPIIdeaInterchange>();
                             ideaList = new List<Idea>();

                             if (IsDraft)
                             {
                                 ideaList = query.GetDraftIdeas(context, UserId);
                             }
                             else
                             {
                                 ideaList = query.GetIdeas(context, UserId);
                             }


                             if (ideaList.Count > 0)
                             {
                                 foreach (var idea in ideaList)
                                 {
                                     RESTAPIIdeaInterchange ideaInterchange = new RESTAPIIdeaInterchange(idea);
                                     ideaInterchangeList.Add(ideaInterchange);
                                 }
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );

            if (ideaInterchangeList != null && ideaInterchangeList.Count > 0)
            {
                response.IdeaList.AddRange(ideaInterchangeList);
            }
        }

        public void GetPublicIdeas(RestAPIGetUserIdeaResponse response, int UserId, int CategoryId = 0, bool Sort = false)
        {
            List<RESTAPIIdeaInterchange> ideaInterchangeList = null;
            List<Idea> ideaList = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             ideaInterchangeList = new List<RESTAPIIdeaInterchange>();
                             ideaList = new List<Idea>();

                             if (!Sort)
                             {
                                 ideaList = query.GetPublicIdeas(context, UserId, CategoryId);

                             }
                             else
                             {
                                 ideaList = query.GetSortedPublicIdeas(context, UserId, CategoryId);
                             }

                             if (ideaList.Count > 0)
                             {
                                 foreach (var idea in ideaList)
                                 {
                                     RESTAPIIdeaInterchange ideaInterchange = new RESTAPIIdeaInterchange(idea);
                                     ideaInterchangeList.Add(ideaInterchange);
                                 }
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );

            if (ideaInterchangeList != null && ideaInterchangeList.Count > 0)
                response.IdeaList.AddRange(ideaInterchangeList);
        }

        public void SubmitIdeaAssignment(ResponseBase response, int IdeaId)
        {
            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            List<IdeaAssignment> ideaAssignmentsList = new List<IdeaAssignment>();

                            Role roles = query.GetAllRoleMappings(context, RoleTypes.REVIEWER.ToString());

                            if (roles == null || roles.RoleMappings.Count == 0)
                            {
                                response.ErrorList.Add(Faults.ReviewerNotExists);
                                return;
                            }

                            foreach (var users in roles.RoleMappings)
                            {
                                IdeaAssignment ideaAssignment1 = new IdeaAssignment() { IdeaId = IdeaId, ReviewByUserId = users.UserId, IsActive = true, CreatedDate = DateTime.UtcNow };
                                ideaAssignmentsList.Add(ideaAssignment1);
                            }

                            query.AddUserAssignmentToIdea(context, ideaAssignmentsList);
                            context.SubmitChanges();
                        }
                        , readOnly: false
                    );

        }

        public void GetIdeaReviewsList(RestAPIGetUserIdeaStatusResponse response, int UserId)
        {
            List<RESTAPIIdeaStatusInterchange> ideaInterchangeList = null;
            List<Idea> ideaList = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             ideaInterchangeList = new List<RESTAPIIdeaStatusInterchange>();
                             ideaList = new List<Idea>();

                             ideaList = query.GetIdeasForReview(context, UserId);

                             if (ideaList.Count > 0)
                             {
                                 foreach (var idea in ideaList)
                                 {
                                     RESTAPIIdeaStatusInterchange ideaInterchange = new RESTAPIIdeaStatusInterchange(idea);
                                     ideaInterchangeList.Add(ideaInterchange);
                                 }
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );


            if (ideaInterchangeList != null && ideaInterchangeList.Count > 0)
                response.IdeaList.AddRange(ideaInterchangeList);
        }

        public void GetIdeaSponsorsList(RestAPIGetUserIdeaStatusResponse response, int UserId)
        {
            List<RESTAPIIdeaStatusInterchange> ideaInterchangeList = null;
            List<Idea> ideaList = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             ideaInterchangeList = new List<RESTAPIIdeaStatusInterchange>();
                             ideaList = new List<Idea>();

                             ideaList = query.GetIdeasForSponsors(context, UserId);

                             if (ideaList.Count > 0)
                             {
                                 foreach (var idea in ideaList)
                                 {
                                     RESTAPIIdeaStatusInterchange ideaInterchange = new RESTAPIIdeaStatusInterchange(idea);
                                     ideaInterchangeList.Add(ideaInterchange);
                                 }
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );

            if (ideaInterchangeList != null && ideaInterchangeList.Count > 0)
                response.IdeaList.AddRange(ideaInterchangeList);
        }

        public IdeaCommentDiscussion InsertCommentReply(ResponseBase response, int IdeaCommentId, int UserId, string DiscussionDescription)
        {
            IdeaCommentDiscussion ideaCommentDiscussion = null;
            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            IdeaComment ideaComment = query.GetIdeaCommentById(context, IdeaCommentId);

                            if (ideaComment != null)
                            {
                                ideaCommentDiscussion = new IdeaCommentDiscussion() { IdeaCommentId = ideaComment.IdeaCommentId, UserId = UserId, DiscussionDescription = DiscussionDescription, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow };
                                query.AddIdeaCommentDiscussion(context, ideaCommentDiscussion);
                                response.Status = Success;
                            }
                            else
                            {
                                response.ErrorList.Add(Faults.IdeaCommentNotFound);
                                response.Status = Failure;
                                return;
                            }
                            context.SubmitChanges();
                        }
                        , readOnly: false
                    );

            return ideaCommentDiscussion;
        }

        public void SearchIdea(RestAPISearchIdeaResponse response, string title, int UserID)
        {
            List<RESTAPIIdeaInterchange> ideaInterchangeList = null;
            List<Idea> ideaList = null;


            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            ideaInterchangeList = new List<RESTAPIIdeaInterchange>();
                            ideaList = new List<Idea>();

                            if (!String.IsNullOrEmpty(title))
                            {
                                ideaList = query.SearchIdea(context, UserID, title);
                                if (ideaList.Count > 0)
                                {
                                    foreach (var idea in ideaList)
                                    {
                                        if (idea.IsSensitive == false && idea.UserId != UserID)
                                        {
                                            RESTAPIIdeaInterchange ideaInterchange = new RESTAPIIdeaInterchange(idea);
                                            ideaInterchangeList.Add(ideaInterchange);

                                        }
                                        else if (idea.UserId == UserID)
                                        {
                                            RESTAPIIdeaInterchange ideaInterchange = new RESTAPIIdeaInterchange(idea);
                                            ideaInterchangeList.Add(ideaInterchange);

                                        }
                                    }
                                }
                                response.Status = Success;
                            }
                            else
                            {
                                response.ErrorList.Add(Faults.InvalidSearch);
                                return;
                            }
                        }
                        , readOnly: true
                    );

            if (ideaInterchangeList != null && ideaInterchangeList.Count > 0)
                response.IdeaSearchList.AddRange(ideaInterchangeList);

        }

        public void GetIdeasDetails(RESTGetUserIdeaDetailsResponse response, int ideaId)
        {
            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             Idea idea = query.GetIdeaById(context, ideaId);
                             if (idea != null)
                             {
                                 response.IdeaDetails = ideaUtils.GetIdeasDetails(context, idea.IdeaId);
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );
        }

        
        public List<IdeaEmailToDetails> GetAllStakeholdersEmailAdd(int IdeaId)
        {
            ResponseBase response = null;
            List<IdeaEmailToDetails> res = new List<IdeaEmailToDetails>();

            DatabaseWrapper.databaseOperation(response,
             (context, query) =>
             {

                 res = query.GetAllStakeholdersEmailAdd(context, IdeaId);
                 context.SubmitChanges();

             }
             , readOnly: true
                );
            return res;
        }

        public RESTAPIIdeaBasicDetailsInterchange GetIdeaBasicDetails(int IdeaId)
        {
            ResponseBase response = null;
            List<string> res = new List<string>();
            RESTAPIIdeaBasicDetailsInterchange ideaBasicDetailsInterchange = new RESTAPIIdeaBasicDetailsInterchange();

            DatabaseWrapper.databaseOperation(response,
             (context, query) =>
             {

                 ideaBasicDetailsInterchange = query.GetIdeaBasicDetails(context, IdeaId);
                 context.SubmitChanges();

             }
             , readOnly: true
                );

            return ideaBasicDetailsInterchange;
        }

        public void UpdateIdea(RestAPIUpdateIdeaResponse response, int UserID, int IdeaId, RestAPIUpdateIdeaRequest request)
        {
            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            Idea idea = query.GetIdeaById(context, IdeaId);
                            if (idea != null)
                            {
                                if (!string.IsNullOrEmpty(request.GitRepo))
                                {
                                    idea.GitRepo = request.GitRepo;
                                }
                                else
                                {
                                    idea.GitRepo = null;
                                }
                                if (!string.IsNullOrEmpty(request.Title))
                                {
                                    idea.Title = request.Title;
                                }
                                if (!string.IsNullOrEmpty(request.Description))
                                {
                                    idea.Description = request.Description;
                                }
                                if (!string.IsNullOrEmpty(request.BusinessImpact))
                                {
                                    idea.BusinessImpact = request.BusinessImpact;
                                }
                                if (!string.IsNullOrEmpty(request.Solution))
                                {
                                    idea.Solution = request.Solution;
                                }
                                if (request.ChallengeId.GetValueOrDefault(0) != 0)
                                {
                                    idea.ChallengeId = request.ChallengeId;
                                }
                                if (request.CategoryId.GetValueOrDefault(0) != 0)
                                {
                                    idea.CategoryId = request.CategoryId;
                                }
                                if (!string.IsNullOrWhiteSpace(request.Tags))
                                {
                                    tagUtil.InsertTags(response, idea, request.Tags, UserID);
                                }
                                else
                                {
                                    query.DeleteIdeaTags(context, IdeaId);
                                }
                            }
                            else
                            {
                                response.ErrorList.Add(Faults.IdeaNotFound);
                                response.Status = Failure;
                                return;
                            }

                            response.Status = Success;
                            idea.ModifiedDate = DateTime.UtcNow;

                            context.SubmitChanges();
                        }
                        , readOnly: false
                    );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
        }

        public void SubmitIdeaIntellectual(RestAPIAddIntellectualResponse response, SubmitIntellectualRequest request)
        {
            DatabaseWrapper.databaseOperation(response,
                     (context, query) =>
                     {
                         Idea idea = query.GetIdea(context, request.IdeaId, request.UserID, Enums.Relation.NoneRel);
                         if (idea == null)
                         {
                             response.ErrorList.Add(Faults.IdeaNotFound);
                             return;
                         }

                         IdeaIntellectualProperty ideaIntellectualProperty = new IdeaIntellectualProperty()
                         {
                             IdeaId = request.IdeaId,
                             RecordId = request.RecordId,
                             Status = request.Status,
                             FiledDate = request.FiledDate,
                             ApplicationNumber = request.ApplicationNumber,
                             PatentId = request.PatentId,
                             CreatedDate = DateTime.UtcNow,
                             ModifiedDate = DateTime.UtcNow,
                             IsAttachment = request.IsAttachment,
                             AttachmentCount = request.AttachmentCount,
                             UserId = request.UserID,
                             InventionReference = request.InventionReference
                         };

                         // insert idea intellectual attachment
                         if (request.AttachmentCount != 0)
                         {
                             request.ideaAttachments.ToList().ForEach((attachment) =>
                             {
                                 IdeaAttachment ideaAttach = new IdeaAttachment();
                                 ideaAttach.IdeaId = request.IdeaId;
                                 ideaAttach.AttachedFileName = attachment.AttachedFileName;
                                 ideaAttach.FileExtention = attachment.FileExtention;
                                 ideaAttach.FileSizeInByte = attachment.FileSizeInByte;
                                 ideaAttach.CreatedDate = DateTime.UtcNow;
                                 ideaAttach.FolderName = attachment.DocumentTypeFolderName;
                                 ideaIntellectualProperty.IdeaAttachments.Add(ideaAttach);

                             });
                         }

                         query.AddIdeaIntellectual(context, ideaIntellectualProperty);
                         context.SubmitChanges();
                         response.IntellectualId = ideaIntellectualProperty.IntellectualId;
                     }
                     , readOnly: false
                 );

            if (response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
                return;
            }
        }

        public void DeleteIntellectProperty(ResponseBase response, int IntellectualId)
        {
            DatabaseWrapper.databaseOperation(response,
           (context, query) =>
           {
               IdeaIntellectualProperty ideaIntellectualProperty = query.GetIdeaIntellectualById(context, IntellectualId);

               if (ideaIntellectualProperty != null)
               {
                   query.DeleteIdeaIntellectual(context, ideaIntellectualProperty);
                   response.Status = Success;
               }
               else
               {
                   response.ErrorList.Add(Faults.IdeaIntellectNotFound);
                   response.Status = Failure;
                   return;
               }
               context.SubmitChanges();
           }
            , readOnly: false
            );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
        }

        public void UpdateIntellectProperty(RestAPIUpdateIntellectResponse response, UpdateIntellectualRequest request)
        {
            DatabaseWrapper.databaseOperation(response,
                       (context, query) =>
                       {
                           IdeaIntellectualProperty intellectualProperty = query.GetIdeaIntellectualById(context, request.IntellectualId);

                           if (intellectualProperty != null)
                           {
                               if (!string.IsNullOrEmpty(request.RecordId))
                               {
                                   intellectualProperty.RecordId = request.RecordId;
                               }
                               if (request.Status.GetValueOrDefault(0) != 0)
                               {
                                   intellectualProperty.Status = request.Status;
                               }
                               if (!string.IsNullOrEmpty(request.ApplicationNumber))
                               {
                                   intellectualProperty.ApplicationNumber = request.ApplicationNumber;
                               }
                               if (!string.IsNullOrEmpty(request.PatentId))
                               {
                                   intellectualProperty.PatentId = request.PatentId;
                               }
                               if (!string.IsNullOrEmpty(request.InventionReference))
                               {
                                   intellectualProperty.InventionReference = request.InventionReference;
                               }
                               if (request.FiledDate.HasValue)
                               {
                                   intellectualProperty.FiledDate = request.FiledDate;
                               }

                               intellectualProperty.ModifiedDate = DateTime.UtcNow;
                           }
                           else
                           {
                               response.ErrorList.Add(Faults.IdeaIntellectNotFound);
                               response.Status = Failure;
                               return;
                           }

                           // insert idea intellectual attachment
                           if (request.AttachmentCount != 0)
                           {
                               request.ideaAttachments.ToList().ForEach((attachment) =>
                               {
                                   IdeaAttachment ideaAttach = new IdeaAttachment();
                                   ideaAttach.IdeaId = (int)intellectualProperty.IdeaId;
                                   ideaAttach.AttachedFileName = attachment.AttachedFileName;
                                   ideaAttach.FileExtention = attachment.FileExtention;
                                   ideaAttach.FileSizeInByte = attachment.FileSizeInByte;
                                   ideaAttach.CreatedDate = DateTime.UtcNow;
                                   ideaAttach.FolderName = attachment.DocumentTypeFolderName;
                                   intellectualProperty.IdeaAttachments.Add(ideaAttach);

                               });
                           }

                           intellectualProperty.AttachmentCount = intellectualProperty.IdeaAttachments.Count;
                           intellectualProperty.IsAttachment = intellectualProperty.IdeaAttachments.Count == 0 ? false : true;
                           context.SubmitChanges();
                           response.Status = Success;
                       }
                       , readOnly: false
                   );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
        }

        public void GetIntellectualProperties(RestAPIGetIntellectualResponse response, int IntellectualId)
        {
            List<RESTAPIIntellectualInterchange> intellectualInterchangeList = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             intellectualInterchangeList = new List<RESTAPIIntellectualInterchange>();
                             IdeaIntellectualProperty ideaIntellectualProperty = query.GetIdeaIntellectualById(context, IntellectualId);

                             if (ideaIntellectualProperty != null)
                             {
                                 RESTAPIIntellectualInterchange intellectualInterchange = new RESTAPIIntellectualInterchange(ideaIntellectualProperty);
                                 intellectualInterchangeList.Add(intellectualInterchange);
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );

            if (intellectualInterchangeList != null && intellectualInterchangeList.Count > 0)
                response.IdeaIntellectList.AddRange(intellectualInterchangeList);
        }

        public void UpdateIdeaDraft(RestAPIUpdateDraftResponse response, RestAPIUpdateIdeaDraftRequest request, int UserID)
        {
            DatabaseWrapper.databaseOperation(response,
           (context, query) =>
           {
               Idea idea = query.GetIdeaById(context, request.IdeaId);
               if (idea != null)
               {
                   if (!string.IsNullOrEmpty(request.Title))
                   {
                       idea.Title = request.Title;
                   }
                   if (!string.IsNullOrEmpty(request.Description))
                   {
                       idea.Description = request.Description;
                   }
                   if (!string.IsNullOrEmpty(request.BusinessImpact))
                   {
                       idea.BusinessImpact = request.BusinessImpact;
                   }
                   if (!string.IsNullOrEmpty(request.Solution))
                   {
                       idea.Solution = request.Solution;
                   }
                   if (request.ChallengeId.GetValueOrDefault(0) != 0)
                   {
                       idea.ChallengeId = request.ChallengeId;
                   }
                   if (request.CategoryId.GetValueOrDefault(0) != 0)
                   {
                       idea.CategoryId = request.CategoryId;
                   }

                   idea.IsDraft = request.IsDraft;
                   idea.ModifiedDate = DateTime.UtcNow;

                   //insert idea status
                   IdeaStatusLog ideaStatusLog = new IdeaStatusLog()
                   {
                       CreatedDate = DateTime.UtcNow,
                       ModifiedByUserId = UserID,
                       IsActive = true,
                       IdeaState = request.IsDraft ? (int)IdeaStatusTypes.SubmitPending : (int)IdeaStatusTypes.ReviewPending
                   };

                   idea.IdeaStatusLogs.Add(ideaStatusLog);

                   context.SubmitChanges();
                   response.Status = Success;
               }
               else
               {
                   response.ErrorList.Add(Faults.IdeaNotFound);
                   response.Status = Failure;
               }

           }
            , readOnly: false
            );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
        }



        public RestAPIDeleteAttachmentResponse DeleteIdeaAttachment(ResponseBase response, int IdeaAttachmentId)
        {
            RestAPIDeleteAttachmentResponse restAPIDeleteAttachmentResponse = new RestAPIDeleteAttachmentResponse();

            DatabaseWrapper.databaseOperation(response,
           (context, query) =>
           {

               IdeaAttachment ideaAttachment = query.GetIdeaAttachmentById(context, IdeaAttachmentId);

               if (ideaAttachment != null)
               {
                   query.DeleteIdeaAttachment(context, ideaAttachment);
                   response.Status = Success;
               }
               else
               {
                   response.ErrorList.Add(Faults.IdeaAttachmentNotFound);
                   response.Status = Failure;
                   return;
               }

               context.SubmitChanges();
           }
            , readOnly: false
            );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
                restAPIDeleteAttachmentResponse.ErrorList = response.ErrorList;
            }

            return restAPIDeleteAttachmentResponse;


        }

        public void ArchiveIdea(RestAPIArchiveIdeaResponse response, int UserID, int IdeaId, string Description, bool isArchive = false)
        {
            IdeaArchiveHistory ideaArchiveHistory = null;

            DatabaseWrapper.databaseOperation(response,
                       (context, query) =>
                       {
                           Idea idea = query.GetIdeaArchive(context, IdeaId);

                           if (idea != null)
                           {
                               idea.IsActive = !isArchive;
                               ideaArchiveHistory = new IdeaArchiveHistory() { IdeaId = IdeaId, Description = Description, UserId = UserID, ArchiveDate = DateTime.UtcNow };

                               query.AddIdeaArchiveHistory(context, ideaArchiveHistory);
                               response.Status = Success;
                           }
                           else
                           {
                               response.ErrorList.Add(Faults.IdeaNotFound);
                               response.Status = Failure;
                               return;
                           }
                           context.SubmitChanges();
                       }
                       , readOnly: false
                   );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
        }

        public void GetArchiveIdeas(RestAPIGetArchiveIdeaResponse response, int UserId)
        {
            List<RESTAPIIdeaInterchange> ideaInterchangeList = null;
            List<Idea> ideaList = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             ideaInterchangeList = new List<RESTAPIIdeaInterchange>();
                             ideaList = new List<Idea>();

                             ideaList = query.GetArchiveIdeas(context, UserId);

                             if (ideaList.Count > 0)
                             {
                                 foreach (var idea in ideaList)
                                 {
                                     RESTAPIIdeaInterchange ideaInterchange = new RESTAPIIdeaInterchange(idea);
                                     ideaInterchangeList.Add(ideaInterchange);
                                 }
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );

            if (ideaInterchangeList != null && ideaInterchangeList.Count > 0)
            {
                response.ArchiveIdeaList.AddRange(ideaInterchangeList);
            }
        }
    }
}

