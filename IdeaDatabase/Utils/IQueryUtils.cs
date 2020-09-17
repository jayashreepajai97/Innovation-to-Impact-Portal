using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using System.Collections.Generic;

namespace IdeaDatabase.Utils
{
    public interface IQueryUtils
    {

        void AddUser(IIdeaDatabaseDataContext dc, User user);
        User GetUser(IIdeaDatabaseDataContext dc, int UserID);
        User GetProfileByUserID(IIdeaDatabaseDataContext ctx, int userId);
        User GetHPIDProfile(IIdeaDatabaseDataContext ctx, string ProfileId);
        UserAuthentication GetAuthenticationByToken(IIdeaDatabaseDataContext ctx, string Token, string CallerId);

        void SetHPPToken(IIdeaDatabaseDataContext ctx, UserAuthentication hppAuthentication);
        UserAuthentication GetHPPAuthenticationByUserID(IIdeaDatabaseDataContext ctx, int UserId, string SessionToken, string CallerId);
        IdeaCategory GetCategoreByName(IIdeaDatabaseDataContext context, string categoriesName);
        User GetProfile(IIdeaDatabaseDataContext ctx, string ProfileId);
        void AddIdeaCategory(IIdeaDatabaseDataContext context, IdeaCategory ideacategory);
        RoleMapping GetUserRoleMapping(IIdeaDatabaseDataContext ctx, int userId, int RoleId);

        Role GetRole(IIdeaDatabaseDataContext ctx, int RoleId);
        List<Role> GetRoles(IIdeaDatabaseDataContext ctx, int[] roles);
        List<RoleMapping> GetUserRoleMappings(IIdeaDatabaseDataContext ctx, int userId);

        void AddUserRoleMapping(IIdeaDatabaseDataContext dc, RoleMapping roleMapping);
        void AddUserRoleMappings(IIdeaDatabaseDataContext dc, List<RoleMapping> roleMappings);

        UserAuthentication GetHPPToken(IIdeaDatabaseDataContext ctx, int UserId, string callerId);
        UserAuthentication GetHPPToken(IIdeaDatabaseDataContext ctx, int UserId, string TokenOrTokenMD5, string CallerId);

        UserAuthentication GetHPPToken(IIdeaDatabaseDataContext ctx, string token, string callerId);

        Idea GetIdea(IIdeaDatabaseDataContext ctx, int IdeaId, int UserID, Relation r);
        IEnumerable<AdmSetting> GetAdmSettings(IIdeaDatabaseDataContext ctx);
        void DeleteIdeaCategory(IIdeaDatabaseDataContext context, IdeaCategory ideacategory);
        bool CheckUserRoleID(IIdeaDatabaseDataContext dc, int userId, int[] RoleID = null);

        bool CheckIdeaStatus(IIdeaDatabaseDataContext dc, int status);

        void AddIdeaStatus(IIdeaDatabaseDataContext dc, IdeaStatusLog ideaState);


        List<IdeaStatusLog> GetIdeaStatus(IIdeaDatabaseDataContext dc);


        List<Role> GetRole(IIdeaDatabaseDataContext dc);
        bool GetRoleByName(IIdeaDatabaseDataContext dc, string RoleName);
        void AddRole(IIdeaDatabaseDataContext dc, Role role);
        List<IdeaCategory> GetCategories(IIdeaDatabaseDataContext dc);

        void AddIdeaStatusHistory(IIdeaDatabaseDataContext dc, IdeaStatusLog ideaState);
        void AddIdeaComment(IIdeaDatabaseDataContext dc, IdeaComment ideaComment);
        List<IdeaComment> GetIdeaComment(IIdeaDatabaseDataContext dc, int IdeaId);
        IdeaComment GetIdeaCommentById(IIdeaDatabaseDataContext dc, int IdeaCommentId);
        void DeleteIdeaComment(IIdeaDatabaseDataContext dc, IdeaComment ideaComment);
        Idea GetIdeaById(IIdeaDatabaseDataContext dc, int IdeaId);

        void AddIdeaAttachments(IIdeaDatabaseDataContext dc, List<IdeaAttachment> ideaAttachments);
    
        List<User> GetUsers(IIdeaDatabaseDataContext dc, string SearchName);
        IdeaStatusLog GetIdeaStatusId(IIdeaDatabaseDataContext dc, int Status);
        IdeaCategory GetIdeaFromCategoryID(IIdeaDatabaseDataContext dc, int categoryid);
        void AddIdea(IIdeaDatabaseDataContext dc, Idea idea);
        List<Idea> GetIdeaByUserId(IIdeaDatabaseDataContext dc, int UserId);
        List<Idea> GetIdeas(IIdeaDatabaseDataContext dc, int UserId);
        List<Idea> GetIdeasForReview(IIdeaDatabaseDataContext dc, int UserId);
        List<Idea> GetIdeasForSponsors(IIdeaDatabaseDataContext dc, int UserId);
        
        List<Idea> GetPublicIdeas(IIdeaDatabaseDataContext dc, int UserId, int CategoryId);
        List<Idea> GetSortedPublicIdeas(IIdeaDatabaseDataContext dc, int UserId, int CategoryId);
        User GetUserDetails(IIdeaDatabaseDataContext dc, int UserId);
        List<RoleMapping> GetUserRoles(IIdeaDatabaseDataContext dc, int UserId);
        IdeaCategory GetCategoreById(IIdeaDatabaseDataContext dc, int CategoryId);
        bool CheckIfIdeaExistsForCategory(IIdeaDatabaseDataContext dc, int CategoryId);

        Role GetAllRoleMappings(IIdeaDatabaseDataContext dc, string roleame);
        void AddUserAssignmentToIdea(IIdeaDatabaseDataContext dc, List<IdeaAssignment> ideaAssignmentsList);
        void AddIdeaCommentDiscussion(IIdeaDatabaseDataContext dc, IdeaCommentDiscussion ideaCommentDiscussion);
        List<Idea> SearchIdea(IIdeaDatabaseDataContext context, int UserID, string title);

        IdeaStatusLog GetIdeaStatusByIdeaId(IIdeaDatabaseDataContext dc, int ideaId);
        void UpdateAllStatus(IIdeaDatabaseDataContext dc, int ideaId);
        string GetUserEmail(IIdeaDatabaseDataContext dc, int UserId);
        RoleMapping GetUserRoleMappingByRoleId(IIdeaDatabaseDataContext dc, int RoleId);
        IdeaAssignment GetAssignmentByIdeaId(IIdeaDatabaseDataContext dc, int IdeaId);
        void AddIdeaAssignment(IIdeaDatabaseDataContext dc, IdeaAssignment ideaAssignment);
        void UpdateAssignmentUser(IIdeaDatabaseDataContext dc, int UserId);
        List<IdeaEmailToDetails> GetAllStakeholdersEmailAdd(IIdeaDatabaseDataContext dc, int IdeaId);
        RESTAPIIdeaBasicDetailsInterchange GetIdeaBasicDetails(IIdeaDatabaseDataContext dc, int IdeaId);
        IdeaChallenge GetChallengeByName(IIdeaDatabaseDataContext dc, string challengeName);
        void AddIdeaChallenge(IIdeaDatabaseDataContext dc, IdeaChallenge ideaChallenge);
        List<IdeaChallenge> GetIdeaChallenges(IIdeaDatabaseDataContext dc);
        void AddGitRepo(IIdeaDatabaseDataContext dc, string GitRepo);
        void AddIdeaTag(IIdeaDatabaseDataContext dc, Ideatag ideatag);
        void AddIdeaTags(IIdeaDatabaseDataContext dc, List<Ideatag> ideatag);
        Ideatag GetIdeatag (IIdeaDatabaseDataContext dc, int IdeaID, string Tags);
        bool GetIdeatags(IIdeaDatabaseDataContext dc, int IdeaID, string Tags);
        EmailTemplate GetEmailTemplate(IIdeaDatabaseDataContext dc, string emailTemplate);
        void DeleteIdeaTags(IIdeaDatabaseDataContext dc, int IdeaID);
        List<User> GetUserByNames(IIdeaDatabaseDataContext dc, List<string> UserNames);
        void AddIdeaIntellectual(IIdeaDatabaseDataContext dc, IdeaIntellectualProperty ideaIntellectual);
        IdeaIntellectualProperty GetIdeaIntellectualById(IIdeaDatabaseDataContext dc, int IntellectualId);
        void DeleteIdeaIntellectual(IIdeaDatabaseDataContext dc, IdeaIntellectualProperty ideaIntellectualProperty);
        List<Idea> GetDraftIdeas(IIdeaDatabaseDataContext dc, int UserId);
        IdeaChallenge GetChallengeByID(IIdeaDatabaseDataContext dc, int challengeId);
        IdeaAttachment GetIdeaAttachmentById(IIdeaDatabaseDataContext context, int ideaAttachmentId);
        void DeleteIdeaAttachment(IIdeaDatabaseDataContext context, IdeaAttachment ideaAttachment);
        List<string> GetIdeatags(IIdeaDatabaseDataContext dc, int IdeaID);
        void DeleteDefaultImageAttachment(IIdeaDatabaseDataContext dc, int IdeaId);
        void AddIdeaLog(IIdeaDatabaseDataContext dc, IdeaLog ideaLog);
        List<IdeaLog> GetIdeaMetrics(IIdeaDatabaseDataContext dc, int IdeaId);
        void AddIdeaArchiveHistory(IIdeaDatabaseDataContext dc, IdeaArchiveHistory ideaArchiveHistory);
        Idea GetIdeaArchive(IIdeaDatabaseDataContext dc, int IdeaId);
        List<Idea> GetArchiveIdeas(IIdeaDatabaseDataContext dc, int UserId);
        List<IdeaAttachment> GetIdeaAttachmentsByIdeaId(IIdeaDatabaseDataContext dc, int IdeaId);


    }
}
