using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovationPortalServiceTests.Utils
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SubmitIdeaUtilTests
    {
        private Mock<ISubmitIdeaUtil> submitIdeaMock;
        private Mock<IIdeaDatabaseDataContext> databaseMock;
        private Mock<IQueryUtils> queryUtilMock;
        private Mock<IIdeaUtils> ideaMock;

        SubmitIdeaUtil submitIdeaUtil = new SubmitIdeaUtil();

        [TestInitialize]
        public void Initialize()
        {
            submitIdeaMock = new Mock<ISubmitIdeaUtil>();
            DependencyInjector.Register(submitIdeaMock.Object).As<ISubmitIdeaUtil>();

            databaseMock = new Mock<IIdeaDatabaseDataContext>();
            DependencyInjector.Register(databaseMock.Object).As<IIdeaDatabaseDataContext>();

            queryUtilMock = new Mock<IQueryUtils>();
            DependencyInjector.Register(queryUtilMock.Object).As<IQueryUtils>();

            ideaMock = new Mock<IIdeaUtils>();
            DependencyInjector.Register(ideaMock.Object).As<IIdeaUtils>();

        }

        [TestMethod()]
        public void UpdateSensitiveTest()
        {

            RestAPIUpdateSensitiveResponse response = new RestAPIUpdateSensitiveResponse();
            Idea idea = new Idea(); 
            int UserId = 1;
            int IdeaId = 1;
            bool isSensitive = true;

            queryUtilMock.Setup(x => x.GetIdeaById(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(new Idea() { IdeaId = 1, Title = "test" });
            submitIdeaMock.Setup(x => x.UpdateSensitive(It.IsAny<RestAPIUpdateSensitiveResponse>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(idea);
            idea = submitIdeaUtil.UpdateSensitive(response, UserId, IdeaId, isSensitive);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void UpdateIdeaStatusTest()
        {
            RestAPIAddIdeaStateResponse response = new RestAPIAddIdeaStateResponse();
            int UserId = 1;
            int IdeaId = 1;

            queryUtilMock.Setup(x => x.GetIdeaStatusByIdeaId(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(new IdeaStatusLog() { IdeaId = 1, IdeaState = 1, IsActive = true });
            queryUtilMock.Setup(x => x.GetUserRoleMappings(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(new List<RoleMapping>() { new RoleMapping(){ UserId = 1, RoleId = 3}});
            queryUtilMock.Setup(x => x.GetAllRoleMappings(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(new Role() { RoleId = 1, RoleName = "SPONSOR", RoleMappings = new List<RoleMapping>() { new RoleMapping() { UserId = 1, RoleId = 3 }}});
            queryUtilMock.Setup(x => x.GetUserRoleMappingByRoleId(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(new RoleMapping() { UserId = 1, RoleId = 3 });

            submitIdeaMock.Setup(x => x.InsertIdeaStatus(It.IsAny<RestAPIAddIdeaStateResponse>(), It.IsAny<int>(), It.IsAny<int>())).Returns(response);
            response = submitIdeaUtil.InsertIdeaStatus(response, IdeaId, UserId);

            Assert.IsTrue(response.ErrorList.Count > 0);
        }

        [TestMethod()]
        public void InsertIdeaCommentTest()
         {
            RestAPIAddUserCommentResponse response = new RestAPIAddUserCommentResponse();
            IdeaComment ideaComment = new IdeaComment();
            int UserId = 1;
            RestAPIAddUserCommentRequest request = new RestAPIAddUserCommentRequest()
            {
                IdeaID = 1,
                CommentDescription = "test" 
            };

            queryUtilMock.Setup(x => x.GetIdeaById(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(new Idea() { IdeaId = 1, Title = "test" });
            submitIdeaMock.Setup(x => x.InsertIdeaComment(It.IsAny<RestAPIAddUserCommentResponse>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(ideaComment);
            ideaComment = submitIdeaUtil.InsertIdeaComment(response, request.IdeaID, request.CommentDescription, UserId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void GetIdeaCommentsTest()
        {
            RestAPIGetUserCommentsResponse response = new RestAPIGetUserCommentsResponse();
            IdeaComment ideaComment = new IdeaComment();
            int IdeaId = 1;
         
            queryUtilMock.Setup(x => x.GetIdeaComment(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(new List<IdeaComment>() { new IdeaComment() { IdeaCommentId = 1, CommentDescription ="test", User = new User() { FirstName = "Sanjay"}}});
            submitIdeaMock.Setup(x => x.GetUserComments(It.IsAny<RestAPIGetUserCommentsResponse>(), It.IsAny<int>()));
            submitIdeaUtil.GetUserComments(response, IdeaId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void DeleteIdeaCommentsTest()
        {
            RestAPIDeleteIdeaResponse response = new RestAPIDeleteIdeaResponse();
            IdeaComment ideaComment = new IdeaComment();
            int IdeaCommentId = 1;

            queryUtilMock.Setup(x => x.GetIdeaCommentById(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(ideaComment);
            submitIdeaMock.Setup(x => x.DeleteIdeaComment(It.IsAny<RestAPIDeleteIdeaResponse>(), It.IsAny<int>()));
            submitIdeaUtil.DeleteIdeaComment(response, IdeaCommentId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void GetIdeaListTest()
        {
            RestAPIGetUserIdeaResponse response = new RestAPIGetUserIdeaResponse();
            List<Idea> ideaList = new List<Idea>();
            int UserId = 1;
            bool IsDraft = false;

            queryUtilMock.Setup(x => x.GetIdeas(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(ideaList);
            submitIdeaMock.Setup(x => x.GetIdeas(It.IsAny< RestAPIGetUserIdeaResponse>(), It.IsAny<int>(), It.IsAny<bool>()));
            submitIdeaUtil.GetIdeas(response, UserId, IsDraft);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void GetIdeasDetailsTest()
        {
            Idea idea = new Idea();
            RESTGetUserIdeaDetailsResponse response = new RESTGetUserIdeaDetailsResponse();
            int IdeaId = 1;

            queryUtilMock.Setup(x => x.GetIdeaById(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>()));
            submitIdeaMock.Setup(x => x.GetIdeasDetails(It.IsAny<RESTGetUserIdeaDetailsResponse>(), It.IsAny<int>()));
            submitIdeaUtil.GetIdeasDetails(response, IdeaId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void GetIdeaReviewsListTest()
        {
            RestAPIGetUserIdeaStatusResponse response = new RestAPIGetUserIdeaStatusResponse();
            List<Idea> ideaList = new List<Idea>();

            int UserId = 1;

            queryUtilMock.Setup(x => x.GetIdeasForReview(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(ideaList);
            submitIdeaMock.Setup(x => x.GetIdeaReviewsList(It.IsAny<RestAPIGetUserIdeaStatusResponse>(), It.IsAny<int>()));
            submitIdeaUtil.GetIdeaReviewsList(response, UserId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void GetIdeaSponsorsListTest()
        {
            RestAPIGetUserIdeaStatusResponse response = new RestAPIGetUserIdeaStatusResponse();
            List<Idea> ideaList = new List<Idea>();

            int UserId = 1;

            queryUtilMock.Setup(x => x.GetIdeasForSponsors(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(ideaList);
            submitIdeaMock.Setup(x => x.GetIdeaSponsorsList(It.IsAny<RestAPIGetUserIdeaStatusResponse>(), It.IsAny<int>()));
            submitIdeaUtil.GetIdeaSponsorsList(response, UserId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void GetPublicIdeasTest()
        {
            RestAPIGetUserIdeaResponse response = new RestAPIGetUserIdeaResponse();
            List<Idea> ideaList = new List<Idea>();

            int UserId = 1;
            int CategoryId = 0;
            bool Sort = false;

            queryUtilMock.Setup(x => x.GetPublicIdeas(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<int>())).Returns(ideaList);
            queryUtilMock.Setup(x => x.GetSortedPublicIdeas(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<int>())).Returns(ideaList);
            submitIdeaMock.Setup(x => x.GetPublicIdeas(It.IsAny<RestAPIGetUserIdeaResponse>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()));
            submitIdeaUtil.GetPublicIdeas(response, UserId, CategoryId, Sort); 

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void InsertCommentReplyTest()
        {
            RestAPIAddCommentReplyResponse response = new RestAPIAddCommentReplyResponse();
            RestAPIAddCommentReplyRequest request = new RestAPIAddCommentReplyRequest()
            {
                IdeaCommentID = 1,
                DiscussionDescription = "test"
            };
            IdeaComment ideaComment = new IdeaComment();
            IdeaCommentDiscussion ideaCommentDiscussion = new IdeaCommentDiscussion();
            int UserId = 1;
            int IdeaCommentId = 1;
            string DiscussionDescription = "test";

            queryUtilMock.Setup(x => x.GetIdeaCommentById(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(ideaComment);
            queryUtilMock.Setup(x => x.AddIdeaCommentDiscussion(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<IdeaCommentDiscussion>()));
            submitIdeaMock.Setup(x => x.InsertCommentReply(It.IsAny<RestAPIAddCommentReplyResponse>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(ideaCommentDiscussion);
            submitIdeaUtil.InsertCommentReply(response, IdeaCommentId, UserId, DiscussionDescription);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void SearchIdeaTest()
        {
            RestAPISearchIdeaResponse response = new RestAPISearchIdeaResponse();
            List<Idea> ideaList = new List<Idea>();
            int UserId = 1;
            string Title = "test";

            queryUtilMock.Setup(x => x.SearchIdea(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>())).Returns(ideaList);
            submitIdeaMock.Setup(x => x.SearchIdea(It.IsAny<RestAPISearchIdeaResponse>(), It.IsAny<string>(), It.IsAny<int>()));
            submitIdeaUtil.SearchIdea(response, Title, UserId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void UpdateIdeaTest()
        {
            RestAPIUpdateIdeaResponse response = new RestAPIUpdateIdeaResponse();
            RestAPIUpdateIdeaRequest request = new RestAPIUpdateIdeaRequest()
            {
                Title = "test",
                Description = "demo"
            };
            Idea idea = new Idea();
            int UserId = 1;
            int IdeaId = 1;

            queryUtilMock.Setup(x => x.GetIdeaById(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(idea);
            submitIdeaMock.Setup(x => x.UpdateIdea(It.IsAny<RestAPIUpdateIdeaResponse>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<RestAPIUpdateIdeaRequest>()));
            submitIdeaUtil.UpdateIdea(response, UserId, IdeaId, request);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }



        [ClassCleanup()]
        public static void ClassCleanup()
        {
            DependencyInjector.Clear();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            submitIdeaMock.Reset();
            queryUtilMock.Reset();
        }

    }




}
