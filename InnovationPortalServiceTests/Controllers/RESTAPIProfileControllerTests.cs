using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using InnovationPortalService.Utils;
using Hpcs.DependencyInjector;
using Moq;
using Credentials;
using InnovationPortalService.Responses;
using Responses;
using InnovationPortalService.Requests;
using IdeaDatabase.Credentials;
using IdeaDatabase.Utils;
using IdeaDatabase.Enums;
using SettingsRepository;
using IdeaDatabase.Interchange;
using InnovationPortalService.HPID;
using InnovationPortalService.Controllers;
using InnovationPortalService;

namespace InnovationPortalServiceTests.Controllers.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RESTAPIProfileControllerTests
    {
        private Mock<ICustomerUtils> customerMock;       
        private Mock<ICustomerHPIDUtils> hpidMock;
        private Mock<IUserUtils> isacMock;
        TokenDetails sessionToken;

        [TestInitialize]
        public void Initialize()
        {
            customerMock = new Mock<ICustomerUtils>();
            DependencyInjector.Register(customerMock.Object).As<ICustomerUtils>();
       
        

            hpidMock = new Mock<ICustomerHPIDUtils>();
            DependencyInjector.Register(hpidMock.Object).As<ICustomerHPIDUtils>();
            isacMock = new Mock<IUserUtils>();
            DependencyInjector.Register(isacMock.Object).As<IUserUtils>();
            sessionToken = new TokenDetails();
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
            {
                new AdmSettings { ParamName = "HPIDEnabled", StringValue = "false" },
                new AdmSettings { ParamName = "MaxSessionTimeMinutes", NumValue = 60 }
            });
        }

        [TestMethod()]
        public void LoginTest_ReturnsFault()
        {
            RESTAPIProfileController api = new RESTAPIProfileController();
            RESTAPILoginCredentials req = new RESTAPILoginCredentials()
            {
                UserName = "",
                Password = "",
                CallerId = "",
                Platform = "",
                Locale = ""
            };
            GetProfileResponse profileRes = new GetProfileResponse()
            {
                ErrorList = new HashSet<Fault>() { new Fault("Profile", "", "") }
            };
            //customerMock.Setup(x => x.GetCustomerProfile(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>())).Returns(profileRes);
            hpidMock.Setup(x => x.GetCustomerProfileByTestLogin(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<APIMethods>())).Returns(profileRes);

            RESTAPILoginResponse response = api.Login(req);

            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.AreEqual(response.ErrorList.First().Origin, "Profile");
        }

        [TestMethod()]
        public void LoginHPIDTest_ReturnsFault()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
            {
                new AdmSettings { ParamName = "HPIDEnabled", StringValue = "true" }
            });

            RESTAPIProfileController api = new RESTAPIProfileController();
            RESTAPILoginCredentials req = new RESTAPILoginCredentials()
            {
                UserName = "",
                Password = "",
                CallerId = "",
                Platform = "",
                Locale = ""
            };
            GetProfileResponse profileRes = new GetProfileResponse()
            {
                ErrorList = new HashSet<Fault>() { new Fault("Profile", "", "") }
            };
           
            hpidMock.Setup(x => x.GetCustomerProfileByTestLogin(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<APIMethods>())).Returns(profileRes);
            RESTAPILoginResponse response = api.Login(req);

            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.AreEqual(response.ErrorList.First().Origin, "Profile");
        }

        [TestMethod()]
        public void LoginTest_ReturnSsessionToken()
        {
            RESTAPIProfileController api = new RESTAPIProfileController();
            RESTAPILoginCredentials req = new RESTAPILoginCredentials()
            {
                UserName = "",
                Password = "",
                CallerId = "",
                Platform = "",
                Locale = "en-US"
            };
            GetProfileResponse profileRes = new GetProfileResponse()
            {
                Credentials = new AccessCredentials() { UserID = 1000, SessionToken = "sessionToken", CallerId = "callerId" },
                CustomerProfileObject = new CustomerProfile() { ActiveHealth = true }
            };
            hpidMock.Setup(x => x.GetCustomerProfileByTestLogin(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<APIMethods>())).Returns(profileRes);

            //customerMock.Setup(x => x.GetCustomerProfile(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>())).Returns(profileRes);
            RESTAPILoginResponse response = api.Login(req);

            Assert.IsTrue(response.SessionToken.Equals("sessionToken"));
        }

        

        [TestMethod()]
        public void LoginTest_ReturnsValidCredentials()
        {
            RESTAPIProfileController api = new RESTAPIProfileController();
            RESTAPILoginCredentials req = new RESTAPILoginCredentials()
            {
                UserName = "",
                Password = "",
                CallerId = "",
                Platform = ""
            };
            GetProfileResponse profileRes = new GetProfileResponse()
            {
                Credentials = new AccessCredentials() { UserID = 1000, SessionToken = "sessionToken", CallerId = "callerId" },
                CustomerProfileObject = new CustomerProfile() { ActiveHealth = true }
            };
            hpidMock.Setup(x => x.GetCustomerProfileByTestLogin(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<APIMethods>())).Returns(profileRes);

           // customerMock.Setup(x => x.GetCustomerProfile(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>())).Returns(profileRes);
            RESTAPILoginResponse response = api.Login(req);

            Assert.IsTrue(response.ErrorList.Count == 0);
            Assert.IsTrue(response.UserID == 1000);
            Assert.AreEqual(response.SessionToken, "sessionToken");
            Assert.AreEqual(response.CallerId, "callerId");
       

        }

        [TestMethod()]
        public void LoginTest_ReturnsValidCredentials_MobileFault()
        {
            RESTAPIProfileController api = new RESTAPIProfileController();
            RESTAPILoginCredentials req = new RESTAPILoginCredentials()
            {
                UserName = "",
                Password = "",
                CallerId = "",
                Platform = ""
            };
            GetProfileResponse profileRes = new GetProfileResponse()
            {
                Credentials = new AccessCredentials() { UserID = 1000, SessionToken = "sessionToken", CallerId = "CallerId" },
                CustomerProfileObject = new CustomerProfile() { ActiveHealth = true }
            };
            hpidMock.Setup(x => x.GetCustomerProfileByTestLogin(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<APIMethods>())).Returns(profileRes);

           // customerMock.Setup(x => x.GetCustomerProfile(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>())).Returns(profileRes);
            ResponseBase mobileResponse = new ResponseBase()
            {
                ErrorList = new HashSet<Fault>() { new Fault("", "", "") }
            };
            RESTAPILoginResponse response = api.Login(req);

            Assert.IsTrue(response.ErrorList.Count == 0);
            Assert.IsTrue(response.UserID == 1000);
            Assert.AreEqual(response.SessionToken, "sessionToken");
            Assert.AreEqual(response.CallerId, "CallerId");
    
        }

        [TestMethod()]
        public void LoginTest_ReturnsValidCredentials_CallRegisterMobile()
        {
            
            RESTAPIProfileController api = new RESTAPIProfileController();
            RESTAPILoginCredentials req = new RESTAPILoginCredentials()
            {
                UserName = "",
                Password = "",
                CallerId = "callerId",
                Platform = "PLATFORM",
                DeviceToken = "deviceToken"
            };
            GetProfileResponse profileRes = new GetProfileResponse()
            {
                Credentials = new AccessCredentials() { UserID = 1000, SessionToken = "sessionToken" },
                CustomerProfileObject = new CustomerProfile() { ActiveHealth = true }
            };
            hpidMock.Setup(x => x.GetCustomerProfileByTestLogin(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<APIMethods>())).Returns(profileRes);

            //customerMock.Setup(x => x.GetCustomerProfile(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>())).Returns(profileRes);

            RESTAPILoginResponse response = api.Login(req);

            Assert.IsTrue(response.ErrorList.Count == 0);
     

        }

       
           
     
        //[TestMethod()]
        //public void LogoutTest()
        //{
        //    RESTAPIProfileController api = new RESTAPIProfileController();
        //     api.Logout();
        //    hpidMock.Verify(x => x.ExecuteLogout(It.IsAny<ResponseBase>(),
        //        It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        //}
       
    
        [TestMethod()]
        public void LoginTest_SessionTimeOutResponse()
        {
            DateTime loginDate = new DateTime(2019, 11, 28, 05, 06, 00);

            RESTAPIProfileController api = new RESTAPIProfileController();
            RESTAPILoginCredentials req = new RESTAPILoginCredentials()
            {
                UserName = "",
                Password = "",
                CallerId = "",
                Platform = "",
                Locale = "pl-PL"
            };
            GetProfileResponse profileRes = new GetProfileResponse()
            {
                Credentials = new AccessCredentials() { UserID = 1000, SessionToken = "sessionToken", CallerId = "callerId" },
                CustomerProfileObject = new CustomerProfile() { ActiveHealth = true },
                LoginDate = loginDate
            };
            hpidMock.Setup(x => x.GetCustomerProfileByTestLogin(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<APIMethods>())).Returns(profileRes);

            //customerMock.Setup(x => x.GetCustomerProfile(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>())).Returns(profileRes);

            RESTAPILoginResponse response = api.Login(req);

            Assert.IsNotNull(response.TimeOut);
            Assert.AreEqual("2019-11-28T06:06:00Z", response.TimeOut);
        }

    }
}