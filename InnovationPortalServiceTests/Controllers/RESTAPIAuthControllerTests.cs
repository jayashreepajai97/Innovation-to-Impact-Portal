using Hpcs.DependencyInjector;
using IdeaDatabase.Credentials;
using InnovationPortalService.Responses;
using InnovationPortalService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responses;
using System.Linq;
using System.Collections.Generic;
using IdeaDatabase.Interchange;
using IdeaDatabase.Utils;
using IdeaDatabase.Enums;
using InnovationPortalService.Controllers;
using InnovationPortalService;

namespace InnovationPortalServiceTests.Controllers.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RESTAPIAuthControllerTests
    {
        Mock<ICustomerHPIDUtils> authMock;
         
        Mock<IUserUtils> isacMock;
        
        RESTAPIAuthController acontroler;

        [TestInitialize()]
        public void Init()
        {
            authMock = new Mock<ICustomerHPIDUtils>();
            DependencyInjector.Register(authMock.Object).As<ICustomerHPIDUtils>();
            
       
            isacMock = new Mock<IUserUtils>();
            DependencyInjector.Register(isacMock.Object).As<IUserUtils>();

            acontroler = new RESTAPIAuthController();
        }

        [TestMethod()]
        public void AuthenticateTest_PassingClientId()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                ErrorList = new HashSet<Fault>() { new Fault("", "", "") }
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            RESTAPIAuthCredentials request = new RESTAPIAuthCredentials();
            string clientId = "theClientId";
            request.ClientId = clientId;

            RESTAPILoginResponse expectedResponse = acontroler.Authenticate(request);

            authMock.Verify(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.Is<string>(s=>s== clientId)), Times.Once);            
        }



        [TestMethod()]
        public void AuthenticateTest_NoCustomerProfileFound()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                ErrorList = new HashSet<Fault>() { new Fault("", "", "") }
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            RESTAPILoginResponse expectedResponse = acontroler.Authenticate(new RESTAPIAuthCredentials());
            Assert.IsTrue(expectedResponse.ErrorList.Count == 1);

            authMock.Verify(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public void AuthenticateTest_MobileDeviceError()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                Credentials = new Credentials.AccessCredentials(),
                CustomerProfileObject = new CustomerProfile()
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            ResponseBase res = new ResponseBase()
            {
                ErrorList = new HashSet<Fault>() { new Fault("", "", "") }
            };

            RESTAPIAuthCredentials req = new RESTAPIAuthCredentials()
            {
                DeviceToken = "SDILASNP",
                Platform = "platform"
            };
            RESTAPILoginResponse expectedResponse = acontroler.Authenticate(req);
            Assert.IsTrue(expectedResponse.ErrorList.Count == 0);

            authMock.Verify(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public void AuthenticateTest()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                Credentials = new Credentials.AccessCredentials(),
                CustomerProfileObject = new CustomerProfile()
                {
                    ActiveHealth = true
                }
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            ResponseBase res = new ResponseBase();

            RESTAPIAuthCredentials req = new RESTAPIAuthCredentials()
            {
                DeviceToken = "SDILASNP",
                Platform = "platform"
            };
            Assert.IsNotNull(acontroler.Authenticate(req));

            authMock.Verify(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public void AuthenticateTest_EmptyOrNullDevicetoken()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                Credentials = new Credentials.AccessCredentials(),
                CustomerProfileObject = new CustomerProfile()
                {
                    ActiveHealth = true
                }
            };

            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            ResponseBase res = new ResponseBase();

            RESTAPIAuthCredentials req = new RESTAPIAuthCredentials()
            {
                Platform = "ios"
            };

            RESTAPILoginResponse expectedResponse = acontroler.Authenticate(req);
            Assert.IsTrue(expectedResponse.ErrorList.Count == 1);
            Assert.IsNotNull(expectedResponse.ErrorList.Where(x => x.ReturnCode == Faults.EmptyOrNullDevicetoken.ReturnCode));
        }

        [TestMethod()]
        public void AuthenticateTest_OtherThanWindowsPlatform()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                Credentials = new Credentials.AccessCredentials(),
                CustomerProfileObject = new CustomerProfile()
                {
                    ActiveHealth = true
                }
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            ResponseBase res = new ResponseBase();

            RESTAPIAuthCredentials req = new RESTAPIAuthCredentials()
            {
                DeviceToken = "SDILASNP",
                Platform = "web"
            };
            Assert.IsNotNull(acontroler.Authenticate(req));

        }

        [TestMethod()]
        public void AuthenticateTest_WindowsPlatform_SetClientVersion()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                Credentials = new Credentials.AccessCredentials(),
                CustomerProfileObject = new CustomerProfile()
                {
                    ActiveHealth = true
                }
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            ResponseBase res = new ResponseBase();

            RESTAPIAuthCredentials req = new RESTAPIAuthCredentials()
            {
                DeviceToken = "SDILASNP",
                Platform = "windows"
            };
            Assert.IsNotNull(acontroler.Authenticate(req));

        }

        [TestMethod()]
        public void AuthenticateTest_ClientIdNotNull_SaveClientIdtoDB()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                Credentials = new Credentials.AccessCredentials(),
                CustomerProfileObject = new CustomerProfile()
                {
                    ActiveHealth = true
                }
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            ResponseBase res = new ResponseBase();

            RESTAPIAuthCredentials req = new RESTAPIAuthCredentials()
            {
                DeviceToken = "SDILASNP",
                Platform = "windows",
                ClientId = "clientId"
            };
            Assert.IsNotNull(acontroler.Authenticate(req));

        }

        [TestMethod()]
        public void AuthenticateTest_IOSPlatform_SetClientVersionSanctuary()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                Credentials = new Credentials.AccessCredentials(),
                CustomerProfileObject = new CustomerProfile()
                {
                    ActiveHealth = true
                }
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            ResponseBase res = new ResponseBase();

            RESTAPIAuthCredentials req = new RESTAPIAuthCredentials()
            {
                DeviceToken = "SDILASNP",
                Platform = "IOS"
            };
            Assert.IsNotNull(acontroler.Authenticate(req));

        }

        [TestMethod()]
        public void AuthenticateTest_AndroidPlatform_SetClientVersionSanctuary()
        {
            GetProfileResponse response = new GetProfileResponse()
            {
                Credentials = new Credentials.AccessCredentials(),
                CustomerProfileObject = new CustomerProfile()
                {
                    ActiveHealth = true
                }
            };
            authMock.Setup(x => x.GetCustomerProfileByAuthentication(It.IsAny<UserAuthenticationInterchange>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<APIMethods>(), It.IsAny<string>())).Returns(response);

            ResponseBase res = new ResponseBase();

            RESTAPIAuthCredentials req = new RESTAPIAuthCredentials()
            {
                DeviceToken = "SDILASNP",
                Platform = "Android"
            };
            Assert.IsNotNull(acontroler.Authenticate(req));

        }
    }
}