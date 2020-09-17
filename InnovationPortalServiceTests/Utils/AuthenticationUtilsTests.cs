using InnovationPortalService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
//using InnovationPortalService.HPPService;
using Hpcs.DependencyInjector;
using InnovationPortalService.Responses;
using Responses;
using System.Linq;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;
using System.ServiceModel;
using InnovationPortalService;

namespace InnovationPortalServiceTests.Utils.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AuthenticationUtilsTests
    {
        Mock<ICommonHelper> helperMock;
       // Mock<DocLiteral> hppMock;
        Mock<IExceptionMapping> excMock;

        [TestInitialize]
        public void Initialize()
        {
            helperMock = new Mock<ICommonHelper>();
           // hppMock = new Mock<DocLiteral>();
            excMock = new Mock<IExceptionMapping>();

            DependencyInjector.Register(helperMock.Object).As<ICommonHelper>();
           // DependencyInjector.Register(hppMock.Object).As<DocLiteral>();
            DependencyInjector.Register(excMock.Object).As<IExceptionMapping>();
        }      

        //[TestMethod()]
        //public void GetProfileIdByEmailTestNull()
        //{
        //    Initialize();

        //    checkUserExistsResponse check = null;
        //    hppMock.Setup(x => x.checkUserExists(It.IsAny<checkUserExistsRequest>())).Returns(check);
        //    AuthenticationUtils auth = new AuthenticationUtils();

        //    Assert.IsNull(auth.GetProfileIdByEmail("email"));

        //    DependencyInjector.Clear();
        //}

        //[TestMethod()]
        //public void GetProfileIdByEmailTestResponse()
        //{
        //    Initialize();

        //    checkUserExistsResponse check = new checkUserExistsResponse() { checkUserExistsResponseElement = new checkUserExistsResultType() { profileIdByEmail = "profile" } };
        //    hppMock.Setup(x => x.checkUserExists(It.IsAny<checkUserExistsRequest>())).Returns(check);
        //    AuthenticationUtils auth = new AuthenticationUtils();

        //    Assert.IsTrue(auth.GetProfileIdByEmail("email").CompareTo("profile") == 0);

        //    DependencyInjector.Clear();
        //}

        //[TestMethod()]
        //public void GetProfileIdByEmailTestFaultException268()
        //{
        //    Initialize();
        //    genericFault genFault = new genericFault();
            
        //    FaultException<genericFault> ex1 = new FaultException<genericFault>(genFault);
        //    genFault.fault = new genericFaultType[1];
        //    genFault.fault[0] = new genericFaultType(){
        //        ruleNumber = 268
        //    };

        //    checkUserExistsResponse check = new checkUserExistsResponse();
        //    hppMock.Setup(x => x.checkUserExists(It.IsAny<checkUserExistsRequest>())).Throws(ex1);
        //    AuthenticationUtils auth = new AuthenticationUtils();

        //    Assert.IsNull(auth.GetProfileIdByEmail(""));

        //    DependencyInjector.Clear();
        //}

        //[TestMethod()]
        //public void GetProfileIdByEmailTestFaultException()
        //{
        //    Initialize();
        //    genericFault genFault = new genericFault();
        //    int ruleNumber = 11268;
        //    FaultException<genericFault> ex1 = new FaultException<genericFault>(genFault);
        //    genFault.fault = new genericFaultType[1];
        //    genFault.fault[0] = new genericFaultType()
        //    {
        //        ruleNumber = ruleNumber /* different number */
        //    };

        //    checkUserExistsResponse check = new checkUserExistsResponse();
        //    hppMock.Setup(x => x.checkUserExists(It.IsAny<checkUserExistsRequest>())).Throws(ex1);
        //    try
        //    {
        //        AuthenticationUtils auth = new AuthenticationUtils();
        //        Assert.IsNull(auth.GetProfileIdByEmail(""));
        //    }
        //    catch (FaultException<genericFault> ex)
        //    {
        //        Assert.IsTrue(ex.Detail.fault[0].ruleNumber == ruleNumber);
        //    }
            
        //    DependencyInjector.Clear();
        //}

        //[TestMethod()]
        //public void GetProfileIdByUserIdTestNull()
        //{
        //    Initialize();

        //    checkUserExistsResponse check = null;
        //    hppMock.Setup(x => x.checkUserExists(It.IsAny<checkUserExistsRequest>())).Returns(check);
        //    AuthenticationUtils auth = new AuthenticationUtils();

        //    Assert.IsNull(auth.GetProfileIdByUserId("user"));

        //    DependencyInjector.Clear();
        //}

        //[TestMethod()]
        //public void GetProfileIdByUserIdTestResponse()
        //{
        //    Initialize();

        //    checkUserExistsResponse check = new checkUserExistsResponse() { checkUserExistsResponseElement = new checkUserExistsResultType() { profileIdByUserId = "profile" } };
        //    hppMock.Setup(x => x.checkUserExists(It.IsAny<checkUserExistsRequest>())).Returns(check);
        //    AuthenticationUtils auth = new AuthenticationUtils();

        //    Assert.IsTrue(auth.GetProfileIdByUserId("user").CompareTo("profile") == 0);

        //    DependencyInjector.Clear();
        //}        
    }
}