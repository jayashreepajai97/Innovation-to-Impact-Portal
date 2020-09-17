using IdeaDatabase.Credentials;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Responses;
using System.Linq;

namespace DeviceDatabaseTests.Credentials
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RESTAPIAuthCredentialsTests
    {
        [TestMethod]
        public void RESTAPIAuthCredentialsTest_InvalidCredentials()
        {
            RESTAPIAuthCredentials ac = new RESTAPIAuthCredentials();
            ResponseBase response = new ResponseBase();
            Assert.IsFalse(ac.IsValid(response));
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.ElementAt(0).ReturnCode.Equals(Faults.InvalidCredentials.ReturnCode));
        }

        [TestMethod]
        public void RESTAPIAuthCredentialsTest_Success()
        {
            RESTAPIAuthCredentials ac = new RESTAPIAuthCredentials();

            ac.DeviceToken = "deviceToken";
            ac.Platform = RESTAPIPlatform.web.ToString();
            ac.AccessCode = "accessCode";
            ac.RedirectUrl = "redirectUrl";
            ac.CallerId = "callerId";
            ac.Locale = "pt-BR";
            ac.ClientViewer = "SANC";

            ResponseBase response = new ResponseBase();
            Assert.IsTrue(ac.IsValid(response));
            Assert.IsTrue(response.ErrorList.Count == 0);
        }

        [TestMethod]
        public void RESTAPIAuthCredentialsTest_ValidLanguageAndCountryCode()
        {
            RESTAPIAuthCredentials ac = new RESTAPIAuthCredentials();
            ac.DeviceToken = "deviceToken";
            ac.Platform = RESTAPIPlatform.web.ToString();
            ac.AccessCode = "accessCode";
            ac.RedirectUrl = "redirectUrl";
            ac.CallerId = "callerId";
            ac.ClientViewer = "SANC";
            var lc = "pt";
            var cc = "BR";
            ac.Locale = $"{lc}-{cc}";

            ResponseBase response = new ResponseBase();
            Assert.IsTrue(ac.IsValid(response));
            Assert.IsTrue(ac.LanguageCode == lc);
            Assert.IsTrue(ac.CountryCode == cc);
        }
    }
}
