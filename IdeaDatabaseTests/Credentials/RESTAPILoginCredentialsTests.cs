using Microsoft.VisualStudio.TestTools.UnitTesting;
using IdeaDatabase.Credentials;
using Responses;
using IdeaDatabase.Responses;
using System.Linq;
using IdeaDatabase.Enums;

namespace DeviceDatabaseTests.Credentials
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RESTAPILoginCredentialsTests
    {
        [TestMethod]
        public void RESTAPILoginCredentialsTest_InvalidCredentials()
        {
            RESTAPILoginCredentials lc = new RESTAPILoginCredentials();
            ResponseBase response = new ResponseBase();
            Assert.IsFalse(lc.IsValid(response));
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.ElementAt(0).ReturnCode.Equals(Faults.InvalidCredentials.ReturnCode));
        }

        [TestMethod]
        public void RESTAPILoginCredentialsTest_Success()
        {
            RESTAPILoginCredentials lc = new RESTAPILoginCredentials();
            lc.DeviceToken = "deviceToken";
            lc.Platform = RESTAPIPlatform.web.ToString();
            lc.Locale = "pt-BR";
            lc.UserName = "userName";
            lc.Password = "password";
            lc.CallerId = "callerId";
            lc.ClientViewer = "SANC";

            ResponseBase response = new ResponseBase();
            Assert.IsTrue(lc.IsValid(response));
            Assert.IsTrue(response.ErrorList.Count == 0);
        }
    }
}
