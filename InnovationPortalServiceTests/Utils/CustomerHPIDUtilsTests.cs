using Credentials;
using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using IdeaDatabase.Utils;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using Hpcs.DependencyInjector;
using InnovationPortalService;
using InnovationPortalService.HPID;
using InnovationPortalService.Responses;
using InnovationPortalService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responses;
using SettingsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static InnovationPortalService.HPID.HPIDUtils;

namespace InnovationPortalServiceTests.Utils.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CustomerHPIDUtilsTests
    {
        private static Mock<IAUTHUtils> hpidUtilsMock;
        private static Mock<IUserUtils> isacMock;
        private static Mock<IQueryUtils> queryUtilsMock;
        private static Mock<IIdeaDatabaseDataContext> databaseMock;
        private static Mock<ISCIMMUtils> scimMock;
        private static Mock<IRoleUtils> roleUtils;

        private static Mock<ICustomerUtils> customerUtilsMock;
 
        //private static Mock<IManufacturingInstalledBaseService> iManufacturingInstalledBaseService;


        delegate void OutCallBack(ResponseBase req, string ip, string hppi, string ppl, TokenDetails rt, string clientId, out bool IsNew, APIMethods apiMethods);

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            hpidUtilsMock = new Mock<IAUTHUtils>();
            isacMock = new Mock<IUserUtils>();
            queryUtilsMock = new Mock<IQueryUtils>();
            databaseMock = new Mock<IIdeaDatabaseDataContext>();
            scimMock = new Mock<ISCIMMUtils>();
            roleUtils = new Mock<IRoleUtils>();
          
            
            customerUtilsMock = new Mock<ICustomerUtils>();
            
           // iManufacturingInstalledBaseService = new Mock<IManufacturingInstalledBaseService>();

            DependencyInjector.Register(roleUtils.Object).As<IRoleUtils>();

            DependencyInjector.Register(hpidUtilsMock.Object).As<IAUTHUtils>();
            DependencyInjector.Register(isacMock.Object).As<IUserUtils>();
            DependencyInjector.Register(queryUtilsMock.Object).As<IQueryUtils>();
            DependencyInjector.Register(databaseMock.Object).As<IIdeaDatabaseDataContext>();
            DependencyInjector.Register(scimMock.Object).As<ISCIMMUtils>();
 
            DependencyInjector.Register(customerUtilsMock.Object).As<ICustomerUtils>();
 
          //  DependencyInjector.Register(iManufacturingInstalledBaseService.Object).As<IManufacturingInstalledBaseService>();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DependencyInjector.Clear();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            hpidUtilsMock.Reset();
            isacMock.Reset();
            queryUtilsMock.Reset();
            databaseMock.Reset();
            scimMock.Reset();
           
         
            customerUtilsMock.Reset();
          
            //iManufacturingInstalledBaseService.Reset();
        }

        [TestMethod()]
        public void CreateCustomerProfileTest_MissingSessionToken()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();
            TokenDetails sessionToken = new TokenDetails();

            GetProfileResponse response = custUtils.GetCustomerProfileforHPID(null, sessionToken, false, It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Contains(Faults.InvalidCredentials));
        }

        [TestMethod()]
        public void CreateCustomerProfileTest_HPIDconnectionFailure()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();
            TokenDetails sessionToken = new TokenDetails();
            sessionToken.AccessToken = "sessionToken";

            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).
                Callback((CustomerIds i, string s, GetProfileResponse r) =>
                        {
                            r.ErrorList.Add(Faults.HPIDInternalError);
                        }).Returns(false);

            GetProfileResponse response = custUtils.GetCustomerProfileforHPID(null, sessionToken, false, It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Contains(Faults.HPIDInternalError));
        }

        [TestMethod()]
        public void GetCustomerProfileTest_InvalidIsacPrifile()
        {
            bool IsNewCustomer = true;
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();
            TokenDetails sessionToken = new TokenDetails();
            UserAuthenticationInterchange hppAuthInt = new UserAuthenticationInterchange()
            {
                ClientId = "hpsa9"
            };

            User aProfile = new User() { EmailConsent = true };

            List<RoleMapping> roleMappings = new List<RoleMapping>();
            RoleMapping role = new RoleMapping();
            role.RoleId = 1;
            role.RoleMappingId = 1;
            role.UserId = 1;
            role.CreatedDate = DateTime.UtcNow;
            roleMappings.Add(role);
            aProfile.RoleMappings = roleMappings;

            sessionToken.AccessToken = "sessionToken";
            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).Returns(true);

            isacMock.Setup(x => x.FindOrInsertHPIDProfile(It.IsAny<ResponseBase>(), It.IsAny<RequestFindOrInsertHPIDProfile>(), out IsNewCustomer)).Returns(aProfile);

            GetProfileResponse response = custUtils.GetCustomerProfileforHPID(hppAuthInt, sessionToken, false, It.IsAny<APIMethods>());

            Assert.IsTrue(response.ErrorList.Count == 1);             
            Assert.IsTrue(IsNewCustomer);
        }

        [TestMethod()]
        public void GetCustomerProfileTest_IsacPrifileThrowsException()
        {
            bool IsNewCustomer = false;
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();
            TokenDetails sessionToken = new TokenDetails();
            sessionToken.AccessToken = "sessionToken";
            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).Returns(true);
            isacMock.Setup(x => x.FindOrInsertHPIDProfile(It.IsAny<ResponseBase>(), It.IsAny<RequestFindOrInsertHPIDProfile>(), out IsNewCustomer)).Throws(new Exception());

            GetProfileResponse response = custUtils.GetCustomerProfileforHPID(null, sessionToken, false, It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.First().ReturnCode.Equals("GetCustomerProfileFailed"));
        }

        [TestMethod()]
        public void GetCustomerProfileByLoginTest_InvalidCredentials()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();

            GetProfileResponse response = custUtils.GetCustomerProfileByLogin(new UserAuthenticationInterchange(), It.IsAny<bool>(), It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.Contains(Faults.InvalidCredentials));
        }

        [TestMethod()]
        public void GetCustomerProfileByLoginTest_HPIDInternalError()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();

            hpidUtilsMock.Setup(x => x.GetHPIDSessionToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>(), It.IsAny<string>(), It.IsAny<int>())).
                Callback((int t, string u, string p, ResponseBase r, string c, int z) =>
                {
                    r.ErrorList.Add(Faults.HPIDInternalError);
                });

            GetProfileResponse response = custUtils.GetCustomerProfileByLogin(new UserAuthenticationInterchange() { UserName = "user", Password = "passwd" }, It.IsAny<bool>(), It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.Contains(Faults.HPIDInternalError));
        }

        [TestMethod()]
        public void GetCustomerProfileByLoginTest_Success()
        {
            bool IsNewCustomer = false;
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();
            TokenDetails sessionToken = new TokenDetails();
            sessionToken.AccessToken = "sessionToken";
            hpidUtilsMock.Setup(x => x.GetHPIDSessionToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>(), It.IsAny<string>(), It.IsAny<int>())).Returns(sessionToken);
            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).
                Callback((CustomerIds i, string u, GetProfileResponse r) =>
                {
                    i = new CustomerIds() { HPIDid = "hpidid", HPPid = "hppid" };
                    r.CustomerProfileObject = new CustomerProfile();
                }).Returns(true);

            User aProfile = new User() { EmailConsent = true };

            List<RoleMapping> roleMappings = new List<RoleMapping>();
            RoleMapping role = new RoleMapping();
            role.RoleId = 1;
            role.RoleMappingId = 1;
            role.UserId = 1;
            role.CreatedDate = DateTime.UtcNow;
            roleMappings.Add(role);
            aProfile.RoleMappings = roleMappings;

            isacMock.Setup(x => x.FindOrInsertHPIDProfile(It.IsAny<ResponseBase>(), It.IsAny<RequestFindOrInsertHPIDProfile>(), out IsNewCustomer)).Returns(aProfile);

            GetProfileResponse response = custUtils.GetCustomerProfileByLogin(new UserAuthenticationInterchange() { UserName = "userlogin", Password = "passwd" }, It.IsAny<bool>(), It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Count == 0);
            Assert.IsTrue(response.CustomerProfileObject.EmailConsent.Equals(EmailConsentType.Y.ToString()));
        }

        [TestMethod()]
        public void GetCustomerProfileByAuthenticationTest_InvalidCredentials()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();

            GetProfileResponse response = custUtils.GetCustomerProfileByAuthentication(null, It.IsAny<bool>(), null, null, It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.Contains(Faults.InvalidCredentials));
        }

        [TestMethod()]
        public void GetCustomerProfileByAuthenticationTest_HPIDInternalError()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();

            hpidUtilsMock.Setup(x => x.GetHPIDSessionToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>(), It.IsAny<string>(), It.IsAny<int>())).
                Callback((int t, string u, string p, ResponseBase r, string c, int z) =>
                {
                    r.ErrorList.Add(Faults.HPIDInternalError);
                });

            GetProfileResponse response = custUtils.GetCustomerProfileByAuthentication(null, It.IsAny<bool>(), "access", "url", It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.Contains(Faults.HPIDInternalError));
        }

        [TestMethod()]
        public void GetCustomerProfileByAuthenticationTest_Success()
        {
            bool IsNewCustomer = false;
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();
            RoleUtils rlUtils = new RoleUtils();
            TokenDetails sessionToken = new TokenDetails();
            sessionToken.AccessToken = "sessionToken";
            hpidUtilsMock.Setup(x => x.GetHPIDSessionToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>(), It.IsAny<string>(), It.IsAny<int>())).Returns(sessionToken);
            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).
                Callback((CustomerIds i, string u, GetProfileResponse r) =>
                {
                    i = new CustomerIds() { HPIDid = "hpidid", HPPid = "hppid" };
                    r.CustomerProfileObject = new CustomerProfile();
                }).Returns(true);


            User aProfile = new User() { EmailConsent = true };

            List<RoleMapping> roleMappings = new List<RoleMapping>();
            RoleMapping role = new RoleMapping();
            role.RoleId = 1;
            role.RoleMappingId = 1;
            role.UserId = 1;
            role.CreatedDate = DateTime.UtcNow;
            roleMappings.Add(role);
            aProfile.RoleMappings = roleMappings;
            isacMock.Setup(x => x.FindOrInsertHPIDProfile(It.IsAny<ResponseBase>(), It.IsAny<RequestFindOrInsertHPIDProfile>(), out IsNewCustomer)).Returns(aProfile);



            GetProfileResponse response = custUtils.GetCustomerProfileByAuthentication(new UserAuthenticationInterchange(), It.IsAny<bool>(), "access", "url", It.IsAny<APIMethods>());
            Assert.IsTrue(response.ErrorList.Count == 0);
            Assert.IsTrue(response.CustomerProfileObject.EmailConsent.Equals(EmailConsentType.Y.ToString()));
        }

       
      
        [TestMethod()]
        public void GetCustomerProfileforHPIDTest_SessionTokenExpired()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();

            TokenDetails sessionToken = new TokenDetails()
            {
                AccessToken = "accessToken",
                tokenScopeType = TokenScopeType.apiProfileGetByTokenCall
            };

            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).
                Callback((CustomerIds i, string u, GetProfileResponse r) =>
                {
                    r.ErrorList.Add(Faults.HPIDInvalidToken);
                }).Returns(false);


            GetProfileResponse response = custUtils.GetCustomerProfileforHPID(null, sessionToken, It.IsAny<bool>(), It.IsAny<APIMethods>());

            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.Contains(Faults.InvalidCredentials));
        }

        [TestMethod()]
        public void GetCustomerProfileforHPIDTest_MissingRefreshToken()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();

            TokenDetails sessionToken = new TokenDetails()
            {
                AccessToken = "accessToken",
                tokenScopeType = TokenScopeType.apiProfileGetCall
            };

            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).
                Callback((CustomerIds i, string u, GetProfileResponse r) =>
                {
                    r.ErrorList.Add(Faults.HPIDInvalidToken);
                }).Returns(false);

            User isaacUser = new User() { RefreshToken = null };
            isacMock.Setup(x => x.GetRefreshToken(It.IsAny<string>(), It.IsAny<TokenDetails>())).Returns(isaacUser);


            GetProfileResponse response = custUtils.GetCustomerProfileforHPID(new UserAuthenticationInterchange(), sessionToken, false, It.IsAny<APIMethods>());

            
            customerUtilsMock.Verify(x => x.UpdateLogoutDate(It.IsAny<ResponseBase>(),
                                   It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.Contains(Faults.HPIDSessionTimeout));
        }

        [TestMethod()]
        public void GetCustomerProfileforHPIDTest_InvalidRefreshToken()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();

            TokenDetails sessionToken = new TokenDetails()
            {
                AccessToken = "accessToken",
                tokenScopeType = TokenScopeType.apiProfileGetCall
            };

            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).
                Callback((CustomerIds i, string u, GetProfileResponse r) =>
                {
                    r.ErrorList.Add(Faults.HPIDInvalidToken);
                }).Returns(false);

            User isaacUser = new User() { RefreshToken = "refreshToken", RefreshTokenType = 1 };
            isacMock.Setup(x => x.GetRefreshToken(It.IsAny<string>(), It.IsAny<TokenDetails>())).Returns(isaacUser);

            queryUtilsMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>())).
                Returns(new UserAuthentication() { UserId = 123, ClientId = "Test" });

            TokenDetails refreshToken = new TokenDetails()
            {
                AccessToken = ""
            };
            hpidUtilsMock.Setup(x => x.GetHPIDSessionToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ResponseBase>(), It.IsAny<string>(), It.IsAny<int>())).Returns(refreshToken);

            GetProfileResponse response = custUtils.GetCustomerProfileforHPID(new UserAuthenticationInterchange(), sessionToken, It.IsAny<bool>(), It.IsAny<APIMethods>());

            
            customerUtilsMock.Verify(x => x.UpdateLogoutDate(It.IsAny<ResponseBase>(),
                                   It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            hpidUtilsMock.Verify(x => x.GetHPIDSessionToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ResponseBase>(), It.Is<string>(y => y == "Test"), It.IsAny<int>()), Times.Once);
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.Contains(Faults.HPIDSessionTimeout));
        }

        [TestMethod()]
        public void GetCustomerProfileforHPIDTest_Success()
        {
            CustomerHPIDUtils custUtils = new CustomerHPIDUtils();

            User isaacUser = new User()
            {
                UserId = 120034,
                RefreshToken = "refreshToken",
                RefreshTokenType = 1,
                ActiveHealth = true,
                EmailConsent = true,
                PrimaryUse = PrimaryUseType.Item003.ToString()
            };
            isacMock.Setup(x => x.GetRefreshToken(It.IsAny<string>(), It.IsAny<TokenDetails>())).Returns(isaacUser);


            TokenDetails sessionToken = new TokenDetails()
            {
                AccessToken = "expiredAccessToken",
                tokenScopeType = TokenScopeType.apiProfileGetCall
            };
            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsAny<string>(), It.IsAny<GetProfileResponse>())).
                Callback((CustomerIds i, string u, GetProfileResponse r) =>
                {
                    r.ErrorList.Add(Faults.HPIDInvalidToken);
                }).Returns(false);

            queryUtilsMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>())).
               Returns(new UserAuthentication() { UserId = 123, ClientId = "Test" });

            TokenDetails refreshToken = new TokenDetails()
            {
                AccessToken = "newAccessToken"
            };
            hpidUtilsMock.Setup(x => x.GetHPIDSessionToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ResponseBase>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(refreshToken);
            hpidUtilsMock.Setup(x => x.GetIdsAndProfile(It.IsAny<CustomerIds>(), It.IsIn<string>(refreshToken.AccessToken), It.IsAny<GetProfileResponse>())).
               Callback((CustomerIds i, string u, GetProfileResponse r) =>
               {
                   r.CustomerProfileObject = new CustomerProfile();
               }).Returns(true);

            GetProfileResponse response = custUtils.GetCustomerProfileforHPID(new UserAuthenticationInterchange(), sessionToken, It.IsAny<bool>(), It.IsAny<APIMethods>());

            hpidUtilsMock.Verify(x => x.GetHPIDSessionToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ResponseBase>(), It.Is<string>(y => y == "Test"), It.IsAny<int>()), Times.Once);
            Assert.IsTrue(response.ErrorList.Count == 0);
            Assert.IsTrue(response.CustomerProfileObject.ActiveHealth == isaacUser.ActiveHealth);
            Assert.IsTrue(response.CustomerProfileObject.EmailConsent == EmailConsentType.Y.ToString());
        }
            
    }
}
