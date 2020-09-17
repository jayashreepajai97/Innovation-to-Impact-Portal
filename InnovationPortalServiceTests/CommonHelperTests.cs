using Microsoft.VisualStudio.TestTools.UnitTesting;
using InnovationPortalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
//using InnovationPortalService.HPPService;

namespace InnovationPortalServiceTests.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass()]
    public class CommonHelperTests
    {
       

        //[TestMethod()]
        //public void GetHPPContextTest()
        //{
        //    CommonHelper ch = new CommonHelper();
        //    var context = ch.GetHPPContext("42");

        //    Assert.IsTrue(context.systemLangCode == "en");
        //    Assert.IsTrue(context.version == "42");

        //}

        //[TestMethod()]
        //public void GetLoginRequestTest()
        //{
        //    CommonHelper common = new CommonHelper();
        //    string username = "username";
        //    string password = "password";
        //    loginRequest request = common.GetLoginRequest(username, password);

        //    Assert.IsTrue(request.loginRequestElement.userId.CompareTo(username) == 0);
        //    Assert.IsTrue(request.loginRequestElement.password.CompareTo(password) == 0);
        //    Assert.IsTrue(request.hppwsHeaderElement.version.CompareTo("3") == 0);
        //}
    }
}