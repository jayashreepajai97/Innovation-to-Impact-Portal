using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using InnovationPortalService.Requests;
using InnovationPortalService.Utils;
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
    public class IdeaUtilsTests
    {
        private Mock<ISubmitIdeaUtil> submitIdeaMock;
        private Mock<IQueryUtils> iqueryMock;
        private Mock<IIdeaUtil> ideaMock;

        private static Mock<IIdeaDatabaseDataContext> databaseMock;


        [TestInitialize]
        public void Initialize()
        {
            submitIdeaMock = new Mock<ISubmitIdeaUtil>();
            DependencyInjector.Register(submitIdeaMock.Object).As<ISubmitIdeaUtil>();

            databaseMock = new Mock<IIdeaDatabaseDataContext>();
            DependencyInjector.Register(databaseMock.Object).As<IIdeaDatabaseDataContext>();

            ideaMock = new Mock<IIdeaUtil>();
            DependencyInjector.Register(ideaMock.Object).As<IIdeaUtil>();

        }

        [TestMethod()]
        public void SubmitIdeasTest()
        {
            IdeaUtil ideaUtil = new IdeaUtil();
            int UserId = 1;
            RestAPISubmitIdeaResponse response = new RestAPISubmitIdeaResponse();
            RestAPISubmitIdeaRequest request = new RestAPISubmitIdeaRequest()
            {
                Title = "test",
                BusinessImpact = "test",
                CategoryId = 1,
                ChallengeId = 54,
                Description = "test",
                IdeaContributors = "test",
                IsDraft = true,
                Solution = "test"
            };

            ideaMock.Setup(x => x.SubmitIdeas(It.IsAny<RestAPISubmitIdeaResponse>(), It.IsAny<RestAPISubmitIdeaRequest>(), It.IsAny<int>())).Returns(response);
            response = ideaUtil.SubmitIdeas(response, request, UserId);

            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        //[TestMethod()]
        //public void SubmitIdeaAttachmentTest()
        //{
        //    SubmitIdeaAttachmentResponse response = new SubmitIdeaAttachmentResponse();
        //    InMemoryMultipartFormDataStreamProvider provider = new InMemoryMultipartFormDataStreamProvider(IdeaDatabase.Enums.MimeRequestType.Idea);
        //    IdeaUtil ideaUtil = new IdeaUtil();
        //    int UserId = 1;

        //    ideaMock.Setup(x => x.SubmitIdeaAttachment(It.IsAny<InMemoryMultipartFormDataStreamProvider>(), It.IsAny<int>())).Returns(response);
        //    response = ideaUtil.SubmitIdeaAttachment(provider, UserId);

        //    Assert.IsTrue(response.ErrorList.Count == 0);

        //}


        [ClassCleanup()]
        public static void ClassCleanup()
        {
            DependencyInjector.Clear();
        }

        [TestCleanup()]
        public void TestCleanup()
        {

            submitIdeaMock.Reset();
            ideaMock.Reset();
        }


    }
}
