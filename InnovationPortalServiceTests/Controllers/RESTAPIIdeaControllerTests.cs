using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils;
using IdeaDatabase.Utils.Interface;
using InnovationPortalService.Controllers;
using InnovationPortalService.Idea;
using InnovationPortalService.Requests;
using InnovationPortalService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responses;
using SettingsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovationPortalServiceTests.Controllers
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RESTAPIIdeaControllerTests
    {
        private Mock<IStatusUtils> statusMock;
        private Mock<ISubmitIdeaUtil> submitIdeaMock;
        private Mock<IIdeaUtil> ideaMock;
        private Mock<IQueryUtils> queryUtilMock;
        private Mock<ILogUtils> logMock;

        [TestInitialize]
        public void Initialize()
        {
            ideaMock = new Mock<IIdeaUtil>();
            DependencyInjector.Register(ideaMock.Object).As<IIdeaUtil>();

            statusMock = new Mock<IStatusUtils>();
            DependencyInjector.Register(statusMock.Object).As<IStatusUtils>();

            submitIdeaMock = new Mock<ISubmitIdeaUtil>();
            DependencyInjector.Register(submitIdeaMock.Object).As<ISubmitIdeaUtil>();

            queryUtilMock = new Mock<IQueryUtils>();
            DependencyInjector.Register(queryUtilMock.Object).As<IQueryUtils>();

            logMock = new Mock<ILogUtils>();
            DependencyInjector.Register(logMock.Object).As<ILogUtils>();
        }

        [TestMethod()]
        public void InsertIdeaTest_OK()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };

            RestAPISubmitIdeaResponse response = new RestAPISubmitIdeaResponse()
            {
                ErrorList = new HashSet<Fault>() { new Fault("Submit Idea", "", "") }
            };

            RestAPISubmitIdeaRequest request = new RestAPISubmitIdeaRequest()
            {
                Title="test",
                BusinessImpact= "test",
                CategoryId = 1,
                ChallengeId = 1,
                Description = "test",
                IdeaContributors = "test",
                IsDraft = false,
                Solution = "test"
            };

            ideaMock.Setup(x => x.SubmitIdeas(It.IsAny<RestAPISubmitIdeaResponse>(), It.IsAny<RestAPISubmitIdeaRequest>(), It.IsAny<int>())).Returns(response);
            response = apiController.InsertIdea(request);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        //[TestMethod()]
        //public void AddIdeaAttachmentsTest()
        //{
        //    RESTAPIIdeaController apiController = new RESTAPIIdeaController()
        //    {
        //        DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
        //        {
        //            DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
        //        }
        //    };

        //    InMemoryMultipartFormDataStreamProvider provider = new InMemoryMultipartFormDataStreamProvider(IdeaDatabase.Enums.MimeRequestType.Idea);
        //    SubmitIdeaAttachmentResponse submitIdeaAttachmentResponse = new SubmitIdeaAttachmentResponse();
        //    Task<SubmitIdeaAttachmentResponse> response;


        //    ideaMock.Setup(x => x.SubmitIdeaAttachment(It.IsAny<InMemoryMultipartFormDataStreamProvider>(), It.IsAny<int>())).Returns(submitIdeaAttachmentResponse);
        //    response = apiController.AddIdeaAttachments();
        //}

        [TestMethod]
        public void UpdateSensitiveTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };

            RestAPIUpdateSensitiveResponse response = new RestAPIUpdateSensitiveResponse();
            int IdeaId = 1;
            bool isSensitive = false;

            logMock.Setup(x => x.InsertIdeaLog(It.IsAny<ResponseBase>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            submitIdeaMock.Setup(x => x.UpdateSensitive(It.IsAny<ResponseBase>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()));
            response = apiController.UpdateSensitive(IdeaId, isSensitive);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod]
        public void UpdateIdeaStatusTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };
            RestAPIAddIdeaStateResponse response = new RestAPIAddIdeaStateResponse();
            int IdeaId = 0;
            submitIdeaMock.Setup(x => x.InsertIdeaStatus(It.IsAny< RestAPIAddIdeaStateResponse>(), It.IsAny<int>(), It.IsAny<int>()));
            submitIdeaMock.Setup(x => x.GetAllStakeholdersEmailAdd(It.IsAny<int>())).Returns(new List<IdeaEmailToDetails>() { new IdeaEmailToDetails() { EmailAddress = "test@gmail.com", IdeaState = 1 } }); 
            queryUtilMock.Setup(x => x.GetAllStakeholdersEmailAdd(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>())).Returns(new List<IdeaEmailToDetails>() { new IdeaEmailToDetails() { EmailAddress = "test@gmail.com", IdeaState = 1 }});

            response = apiController.UpdateIdeaStatus(IdeaId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod()]
        public void AddIdeaCommentTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };
            RestAPIAddUserCommentResponse response = new RestAPIAddUserCommentResponse();
            RestAPIAddUserCommentRequest request = new RestAPIAddUserCommentRequest()
            {
                IdeaID = 1,
                CommentDescription = "test"
            };
            Assert.IsNotNull(apiController.AddIdeaComment(request));
            submitIdeaMock.Verify(x => x.InsertIdeaComment(It.IsAny<ResponseBase>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        [TestMethod()]
        public void GetIdeaCommentsTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };
            RestAPIGetUserCommentsResponse response = new RestAPIGetUserCommentsResponse();
            int IdeaId = 1;

            Assert.IsNotNull(apiController.GetIdeaComments(IdeaId));
            submitIdeaMock.Verify(x => x.GetUserComments(It.IsAny< RestAPIGetUserCommentsResponse>(), It.IsAny<int>()));
        }

        [TestMethod()]
        public void DeleteIdeaCommentTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };
            RestAPIDeleteIdeaResponse response = new RestAPIDeleteIdeaResponse();
            int IdeaId = 1;

            Assert.IsNotNull(apiController.DeleteIdeaComment(IdeaId));
            submitIdeaMock.Verify(x => x.DeleteIdeaComment(It.IsAny<RestAPIDeleteIdeaResponse>(), It.IsAny<int>()));
        }

        [TestMethod()]
        public void GetIdeaListTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };
            Assert.IsNotNull(apiController.GetIdeaList());
            submitIdeaMock.Verify(t => t.GetIdeas(It.IsAny< RestAPIGetUserIdeaResponse>(), It.IsAny<int>(), It.IsAny<bool>()));
        }

        [TestMethod()]
        public void GetDetailsTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };

            int IdeaId = 1;

            Assert.IsNotNull(apiController.GetDetails(IdeaId));
            submitIdeaMock.Verify(t => t.GetIdeasDetails(It.IsAny<RESTGetUserIdeaDetailsResponse>(), It.IsAny<int>()));
        }

        [TestMethod()]
        public void GetIdeaReviewsListTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };

            Assert.IsNotNull(apiController.GetIdeaReviewsList());
            submitIdeaMock.Verify(t => t.GetIdeaReviewsList(It.IsAny<RestAPIGetUserIdeaStatusResponse>(), It.IsAny<int>()));
        }

        [TestMethod()]
        public void GetIdeaSponsorsListTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };

            Assert.IsNotNull(apiController.GetIdeaSponsorsList());
            submitIdeaMock.Verify(t => t.GetIdeaSponsorsList(It.IsAny<RestAPIGetUserIdeaStatusResponse>(), It.IsAny<int>()));
        }

        [TestMethod()]
        public void GetPublicListTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };

            int CategoryId = 0;
            bool Sort = false;

            Assert.IsNotNull(apiController.GetPublicList(CategoryId, Sort));
            submitIdeaMock.Verify(t => t.GetPublicIdeas(It.IsAny<RestAPIGetUserIdeaResponse>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()));
        }

        [TestMethod()]
        public void AddIdeaCommentReplyTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };

            RestAPIAddCommentReplyRequest request = new RestAPIAddCommentReplyRequest()
            {
                IdeaCommentID = 1,
                DiscussionDescription = "test"
            };

            Assert.IsNotNull(apiController.AddIdeaCommentReply(request));
            submitIdeaMock.Verify(t => t.InsertCommentReply(It.IsAny<RestAPIAddCommentReplyResponse>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
        }

        [TestMethod()]
        public void SearchIdeaTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };

            string SearchText = "test";
            Assert.IsNotNull(apiController.SearchIdea(SearchText));
            submitIdeaMock.Verify(t => t.SearchIdea(It.IsAny<RestAPISearchIdeaResponse>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        [TestMethod()]
        public void UpdateDetailsTest()
        {
            RESTAPIIdeaController apiController = new RESTAPIIdeaController()
            {
                DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
                {
                    DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
                }
            };
            RestAPIUpdateIdeaRequest request = new RestAPIUpdateIdeaRequest()
            {
               Title = "test"
            };

            int IdeaId = 1;
            Assert.IsNotNull(apiController.UpdateDetails(request, IdeaId));
            submitIdeaMock.Verify(t => t.UpdateIdea(It.IsAny<RestAPIUpdateIdeaResponse>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<RestAPIUpdateIdeaRequest>()));
        }

        //[TestMethod()]
        //public void SendEmailTest()
        //{
        //    RESTAPIIdeaController apiController = new RESTAPIIdeaController()
        //    {
        //        DeviceWithDbContext = new RESTAPIDeviceWithDbContext()
        //        {
        //            DbContext = new IdeaDatabase.DataContext.IdeaDatabaseDataContext()
        //        }
        //    };

        //    string EmailAddress = "testemail@gmail.com";
        //    Assert.IsNotNull(apiController.SendEmail(EmailAddress));
        //    emailMock.Verify(t => t.GetEmailTemplate(It.IsAny<string>()));
        //}


        [ClassCleanup()]
        public static void ClassCleanup()
        {
            DependencyInjector.Clear();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            ideaMock.Reset();
            submitIdeaMock.Reset();
            statusMock.Reset();
            queryUtilMock.Reset();
        }
    }
}
