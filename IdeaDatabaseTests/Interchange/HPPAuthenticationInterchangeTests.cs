using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IdeaDatabase.Interchange;
using IdeaDatabase.DataContext;

namespace DeviceDatabaseTests.Interchange
{
    [TestClass]
    public class HPPAuthenticationInterchangeTests
    {
        [TestMethod]
        public void HPPAuthInterchangeCastTest()
        {
            UserAuthenticationInterchange hppAuthInterchange = GetHPPAuthInterchangeByPlatform("Android");

            UserAuthentication hppAuth = (UserAuthentication)hppAuthInterchange;

            Assert.IsNotNull(hppAuth.TokenMD5);
            Assert.IsTrue(hppAuth.IsHPID.Value);
            Assert.AreEqual(hppAuth.ClientPlatform, "android");
        }

        [TestMethod]
        public void HPPAuthInterchangeCastTest_PlatformIOS()
        {
            UserAuthenticationInterchange hppAuthInterchange = GetHPPAuthInterchangeByPlatform("IOS");

            UserAuthentication hppAuth = (UserAuthentication)hppAuthInterchange;

            Assert.IsNotNull(hppAuth.TokenMD5);
            Assert.IsTrue(hppAuth.IsHPID.Value);
            Assert.AreEqual(hppAuth.ClientPlatform, "ios");
        }

        [TestMethod]
        public void HPPAuthInterchangeCastTest_PlatformInvalid()
        {
            UserAuthenticationInterchange hppAuthInterchange = GetHPPAuthInterchangeByPlatform("xyz");

            UserAuthentication hppAuth = (UserAuthentication)hppAuthInterchange;

            Assert.IsNotNull(hppAuth.TokenMD5);
            Assert.IsTrue(hppAuth.IsHPID.Value);
            Assert.IsNull(hppAuth.ClientApplication);
        }

        [TestMethod]
        public void HPPAuthInterchangeCastTest_MapPlatformtoCLientApplication()
        {
            UserAuthenticationInterchange hppAuthInterchange = GetHPPAuthInterchangeByPlatform("web");

            UserAuthentication hppAuth = (UserAuthentication)hppAuthInterchange;

            Assert.IsNotNull(hppAuth.TokenMD5);
            Assert.IsTrue(hppAuth.IsHPID.Value);
            Assert.AreEqual(hppAuth.ClientApplication, "HP.InnovationPortal");
            Assert.AreEqual(hppAuth.ClientPlatform, "web");
 
        }

        private static UserAuthenticationInterchange GetHPPAuthInterchangeByPlatform(string platform)
        {
            return new UserAuthenticationInterchange()
            {
                UserName = "Username",
                Password = "Password",
                UserId = 1234,
                TokenMD5 = null,
                CallerId = "CallerId",
                Token = "Token",
                LanguageCode = "en",
                CountryCode = "US",
                IsHPID = true,
                DateUpdated = DateTime.UtcNow,
                UseCaseGroup = "UseCaseGroup",
                Platform = platform,
                ClientVersion = null,
                ClientApplication = UserAuthenticationInterchange.MapPlatformToClientApplication(platform)
            };
        }
    }
}
