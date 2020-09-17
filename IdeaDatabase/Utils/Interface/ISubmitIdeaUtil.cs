using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using IdeaDatabase.Responses;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
    public interface ISubmitIdeaUtil
    {
        void SubmitIdeaRequest(RestAPISubmitIdeaResponse response, SubmitIdeaRequest submitIdeaRequest, int UserId);
 
        RestAPIAddIdeaStateResponse InsertIdeaStatus(RestAPIAddIdeaStateResponse response, int IdeaID, /*string IdeaStatus,*/ int UserId);
        IdeaComment InsertIdeaComment(ResponseBase response, int IdeaID, string CommentDescription, int UserId);
        void GetUserComments(RestAPIGetUserCommentsResponse response, int IdeaId);
        RestAPIDeleteIdeaResponse DeleteIdeaComment(ResponseBase response, int IdeaCommentId);
        Idea UpdateSensitive(ResponseBase response, int UserId, int IdeaID, bool isSensitive);
        void GetIdeas(RestAPIGetUserIdeaResponse response, int UserId, bool IsDraft= false);

        void GetIdeaReviewsList(RestAPIGetUserIdeaStatusResponse response, int UserId);
        void GetIdeaSponsorsList(RestAPIGetUserIdeaStatusResponse response, int UserId);
        void SearchIdea(RestAPISearchIdeaResponse response, string title, int UserID);
        void GetPublicIdeas(RestAPIGetUserIdeaResponse response, int UserId, int CategoryId = 0, bool Sort = false );

        void SubmitIdeaAssignment(ResponseBase response, int IdeaId);
        IdeaCommentDiscussion InsertCommentReply(ResponseBase response, int IdeaCommentId, int UserId, string DiscussionDescription);

        void GetIdeasDetails(RESTGetUserIdeaDetailsResponse response, int ideaId);

        List<IdeaEmailToDetails> GetAllStakeholdersEmailAdd(int IdeaId);
        RESTAPIIdeaBasicDetailsInterchange GetIdeaBasicDetails(int IdeaId);
        void UpdateIdea(RestAPIUpdateIdeaResponse response,int UserID, int IdeaId, RestAPIUpdateIdeaRequest request);
        void SubmitIdeaIntellectual(RestAPIAddIntellectualResponse response, SubmitIntellectualRequest request);
        void DeleteIntellectProperty(ResponseBase response, int IntellectualId);
        void UpdateIntellectProperty(RestAPIUpdateIntellectResponse response, UpdateIntellectualRequest request);
        void GetIntellectualProperties(RestAPIGetIntellectualResponse response, int IntellectualId);
        void UpdateIdeaDraft(RestAPIUpdateDraftResponse response, RestAPIUpdateIdeaDraftRequest request, int UserID);
        RestAPIDeleteAttachmentResponse DeleteIdeaAttachment(ResponseBase response, int ideaAttachmentId);
        void ArchiveIdea(RestAPIArchiveIdeaResponse response,int UserID, int IdeaId, string Description, bool isArchive = false);
        void GetArchiveIdeas(RestAPIGetArchiveIdeaResponse response, int UserId);

    }
}
