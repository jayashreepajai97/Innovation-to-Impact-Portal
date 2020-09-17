using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responses;
using System;

namespace IdeaDatabase.Utils.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class IsacUserUtilsTests
    {
        Mock<IQueryUtils> queryUtilsMock;
        Mock<IIdeaDatabaseDataContext> databaseMock;
        Mock<IUserUtils> isacUserUtilsMock = null;
        RequestFindOrInsertHPIDProfile requestFindOrInsertHPIDProfile = null;

        [TestInitialize]
        public void Initialize()
        {
            queryUtilsMock = new Mock<IQueryUtils>();
            DependencyInjector.Register(queryUtilsMock.Object).As<IQueryUtils>();
            databaseMock = new Mock<IIdeaDatabaseDataContext>();
            DependencyInjector.Register(databaseMock.Object).As<IIdeaDatabaseDataContext>();

            isacUserUtilsMock = new Mock<IUserUtils>();
            DependencyInjector.Register(isacUserUtilsMock.Object).As<IUserUtils>();

            requestFindOrInsertHPIDProfile = new RequestFindOrInsertHPIDProfile();
            requestFindOrInsertHPIDProfile.Locale = "en-US";
            requestFindOrInsertHPIDProfile.HPIDprofileId = "hpidProfile";
            requestFindOrInsertHPIDProfile.HPPprofileId = "HPPprofileId";
            requestFindOrInsertHPIDProfile.tokenDetails = new TokenDetails();
            requestFindOrInsertHPIDProfile.clientId = "clientID";
            requestFindOrInsertHPIDProfile.apiRetainOldValues = APIMethods.POSTGetProfile;
        }

        [TestMethod()]
        public void FindOrInsertHPIDProfileTest_HPPProfileFound()
        {
            UserUtils isacUtils = new UserUtils();

            User iui = null;
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(),It.IsAny<string>())).Returns(iui);

            User iup = new User() { ProfileId = "profileId"};
            queryUtilsMock.Setup(x => x.GetProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iup);
            

            ResponseBase response = new ResponseBase();
            User expectedProfile = isacUtils.FindOrInsertHPIDProfile(response, "hpidProfile", "hppProfile");
            
            Assert.IsTrue(expectedProfile == iup);
            databaseMock.Verify(x => x.SubmitChanges(), Times.Once);
        }
        
        [TestMethod()]
        public void FindOrInsertHPIDProfileTest_HPIDProfileFound()
        {
            UserUtils isacUtils = new UserUtils();

            User iui = new User() { HPIDprofileId = "profileId" , PrimaryUse = PrimaryUseType.Item002.ToString() };
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iui);

            User iup = null;
            queryUtilsMock.Setup(x => x.GetProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iup);


            ResponseBase response = new ResponseBase();
            User expectedProfile = isacUtils.FindOrInsertHPIDProfile(response, "hpidProfile", "hppProfile");

            Assert.IsTrue(expectedProfile == iui);
            databaseMock.Verify(x => x.SubmitChanges(), Times.Never);
        }

        [TestMethod()]
        public void FindOrInsertHPIDProfileTest_HPIDProfileFound_UpdateLocale()
        {
            UserUtils isacUtils = new UserUtils();

            User iui = new User() { HPIDprofileId = "profileId", PrimaryUse = PrimaryUseType.Item002.ToString() };
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iui);

            User iup = null;
            queryUtilsMock.Setup(x => x.GetProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iup);

            bool IsNewCustomer = false;
            ResponseBase response = new ResponseBase();
            User expectedProfile = isacUtils.FindOrInsertHPIDProfile(response, requestFindOrInsertHPIDProfile, out IsNewCustomer);
            Assert.IsTrue(expectedProfile == iui);
            databaseMock.Verify(x => x.SubmitChanges(), Times.Exactly(1));
        }

        [TestMethod()]
        public void FindOrInsertHPIDProfileTest_HPIDProfileFound_NoUpdateForSameLocale()
        {
            UserUtils isacUtils = new UserUtils();

            User iui = new User() { HPIDprofileId = "profileId", PrimaryUse = PrimaryUseType.Item002.ToString(), Locale = "en_IN" };
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iui);

            User iup = null;
            queryUtilsMock.Setup(x => x.GetProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iup);

            bool IsNewCustomer = false;
            ResponseBase response = new ResponseBase();
            User expectedProfile = isacUtils.FindOrInsertHPIDProfile(response, requestFindOrInsertHPIDProfile, out IsNewCustomer);
            Assert.IsTrue(expectedProfile == iui);
            databaseMock.Verify(x => x.SubmitChanges(), Times.Once);
        }

        [TestMethod()]
        public void FindOrInsertHPIDProfileTest_HPIDProfileFound_NullUpdateForSameLocale()
        {
            UserUtils isacUtils = new UserUtils();
           

            User iui = new User() { HPIDprofileId = "profileId", PrimaryUse = PrimaryUseType.Item002.ToString(), Locale = null };
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iui);

            User iup = null;
            queryUtilsMock.Setup(x => x.GetProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iup);

            bool IsNewCustomer = false;
            ResponseBase response = new ResponseBase();
            User expectedProfile = isacUtils.FindOrInsertHPIDProfile(response, requestFindOrInsertHPIDProfile,  out IsNewCustomer);

            Assert.IsTrue(expectedProfile == iui);
            databaseMock.Verify(x => x.SubmitChanges(), Times.Exactly(1));
        }

        [TestMethod()]
        public void FindOrInsertHPIDProfileTest_HPIDProfileFound_NoneApiMethodCallForLocale()
        {
            UserUtils isacUtils = new UserUtils();

            User iui = new User() { HPIDprofileId = "profileId", PrimaryUse = PrimaryUseType.Item002.ToString(), Locale = null,  };
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iui);

            User iup = null;
            queryUtilsMock.Setup(x => x.GetProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iup);

            bool IsNewCustomer = false;
            ResponseBase response = new ResponseBase();
            User expectedProfile = isacUtils.FindOrInsertHPIDProfile(response, requestFindOrInsertHPIDProfile, out IsNewCustomer);
            Assert.IsTrue(expectedProfile == iui);
            databaseMock.Verify(x => x.SubmitChanges(), Times.Once);
        }

        [TestMethod()]
        public void FindOrInsertHPIDProfileTest_CannotInsertNewIsacUser()
        {
            UserUtils isacUtils = new UserUtils();

            User iu = null;
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iu);
            queryUtilsMock.Setup(x => x.GetProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iu);
            queryUtilsMock.Setup(x => x.AddUser(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<User>()));

            ResponseBase response = new ResponseBase();
            User expectedProfile = isacUtils.FindOrInsertHPIDProfile(response, null, null);

            Assert.IsNull(expectedProfile);
            Assert.IsTrue(response.ErrorList.Contains(Faults.ServerIsBusy));

            queryUtilsMock.Verify(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod()]
        public void FindOrInsertHPIDProfileTest_HPIDProfileFound_MissingPrimaryUse()
        {
            UserUtils isacUtils = new UserUtils();

            User iui = new User() { HPIDprofileId = "profileId"};
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iui);

            User iup = null;
            queryUtilsMock.Setup(x => x.GetProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iup);


            ResponseBase response = new ResponseBase();
            User expectedPrifile = isacUtils.FindOrInsertHPIDProfile(response, "hpidProfile", "hppProfile");

            Assert.IsTrue(expectedPrifile == iui);
            databaseMock.Verify(x => x.SubmitChanges(), Times.Once);
        }

        [TestMethod()]
        public void SaveHPIDProfileTest_HPPProfileNotFound()
        {
            UserUtils isacUtils = new UserUtils();

            User iui = null;
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iui);

            
            ResponseBase response = new ResponseBase();
            isacUtils.SaveHPIDProfile(response, new User());

            databaseMock.Verify(x => x.SubmitChanges(), Times.Never);
        }

        [TestMethod()]
        public void SaveHPIDProfileTest_EmptyData()
        {
            UserUtils isacUtils = new UserUtils();
            
            ResponseBase response = new ResponseBase();
            isacUtils.SaveHPIDProfile(response, null);
            queryUtilsMock.Verify(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>()), Times.Never);
            databaseMock.Verify(x => x.SubmitChanges(), Times.Never);
        }

        [TestMethod()]
        public void SaveHPIDProfileTest_Success()
        {
            UserUtils isacUtils = new UserUtils();

            User iui = new User();
            queryUtilsMock.Setup(x => x.GetHPIDProfile(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<string>())).Returns(iui);


            ResponseBase response = new ResponseBase();
            isacUtils.SaveHPIDProfile(response, new User());

            databaseMock.Verify(x => x.SubmitChanges(), Times.Once);
        }

        [TestMethod]
        public void GetRefreshTokenTest_InvalidCredentials()
        {            
            TokenDetails accessToken = new TokenDetails() { AccessToken = "dasfdasfdasfadsfasdfdasfdasfds" };

            UserAuthentication appAuth = null;
            queryUtilsMock.Setup(q => q.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(),
                It.IsAny<String>(), It.IsAny<String>())).Returns(appAuth);

            Assert.IsNull(new UserUtils().GetRefreshToken("",accessToken));

            queryUtilsMock.Verify(q => q.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(),
                It.IsAny<String>(), It.IsAny<String>()), Times.Once);

            queryUtilsMock.Verify(q => q.GetUser(It.IsAny<IIdeaDatabaseDataContext>(),It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void GetRefreshTokenTest_Success()
        {            
            TokenDetails accessToken = new TokenDetails() { AccessToken = "dasfdasfdasfadsfasdfdasfdasfds" };

            UserAuthentication appAuth = new UserAuthentication() { UserId = 123456};
            queryUtilsMock.Setup(q => q.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(),
                It.IsAny<String>(), It.IsAny<String>())).Returns(appAuth);


            User profile = new User()
            {
                RefreshToken = "refreshToken",
                CompanyName = "companyName",
                UserId = 123456                
            };
            queryUtilsMock.Setup(q => q.GetUser(It.IsAny<IIdeaDatabaseDataContext>(),It.IsAny<int>())).Returns(profile);

            User expectedResponse = new UserUtils().GetRefreshToken("", accessToken);

            Assert.AreEqual(expectedResponse, profile);

            queryUtilsMock.Verify(q => q.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(),
                It.IsAny<String>(), It.IsAny<String>()), Times.Once);

            queryUtilsMock.Verify(q => q.GetUser(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>()), Times.Once);
        }
    }
}