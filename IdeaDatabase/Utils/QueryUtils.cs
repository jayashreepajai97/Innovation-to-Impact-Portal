using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using NLog;
using Responses;
using SettingsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace IdeaDatabase.Utils
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class QueryUtils : IQueryUtils
    {
        private static Boolean useEager;
        private static Logger logger = LogManager.GetLogger("DatabaseSubmitChanges");

        private IQueryable<Idea> BuildIdeaQuery(IIdeaDatabaseDataContext ctx, Relation relation)
        {
            IQueryable<Idea> querableIdeas = ctx.Ideas;
            if (useEager == true)
            {
                //if (relation.HasFlag(MessagesRel))
                //    querableDevices = querableDevices.Include(x => x.DeviceMessages.Select(y => y.DeviceMessagesStatusHistories));
                //if (relation.HasFlag(UpdatesRel))
                //    querableDevices = querableDevices.Include(x => x.DeviceUpdates.Select(y => y.DeviceUpdatesStatusHistories));
                //if (relation.HasFlag(EnitlementRel))
                //    querableDevices = querableDevices.Include(x => x.DeviceEntitlement);
                //if (relation.HasFlag(PropertiesRel))
                //    querableDevices = querableDevices.Include(x => x.DeviceProperty);
                //if (relation.HasFlag(AccessoriesRel))
                //    querableDevices = querableDevices.Include(x => x.DeviceAccessories);
                //if (relation.HasFlag(InternetAndSecurityRel))
                //    querableDevices = querableDevices.Include(x => x.InternetAndSecurity);
                //if (relation.HasFlag(SoftwareRel))
                //    querableDevices = querableDevices.Include(x => x.Softwares);
                //if (relation.HasFlag(SpecificationRel))
                //    querableDevices = querableDevices.Include(x => x.Specification);
                //if (relation.HasFlag(StorageRel))
                //    querableDevices = querableDevices.Include(x => x.Storages);
                //if (relation.HasFlag(ResourcesRel))
                //    querableDevices = querableDevices.Include(x => x.DeviceResources);
            }
            return querableIdeas;
        }

        static QueryUtils()
        {
            SettingRepository.TryGet<bool>("EnableEagerDatabaseAccess", out useEager);
        }

        public static string GetMD5(string token)
        {
            if (token == null)
            {
                return null;
            }
            string hash = BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName("MD5"))
                .ComputeHash(new UTF8Encoding().GetBytes(token)));
            return hash.Replace("-", "").ToLower();
        }
        public void AddUser(IIdeaDatabaseDataContext dc, User u)
        {
            dc.Users.Add(u);
            dc.SubmitChanges();
        }

        public User GetUser(IIdeaDatabaseDataContext dc, int UserID)
        {
            return dc.Users.Select(x => x).Where(x => x.UserId == UserID).FirstOrDefault();
        }

        public string GetUserEmail(IIdeaDatabaseDataContext dc, int UserID)
        {
            return dc.Users.Where(x => x.UserId == UserID).Select(x => x.EmailAddress).FirstOrDefault();
        }

        public List<IdeaEmailToDetails> GetAllStakeholdersEmailAdd(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            // Need two separate db calls since 1 Assignment in IdeaAssigmenst tabel correspond to 2 status. Ex When a idea is created it means it is in Submitted state and Ready for review state. 
            // For Ex When an Idea is in Submitted stae for getting all stakeholders we need to query Staus log for Submitter's details and IdeaAssignment for Reviweer's details.
            List<IdeaEmailToDetails> resAssignments = new List<IdeaEmailToDetails>();
            // Get users from Status log table
            var resStatusLog = (from usrs in dc.Users
                                join log in dc.IdeaStatusLogs on usrs.UserId equals log.ModifiedByUserId
                                where log.IdeaId == IdeaId
                                orderby log.IdeaState descending
                                select new IdeaEmailToDetails { IdeaState = log.IdeaState, EmailAddress = usrs.EmailAddress }).ToList();

            if (resStatusLog[0].IdeaState < 3)
            {
                resAssignments = (from usrs in dc.Users
                                  join asgn in dc.IdeaAssignments on usrs.UserId equals asgn.ReviewByUserId
                                  where asgn.IdeaId == IdeaId
                                  select new IdeaEmailToDetails { IdeaState = 0, EmailAddress = usrs.EmailAddress }).ToList();
            }

            resStatusLog.AddRange(resAssignments);
            return resStatusLog;
        }

        public UserAuthentication GetAuthenticationByToken(IIdeaDatabaseDataContext ctx, string Token, string CallerId)
        {
            return ctx.UserAuthentications.Where(x => x.TokenMD5 == Token && x.CallerId == CallerId).FirstOrDefault();
        }

        public UserAuthentication GetHPPAuthenticationByUserID(IIdeaDatabaseDataContext ctx, int UserId, string SessionToken, string CallerId)
        {
            return ctx.UserAuthentications.Where(x => x.UserId == UserId && x.TokenMD5 == SessionToken && x.CallerId == CallerId).FirstOrDefault();
        }

        public User GetHPIDProfile(IIdeaDatabaseDataContext ctx, string ProfileId)
        {
            return ctx.Users.Select(x => x).Where(x => x.HPIDprofileId == ProfileId).FirstOrDefault();
        }

        public User GetProfileByUserID(IIdeaDatabaseDataContext ctx, int userId)
        {
            return ctx.Users.Select(x => x).Where(x => x.UserId == userId).FirstOrDefault();
        }
        public User GetProfile(IIdeaDatabaseDataContext ctx, string ProfileId)
        {
            return ctx.Users.Select(x => x).Where(x => x.ProfileId == ProfileId).FirstOrDefault();
        }

        public void SetHPPToken(IIdeaDatabaseDataContext ctx, UserAuthentication hppAuth)
        {
            ctx.UserAuthentications.Add(hppAuth);
            ctx.SubmitChanges();
        }

        public IEnumerable<AdmSetting> GetAdmSettings(IIdeaDatabaseDataContext ctx)
        {
            return ctx.AdmSettings.ToList();
        }

        public UserAuthentication GetHPPToken(IIdeaDatabaseDataContext ctx, int UserId, string callerId)
        {
            return ctx.UserAuthentications.FirstOrDefault(x => x.UserId == UserId && x.CallerId == callerId);
        }

        public UserAuthentication GetHPPToken(IIdeaDatabaseDataContext ctx, int UserId, string TokenOrTokenMD5, string CallerId)
        {
            string TokenMD5;
            if (!string.IsNullOrEmpty(TokenOrTokenMD5) && TokenOrTokenMD5.Length > 128)
            {
                TokenMD5 = GetMD5(TokenOrTokenMD5);
            }
            else
            {
                TokenMD5 = TokenOrTokenMD5;
            }

            UserAuthentication hppAuth = ctx.UserAuthentications.FirstOrDefault(x => x.UserId == UserId && x.TokenMD5 == TokenMD5 && x.CallerId == CallerId);
            if (hppAuth != null && (hppAuth?.ModifiedDate == null || hppAuth.ModifiedDate.Date != DateTime.UtcNow.Date))
            {
                UpdateHPPAuthenticationDateUpdated(hppAuth);
            }
            return hppAuth;
        }
        public UserAuthentication GetHPPToken(IIdeaDatabaseDataContext ctx, string Token, string CallerId)
        {
            string tokenMD5 = GetMD5(Token);
            UserAuthentication hppAuth = ctx.UserAuthentications.FirstOrDefault(x => x.TokenMD5 == tokenMD5 && x.CallerId == CallerId);
            if (hppAuth != null && (hppAuth?.ModifiedDate == null || hppAuth.ModifiedDate.Date != DateTime.UtcNow.Date))
            {
                UpdateHPPAuthenticationDateUpdated(hppAuth);
            }
            return hppAuth;
        }

        private void UpdateHPPAuthenticationDateUpdated(UserAuthentication hppAuth)
        {
            try
            {
                IIdeaDatabaseDataContext writableContext = DependencyInjector.Get<IIdeaDatabaseDataContext, IdeaDatabaseReadWrite>();
                DatabaseWrapper.databaseOperation(new ResponseBase(), writableContext, (context) =>
                {
                    UserAuthentication writableHPPAuth = context.UserAuthentications.Where(x => x.UserId == hppAuth.UserId && x.TokenMD5 == hppAuth.TokenMD5 && x.CallerId == hppAuth.CallerId).FirstOrDefault();

                    writableHPPAuth.ModifiedDate = DateTime.UtcNow;
                    context.SubmitChanges();
                }
                );
            }
            catch (Exception ex)
            {
                logger.Error($"Exception {ex.Message} while updating dateUpdated in UserAuthentication for UserID : {hppAuth.UserId} and CallerId : {hppAuth.CallerId}");
            }
        }

        public Idea GetIdea(IIdeaDatabaseDataContext ctx, int IdeaId, int UserID, Relation r)
        {
            IQueryable<Idea> ideas = BuildIdeaQuery(ctx, r);
            return ideas.FirstOrDefault(t => t.IdeaId == IdeaId && t.UserId == UserID && t.IsActive != false);
        }

        public void AddUserRoleMapping(IIdeaDatabaseDataContext dc, RoleMapping roleMapping)
        {
            dc.RoleMappings.Add(roleMapping);
        }

        public void AddUserRoleMappings(IIdeaDatabaseDataContext dc, List<RoleMapping> roleMappings)
        {
            dc.RoleMappings.AddRange(roleMappings);
        }

        public Role GetRole(IIdeaDatabaseDataContext ctx, int RoleId)
        {
            return ctx.Roles.Where(r => r.RoleId == RoleId && r.IsActive == true).FirstOrDefault();
        }

        public List<Role> GetRoles(IIdeaDatabaseDataContext ctx, int[] roles)
        {
            return ctx.Roles.Where(r => roles.Contains(r.RoleId) && r.IsActive == true).ToList();
        }

        public RoleMapping GetUserRoleMapping(IIdeaDatabaseDataContext ctx, int userId, int RoleId)
        {
            throw new NotImplementedException();
        }
        public List<RoleMapping> GetUserRoleMappings(IIdeaDatabaseDataContext ctx, int userId)
        {
            return ctx.RoleMappings.Where(r => r.UserId == userId).ToList();
        }

        public bool CheckUserRoleID(IIdeaDatabaseDataContext dc, int userId, int[] RoleID)
        {
            bool result = false;

            foreach (int roleid in RoleID)
            {
                result = dc.RoleMappings.Any(x => x.RoleId == roleid && x.UserId == userId);

                if (result == false)
                {
                    result = false;
                }
            }

            return result;
        }

        public bool CheckIdeaStatus(IIdeaDatabaseDataContext dc, int status)
        {
            return dc.IdeaStatusLogs.Any(x => x.IdeaState == status);
        }

        public void AddIdeaStatus(IIdeaDatabaseDataContext dc, IdeaStatusLog ideaState)
        {
            if (ideaState != null)
            {
                dc.IdeaStatusLogs.Add(ideaState);
            }
        }

        public List<IdeaStatusLog> GetIdeaStatus(IIdeaDatabaseDataContext dc)
        {
            return dc.IdeaStatusLogs.ToList();
        }

        public List<Role> GetRole(IIdeaDatabaseDataContext dc)
        {
            return dc.Roles.ToList();
        }

        public bool GetRoleByName(IIdeaDatabaseDataContext dc, string RoleName)
        {
            bool result = dc.Roles.Any(x => x.RoleName == RoleName);
            return result;
        }

        public void AddRole(IIdeaDatabaseDataContext dc, Role role)
        {
            if (role != null)
            {
                dc.Roles.Add(role);
            }
        }

        public IdeaCategory GetCategoreByName(IIdeaDatabaseDataContext dc, string Category)
        {
            return dc.IdeaCategories.Where(x => x.CategoriesName == Category).FirstOrDefault();
        }

        public List<IdeaCategory> GetCategories(IIdeaDatabaseDataContext dc)
        {
            return dc.IdeaCategories.ToList();
        }

        public void AddIdeaCategory(IIdeaDatabaseDataContext dc, IdeaCategory ideacategory)
        {
            if (ideacategory != null)
            {
                dc.IdeaCategories.Add(ideacategory);
            }
        }

        //public bool GetCategoreByName(IIdeaDatabaseDataContext dc, string categoriesName, int addedByUserId)
        //{

        //    bool result = dc.IdeaCategories.Any(x => (x.CategoriesName == categoriesName && x.AddedByUserId == addedByUserId));

        //    return result;
        //}

        public void DeleteIdeaCategory(IIdeaDatabaseDataContext ctx, IdeaCategory ideacategory)
        {
            ctx.IdeaCategories.Remove(ideacategory);
        }

        public void AddIdeaStatusHistory(IIdeaDatabaseDataContext dc, IdeaStatusLog ideaState)
        {
            dc.IdeaStatusLogs.Add(ideaState);
        }

        public void AddIdeaComment(IIdeaDatabaseDataContext dc, IdeaComment ideaComment)
        {
            if (ideaComment != null)
            {
                dc.IdeaComments.Add(ideaComment);
            }
        }

        public List<IdeaComment> GetIdeaComment(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            return dc.IdeaComments.Where(x => x.IdeaId == IdeaId).ToList();
        }

        public IdeaComment GetIdeaCommentById(IIdeaDatabaseDataContext dc, int IdeaCommentId)
        {
            return dc.IdeaComments.Where(x => x.IdeaCommentId == IdeaCommentId).FirstOrDefault();
        }

        public void DeleteIdeaComment(IIdeaDatabaseDataContext dc, IdeaComment ideaComment)
        {
            if (ideaComment != null)
            {
                dc.IdeaComments.Remove(ideaComment);
            }
        }

        public Idea GetIdeaById(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            return dc.Ideas.Where(x => x.IdeaId == IdeaId && x.IsActive == true).FirstOrDefault();
        }

        public List<User> GetUsers(IIdeaDatabaseDataContext dc, string SearchName)
        {
            return dc.Users.Where(x => x.EmailAddress.Contains(SearchName)).ToList();
        }

        public IdeaStatusLog GetIdeaStatusId(IIdeaDatabaseDataContext dc, int Status)
        {
            return dc.IdeaStatusLogs.Where(x => x.IdeaState == Status).FirstOrDefault();
        }

        public void AddIdea(IIdeaDatabaseDataContext dc, Idea idea)
        {
            if (idea != null)
            {
                dc.Ideas.Add(idea);
            }
        }

        public void AddIdeaAttachments(IIdeaDatabaseDataContext dc, List<IdeaAttachment> ideaAttachments)
        {
            if (ideaAttachments.Count != 0)
            {
                dc.IdeaAttachments.AddRange(ideaAttachments);
            }
        }

        public IdeaCategory GetIdeaFromCategoryID(IIdeaDatabaseDataContext dc, int categoryid)
        {
            return dc.IdeaCategories.Where(d => d.IdeaCategorieId == categoryid).FirstOrDefault();
        }

        public List<Idea> GetIdeaByUserId(IIdeaDatabaseDataContext dc, int UserId)
        {
            return dc.Ideas.Where(x => x.UserId == UserId).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<Idea> GetIdeas(IIdeaDatabaseDataContext dc, int UserId)
        {
            return dc.Ideas.Where(x => x.UserId == UserId && x.IsActive == true).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<Idea> GetIdeasForReview(IIdeaDatabaseDataContext dc, int UserId)
        {
            var res = (from i in dc.Ideas
                       where i.IsActive == true
                        where
                         (from ia in dc.IdeaAssignments
                          where ia.ReviewByUserId == UserId
                          select ia.IdeaId).Union(from isl in dc.IdeaStatusLogs
                                                  where isl.ModifiedByUserId == UserId
                                                  where isl.IdeaState == (int)IdeaStatusTypes.SponsorPending
                                                  select isl.IdeaId).Contains(i.IdeaId)
                          select i).ToList();

            return res;
        }

        public List<Idea> GetIdeasForSponsors(IIdeaDatabaseDataContext dc, int UserId)
        {
            var res = (from i in dc.Ideas
                       where i.IsActive == true
                       where
                        (from ia in dc.IdeaAssignments
                         where ia.ReviewByUserId == UserId
                         select ia.IdeaId).Union(from isl in dc.IdeaStatusLogs
                                                 where isl.ModifiedByUserId == UserId
                                                 where isl.IdeaState == (int)IdeaStatusTypes.Sponsored
                                                 select isl.IdeaId).Contains(i.IdeaId)
                         select i).ToList();

            return res;
        }

        public List<Idea> GetPublicIdeas(IIdeaDatabaseDataContext dc, int UserId, int CategoryId)
        {
            if (CategoryId != 0)
            {
                return dc.Ideas.Where(x => x.UserId != UserId && x.CategoryId == CategoryId && x.IsSensitive == false && x.IsActive == true).ToList();
            }

            return dc.Ideas.Where(x => x.UserId != UserId && x.IsSensitive == false && x.IsActive == true).ToList();
        }

        public List<Idea> GetSortedPublicIdeas(IIdeaDatabaseDataContext dc, int UserId, int CategoryId)
        {
            return dc.Ideas.Where(x => x.UserId != UserId && x.CategoryId == CategoryId && x.IsSensitive == false && x.IsActive == true).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public User GetUserDetails(IIdeaDatabaseDataContext dc, int UserId)
        {
            return dc.Users.Where(x => x.UserId == UserId).FirstOrDefault();
        }

        public List<RoleMapping> GetUserRoles(IIdeaDatabaseDataContext dc, int UserId)
        {
            return dc.RoleMappings.Where(x => x.UserId == UserId).ToList();
        }

        public IdeaCategory GetCategoreById(IIdeaDatabaseDataContext dc, int CategoryId)
        {
            return dc.IdeaCategories.Where(x => x.IdeaCategorieId == CategoryId).FirstOrDefault();
        }
        public bool CheckIfIdeaExistsForCategory(IIdeaDatabaseDataContext dc, int CategoryId)
        {
            return dc.Ideas.Any(x => x.CategoryId == CategoryId);
        }


        public Role GetAllRoleMappings(IIdeaDatabaseDataContext dc, string roleame)
        {
            return dc.Roles.Where(r => r.RoleName.Equals(roleame)).FirstOrDefault();
        }

        public void AddUserAssignmentToIdea(IIdeaDatabaseDataContext dc, List<IdeaAssignment> ideaAssignmentsList)
        {
            dc.IdeaAssignments.AddRange(ideaAssignmentsList);
        }

        public void AddIdeaCommentDiscussion(IIdeaDatabaseDataContext dc, IdeaCommentDiscussion ideaCommentDiscussion)
        {
            if (ideaCommentDiscussion != null)
            {
                dc.IdeaCommentDiscussions.Add(ideaCommentDiscussion);
            }
        }

        public List<Idea> SearchIdea(IIdeaDatabaseDataContext dc, int categoryId, string searchCriteria)
        {
            var resCat = (from a in dc.Ideas
                          join c in dc.IdeaCategories on a.CategoryId equals c.IdeaCategorieId into ideaGroup
                          from ideRes in ideaGroup.DefaultIfEmpty()
                          where (a.Description.Contains(searchCriteria) || a.Title.Contains(searchCriteria) || ideRes.CategoriesName.Contains(searchCriteria))
                          where a.IsActive == true
                          select a).Distinct().ToList();

            var resTag = (from ideas in dc.Ideas
                          join tags in dc.Ideatags on ideas.IdeaId equals tags.IdeaId into ideaTagGroup
                          from ideRes in ideaTagGroup.DefaultIfEmpty()
                          where (ideas.Description.Contains(searchCriteria) || ideas.Title.Contains(searchCriteria) || ideRes.Tags.Contains(searchCriteria))
                          where ideas.IsActive == true
                          select ideas).Distinct().ToList();

            var res = resCat.Union(resTag).Distinct().OrderByDescending(x => x.IdeaId).ToList();
            return res;

        }

        public IdeaStatusLog GetIdeaStatusByIdeaId(IIdeaDatabaseDataContext dc, int ideaId)
        {
            return dc.IdeaStatusLogs.Where(x => x.IdeaId == ideaId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        public void UpdateAllStatus(IIdeaDatabaseDataContext dc, int ideaId)
        {
            var statuslogs = dc.IdeaStatusLogs.Where(x => x.IdeaId == ideaId).ToList();

            foreach (var status in statuslogs)
            {
                if (status != null)
                {
                    status.IsActive = false;
                }
            }
        }

        public RoleMapping GetUserRoleMappingByRoleId(IIdeaDatabaseDataContext dc, int RoleId)
        {
            return dc.RoleMappings.Where(x => x.RoleId == RoleId).FirstOrDefault();
        }

        public IdeaAssignment GetAssignmentByIdeaId(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            return dc.IdeaAssignments.Where(x => x.IdeaId == IdeaId).FirstOrDefault();
        }

        public void AddIdeaAssignment(IIdeaDatabaseDataContext dc, IdeaAssignment ideaAssignment)
        {
            if (ideaAssignment != null)
            {
                dc.IdeaAssignments.Add(ideaAssignment);
            }
        }

        public void UpdateAssignmentUser(IIdeaDatabaseDataContext dc, int UserId)
        {

        }

        public RESTAPIIdeaBasicDetailsInterchange GetIdeaBasicDetails(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            RESTAPIIdeaBasicDetailsInterchange ideaBasicDetailsInterchange = null;

            ideaBasicDetailsInterchange = (from idea in dc.Ideas
                                           where idea.IdeaId == IdeaId
                                           select new RESTAPIIdeaBasicDetailsInterchange() { IdeaId = idea.IdeaId, Title = idea.Title }
                                              ).FirstOrDefault();
            return ideaBasicDetailsInterchange;
        }

        public IdeaChallenge GetChallengeByName(IIdeaDatabaseDataContext dc, string challengeName)
        {
            return dc.IdeaChallenges.Where(x => x.ChallengeName == challengeName).FirstOrDefault();
        }

        public void AddIdeaChallenge(IIdeaDatabaseDataContext dc, IdeaChallenge ideaChallenge)
        {
            if (ideaChallenge != null)
            {
                dc.IdeaChallenges.Add(ideaChallenge);
            }
        }

        public List<IdeaChallenge> GetIdeaChallenges(IIdeaDatabaseDataContext dc)
        {
            return dc.IdeaChallenges.ToList();
        }

        public void AddGitRepo(IIdeaDatabaseDataContext dc, string GitRepo)
        {
            throw new NotImplementedException();
        }

        public void AddIdeaTag(IIdeaDatabaseDataContext dc, Ideatag ideatag)
        {
            if (ideatag != null)
            {
                dc.Ideatags.Add(ideatag);
            }
        }


        public void AddIdeaTags(IIdeaDatabaseDataContext dc, List<Ideatag> ideatag)
        {
            if (ideatag.Count != 0)
            {
                dc.Ideatags.AddRange(ideatag);
            }
        }

        public EmailTemplate GetEmailTemplate(IIdeaDatabaseDataContext dc, string emailTemplate)
        {
            return dc.EmailTemplates.Where(x => x.Event == emailTemplate).FirstOrDefault();
        }
        public Ideatag GetIdeatag(IIdeaDatabaseDataContext dc, int IdeaID, string Tags)
        {
            return dc.Ideatags.Where(x => x.IdeaId == IdeaID && x.Tags == Tags).FirstOrDefault();
        }

        public bool GetIdeatags(IIdeaDatabaseDataContext dc, int IdeaID, string Tags)
        {
            return dc.Ideatags.Any(x => x.IdeaId == IdeaID && x.Tags == Tags);
        }

        public void DeleteIdeaTags(IIdeaDatabaseDataContext dc, int IdeaID)
        {
            var tags = dc.Ideatags.Where(x => x.IdeaId == IdeaID).ToList();

            dc.Ideatags.RemoveRange(tags);

        }

        public List<User> GetUserByNames(IIdeaDatabaseDataContext dc, List<string> UserNames)
        {
            List<User> userList = new List<User>();
            foreach (var name in UserNames)
            {
                var user = dc.Users.Where(x => x.EmailAddress.Contains(name.Trim())).FirstOrDefault();
                userList.Add(user);
            }
            return userList;
        }

        public void AddIdeaIntellectual(IIdeaDatabaseDataContext dc, IdeaIntellectualProperty ideaIntellectual)
        {
            if (ideaIntellectual != null)
            {
                dc.IdeaIntellectualProperties.Add(ideaIntellectual);
            }
        }

        public IdeaIntellectualProperty GetIdeaIntellectualById(IIdeaDatabaseDataContext dc, int IntellectualId)
        {
            return dc.IdeaIntellectualProperties.Where(x => x.IntellectualId == IntellectualId).FirstOrDefault();
        }

        public void DeleteIdeaIntellectual(IIdeaDatabaseDataContext dc, IdeaIntellectualProperty ideaIntellectualProperty)
        {
            if (ideaIntellectualProperty != null)
            {
                dc.IdeaIntellectualProperties.Remove(ideaIntellectualProperty);
            }
        }

        public List<Idea> GetDraftIdeas(IIdeaDatabaseDataContext dc, int UserId)
        {
            return dc.Ideas.Where(x => x.UserId == UserId && x.IsDraft == true && x.IsActive == true).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IdeaChallenge GetChallengeByID(IIdeaDatabaseDataContext dc, int challengeId)
        {
            return dc.IdeaChallenges.Where(x => x.IdeaChallengeId == challengeId).FirstOrDefault();
        }

        public IdeaAttachment GetIdeaAttachmentById(IIdeaDatabaseDataContext dc, int IdeaAttachmentId)
        {
            return dc.IdeaAttachments.Where(x => x.IdeaAttachmentId == IdeaAttachmentId).FirstOrDefault();
        }

        public void DeleteIdeaAttachment(IIdeaDatabaseDataContext dc, IdeaAttachment ideaAttachment)
        {
            if (ideaAttachment != null)
            {
                dc.IdeaAttachments.Remove(ideaAttachment);
            }
        }

        public List<string> GetIdeatags(IIdeaDatabaseDataContext dc, int IdeaID)
        {
            return dc.Ideatags.Where(x => x.IdeaId == IdeaID).Select(x => x.Tags).ToList();
        }

        public void DeleteDefaultImageAttachment(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            var attachmentList = dc.IdeaAttachments.Where(x => x.IdeaId == IdeaId && x.FolderName == folderNames.DefaultImage.ToString()).ToList();
            dc.IdeaAttachments.RemoveRange(attachmentList);
        }

        public void AddIdeaLog(IIdeaDatabaseDataContext dc, IdeaLog ideaLog)
        {
            if (ideaLog != null)
                dc.IdeaLogs.Add(ideaLog);
        }

        public List<IdeaLog> GetIdeaMetrics(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            return dc.IdeaLogs.Where(x => x.IdeaId == IdeaId).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public void AddIdeaArchiveHistory(IIdeaDatabaseDataContext dc, IdeaArchiveHistory ideaArchiveHistory)
        {
            if (ideaArchiveHistory != null)
                dc.IdeaArchiveHistories.Add(ideaArchiveHistory);
        }

        public Idea GetIdeaArchive(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            return dc.Ideas.Where(x => x.IdeaId == IdeaId).FirstOrDefault();
        }

        public List<Idea> GetArchiveIdeas(IIdeaDatabaseDataContext dc, int UserId)
        {
            return dc.Ideas.Where(x => x.UserId == UserId && x.IsActive == false).ToList();
        }

        public List<IdeaAttachment> GetIdeaAttachmentsByIdeaId(IIdeaDatabaseDataContext dc, int IdeaId)
        {
            return dc.IdeaAttachments.Where(x => x.IdeaId == IdeaId).ToList();
        }
    }
}
