using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using IdeaDatabase.DataContext;
using InnovationPortalService.Responses;
using Moq;
using Hpcs.DependencyInjector;
using System.Web.Services.Protocols;
//using InnovationPortalService.HPPService;
using System.Xml;
using Responses;
using IdeaDatabase.Utils;
using System.ServiceModel;
using IdeaDatabase.Enums;
using System.Collections.Concurrent;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using Credentials;
using InnovationPortalService.Utils;
using InnovationPortalService;

namespace InnovationPortalServiceTests.Utils.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CustomerUtilsTests
    {
 
        private static Mock<IIdeaDatabaseDataContext> databaseMock;
        private static Mock<IdeaDatabase.Utils.IQueryUtils> queryUtilsMock;
       // private static Mock<DocLiteral> hPPMock;
        private static Mock<IUserUtils> isacUserUtilsMock;
     
        private static Mock<ICustomerUtils> custUtilsMock;
        private static Mock<ICommonHelper> helperMock;
        private static Mock<IAuthenticationUtils> authMock;
      //  private static Mock<ICustProdRegistrationsserviceagent> svcMock;
 

        [ClassInitialize()]
        public static void ClassInitialize(TestContext context)
        {
            
            databaseMock = new Mock<IIdeaDatabaseDataContext>();
            queryUtilsMock = new Mock<IdeaDatabase.Utils.IQueryUtils>();
           // svcMock = new Mock<ICustProdRegistrationsserviceagent>();
            helperMock = new Mock<ICommonHelper>();
            //hPPMock = new Mock<DocLiteral>();
            isacUserUtilsMock = new Mock<IUserUtils>();
            authMock = new Mock<IAuthenticationUtils>();
            
            custUtilsMock = new Mock<ICustomerUtils>();
           

       
            DependencyInjector.Register(databaseMock.Object).As<IIdeaDatabaseDataContext>();
            DependencyInjector.Register(queryUtilsMock.Object).As<IdeaDatabase.Utils.IQueryUtils>();
            //DependencyInjector.Register(svcMock.Object).As<ICustProdRegistrationsserviceagent>();
            DependencyInjector.Register(helperMock.Object).As<ICommonHelper>();
            //DependencyInjector.Register(hPPMock.Object).As<DocLiteral>();
            DependencyInjector.Register(isacUserUtilsMock.Object).As<IUserUtils>();
            DependencyInjector.Register(authMock.Object).As<IAuthenticationUtils>();
        
            DependencyInjector.Register(custUtilsMock.Object).As<ICustomerUtils>();
         

          

        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            DependencyInjector.Clear();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            
            databaseMock.Reset();
            queryUtilsMock.Reset();
         //   svcMock.Reset();
            helperMock.Reset();
           // hPPMock.Reset();
            isacUserUtilsMock.Reset();
            authMock.Reset();
     
            custUtilsMock.Reset();
            
        }

        //[TestMethod()]
        //public void GetCustomerProfileTestHPPException()
        //{
        //    DependencyInjector.Register(new CommonHelper()).As<ICommonHelper>();

        //    hPPMock.Setup(x => x.login(It.IsAny<loginRequest>())).Throws(new Exception());

        //    CustomerUtils custUtils = new CustomerUtils();
        //    GetProfileResponse response = custUtils.GetCustomerProfile(new UserAuthenticationInterchange(), It.IsAny<bool>());

        //    Assert.IsTrue(response.ErrorList.Count == 1);
        //    Assert.IsTrue(response.ErrorList.First().DebugStatusText.CompareTo(Faults.HPPInternalError.DebugStatusText) == 0);

        //    DependencyInjector.Register(helperMock.Object).As<ICommonHelper>();
        //}

        //[TestMethod()]
        //public void GetCustomerProfileTestHPPFaultException()
        //{
        //    genericFault genFault = new genericFault();
        //    int ruleNumber = 237;
        //    FaultException<genericFault> ex1 = new FaultException<genericFault>(genFault);
        //    genFault.fault = new genericFaultType[1];
        //    genFault.fault[0] = new genericFaultType()
        //    {
        //        ruleNumber = ruleNumber /* different number */
        //    };

        //    string sessiontoken = "sessionToken";
        //    loginResponse r = new loginResponse();
        //    r.loginResponseElement = new loginResultType() { sessionToken = sessiontoken };
        //    getUserResponse hppData = new getUserResponse();
        //    hppData.getUserResponseElement = new getUserResultType()
        //    {
        //        profileIdentity = new profileIdentityType()
        //    };
        //    hPPMock.Setup(x => x.login(It.IsAny<loginRequest>())).Returns(r);

        //    helperMock.Setup(x => x.GetUserProfile(It.IsAny<DocLiteral>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).Throws(ex1);

        //    CustomerUtils custUtils = new CustomerUtils();
        //    GetProfileResponse response = custUtils.GetCustomerProfile(new UserAuthenticationInterchange(), It.IsAny<bool>());

        //    Assert.IsTrue(response.ErrorList.Count == 1);
        //    Assert.AreEqual(response.ErrorList.First(), Faults.HPPInternalError);
        //}

        //[TestMethod()]
        //public void GetCustomerProfileTestInsertProfileFailes()
        //{
        //    string sessiontoken = "sessionToken";
        //    loginResponse r = new loginResponse();
        //    r.loginResponseElement = new loginResultType() { sessionToken = sessiontoken };
        //    getUserResponse hppData = new getUserResponse();
        //    hppData.getUserResponseElement = new getUserResultType()
        //    {
        //        profileIdentity = new profileIdentityType()
        //    };
        //    hPPMock.Setup(x => x.login(It.IsAny<loginRequest>())).Returns(r);

        //    helperMock.Setup(x => x.GetUserProfile(It.IsAny<DocLiteral>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).Returns(hppData);

        //    isacUserUtilsMock.Setup(x => x.FindOrInsertProfile(It.IsAny<ResponseBase>(), It.IsAny<string>())).Callback(
        //        ((ResponseBase res, string s) =>
        //        {
        //            res.ErrorList.Add(Faults.GenericError);
        //        }));

        //    CustomerUtils custUtils = new CustomerUtils();
        //    GetProfileResponse response = custUtils.GetCustomerProfile(new UserAuthenticationInterchange(), It.IsAny<bool>());

        //    Assert.IsTrue(response.ErrorList.Count == 1);
        //    Assert.IsTrue(response.ErrorList.First().DebugStatusText.CompareTo(Faults.GenericError.DebugStatusText) == 0);
        //}

        //[TestMethod()]
        //public void GetCustomerProfileTestException()
        //{
        //    string sessiontoken = "sessionToken";
        //    loginResponse r = new loginResponse();
        //    r.loginResponseElement = new loginResultType() { sessionToken = sessiontoken };

        //    hPPMock.Setup(x => x.login(It.IsAny<loginRequest>())).Returns(r);

        //    helperMock.Setup(x => x.GetUserProfile(It.IsAny<DocLiteral>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).Throws(new Exception());

        //    CustomerUtils custUtils = new CustomerUtils();
        //    GetProfileResponse response = custUtils.GetCustomerProfile(null, false);

        //    Assert.IsTrue(response.ErrorList.Count == 1);
        //    Assert.IsTrue(response.ErrorList.First().ReturnCode.CompareTo("GetCustomerProfileFailed") == 0);
        //}

        //[TestMethod()]
        //public void GetCustomerProfileTestOk()
        //{
        //    int custId = 100;
        //    string sessiontoken = "sessionToken";
        //    string FirstName = "firstName", Email = "aaa@aa.com", ContactPrefEmail = "Y", CompanyName = "HP", CountryCode = "US";
        //    loginResponse r = new loginResponse();
        //    r.loginResponseElement = new loginResultType() { sessionToken = sessiontoken };
        //    getUserResponse hppData = new getUserResponse();
        //    hppData.getUserResponseElement = new getUserResultType()
        //    {
        //        profileIdentity = new profileIdentityType(),
        //        profileCore = new profileCoreType() { firstName = FirstName, email = Email, contactPrefEmail = ContactPrefEmail, residentCountryCode = CountryCode, segmentName = "Item002" },
        //        profileExtended = new profileExtendedType() { busCompanyName = CompanyName }

        //    };
        //    hPPMock.Setup(x => x.login(It.IsAny<loginRequest>())).Returns(r);

        //    helperMock.Setup(x => x.GetUserProfile(It.IsAny<DocLiteral>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).Returns(hppData);

        //    User isacUser = new User();
        //    isacUser.UserId = custId;
        //    isacUserUtilsMock.Setup(x => x.FindOrInsertProfile(It.IsAny<ResponseBase>(), It.IsAny<string>())).Returns(isacUser);

        //    CustomerUtils custUtils = new CustomerUtils();
        //    GetProfileResponse response = custUtils.GetCustomerProfile(new UserAuthenticationInterchange(), It.IsAny<bool>());

        //    Assert.IsTrue(response.ErrorList.Count == 0);
        //    Assert.IsTrue(response.Credentials.UserID == custId);
        //    Assert.IsTrue(response.CustomerProfileObject.EmailConsent == ContactPrefEmail);
        //    Assert.IsTrue(response.CustomerProfileObject.CompanyName == CompanyName);
        //    Assert.IsTrue(response.CustomerProfileObject.PrimaryUse == "ItemItem002");
        //    Assert.IsTrue(response.Credentials.SessionToken.CompareTo(sessiontoken) == 0);
        //    Assert.IsTrue(response.CustomerProfileObject.FirstName.CompareTo(FirstName) == 0);
        //    Assert.IsTrue(response.CustomerProfileObject.EmailAddress.CompareTo(Email) == 0);
        //}

         
         
        
       
 

        //[TestMethod()]
        //public void RefreshTokenTestHppException()
        //{
        //    DependencyInjector.Register(new CommonHelper()).As<ICommonHelper>();

        //    hPPMock.Setup(x => x.login(It.IsAny<loginRequest>())).Throws(new Exception());

        //    Mock<CustomerUtils> custUtils = new Mock<CustomerUtils>();
        //    GetProfileResponse response = custUtils.Object.RefreshToken("user_name", "pass_word", "caller_id", 0, false);
        //    Assert.AreEqual(response.ErrorList.Count, 1);
        //    Assert.IsNull(response.Credentials);

        //    DependencyInjector.Register(helperMock.Object).As<ICommonHelper>();
        //}

        //[TestMethod()]
        //public void RefreshTokenTestAllOk()
        //{
        //    string sessionToken = "1234567890";
        //    string caller_id = "caller_id";
        //    int customerId = 12457545;
        //    loginResponse hppResponse = new loginResponse() { loginResponseElement = new loginResultType() { sessionToken = sessionToken } };

        //    hPPMock.Setup(x => x.login(It.IsAny<loginRequest>())).Returns(hppResponse);
        //    Mock<CustomerUtils> custUtils = new Mock<CustomerUtils>();
        //    GetProfileResponse response = custUtils.Object.RefreshToken("", "", caller_id, customerId, It.IsAny<bool>());
        //    Assert.AreEqual(response.ErrorList.Count, 0);
        //    Assert.IsTrue(response.Credentials.SessionToken.CompareTo(sessionToken) == 0);
        //    Assert.IsTrue(response.Credentials.CallerId.CompareTo(caller_id) == 0);
        //    Assert.IsTrue(response.Credentials.UserID == customerId);
        //}

        //[TestMethod()]
        //public void RefreshTokenTestDatabaseException()
        //{
        //    string sessionToken = "1234567890";
        //    string caller_id = "caller_id";
        //    int customerId = 12457545;
        //    loginResponse hppResponse = new loginResponse() { loginResponseElement = new loginResultType() { sessionToken = sessionToken } };

        //    hPPMock.Setup(x => x.login(It.IsAny<loginRequest>())).Returns(hppResponse);

        //    queryUtilsMock.Setup(x => x.SetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<UserAuthentication>())).Throws(new Exception());
        //    Mock<CustomerUtils> custUtils = new Mock<CustomerUtils>();
        //    GetProfileResponse response = custUtils.Object.RefreshToken("", "", caller_id, customerId, true);

        //    if (ConfigurationUtils.ShowOnlyOneServerIsBusyError)
        //        Assert.AreEqual(response.ErrorList.Count, 1);
        //    else
        //        Assert.AreEqual(response.ErrorList.Count, RetryCounter.DbRetryCounter);
        //}

        

       
       

        #region UpdateLogoutDate Test
        [TestMethod()]
        public void TestUpdateLogoutDate_ValidToken_LogoutDateShouldBeUpdated()
        {
            #region Arrange
            int customerId = 572845;
            string callerId = "Portal";
            string tokenMd5 = "a43b38e70c75d1ed8ef9c47f5de2833b";
            UserAuthentication hppAuth = new UserAuthentication()
            {
                UserId = customerId,
                CallerId = callerId,
                TokenMD5 = tokenMd5,
                CountryCode = "en",
                LanguageCode = "US"
            };
            queryUtilsMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(hppAuth);
            #endregion Arrange

            #region Act
            ResponseBase response = new ResponseBase();
            CustomerUtils custUtils = new CustomerUtils();
            custUtils.UpdateLogoutDate(response, customerId, callerId, tokenMd5);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, response.ErrorList.Count, "Unexpected fault encountered");
            databaseMock.Verify(x => x.SubmitChanges(), Times.Exactly(1));
            #endregion Assert
        }

        [TestMethod()]
        public void TestUpdateLogoutDate_InvalidToken_LogoutDateShouldNotBeUpdated()
        {
            #region Arrange
            int customerId = 572845;
            string callerId = "Portal";
            string tokenMd5 = "a43b38e70c75d1ed8ef9c47f5de2833b";
            queryUtilsMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<UserAuthentication>(null);
            #endregion Arrange

            #region Act
            ResponseBase response = new ResponseBase();
            CustomerUtils custUtils = new CustomerUtils();
            custUtils.UpdateLogoutDate(response, customerId, callerId, tokenMd5);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, response.ErrorList.Count, "Unexpected fault encountered");
            databaseMock.Verify(x => x.SubmitChanges(), Times.Exactly(0));
            #endregion Assert
        }
        #endregion UpdateLogoutDate Test


       
    }
}
