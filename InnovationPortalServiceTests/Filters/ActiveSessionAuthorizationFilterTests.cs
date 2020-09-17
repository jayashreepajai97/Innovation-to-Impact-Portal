using IdeaDatabase.DataContext;
using IdeaDatabase.Utils;
using InnovationPortalService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Controllers;

namespace InnovationPortalServiceTests.Filters
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ActiveSessionAuthorizationFilterTests
    {
        //HttpActionContext context; //= new HttpActionExecutedContext();
        //Mock<ICustomerHPIDUtils> customerHPIDUtils;
        //Mock<IIdeaDatabaseDataContext> dbcontextMock;
        //Mock<IQueryUtils> queryMock;
       // [TestInitialize]
        //public void Init()
        //{
        //    dbcontextMock = new Mock<IIdeaDatabaseDataContext>();
        //    DependencyInjector.Register(dbcontextMock.Object).As<IIdeaDatabaseDataContext>();

        //    queryMock = new Mock<IQueryUtils>();
        //    DependencyInjector.Register(queryMock.Object).As<IQueryUtils>();

        //    customerHPIDUtils = new Mock<ICustomerHPIDUtils>();
        //    DependencyInjector.Register(customerHPIDUtils.Object).As<ICustomerHPIDUtils>();

        //    ResponseBase response = new ResponseBase();

        //    context = new HttpActionContext()
        //    {
        //        ControllerContext = new HttpControllerContext()
        //        {
        //            Request = new HttpRequestMessage()
        //            {
        //                Method = HttpMethod.Get,
        //                RequestUri = new Uri("http://localhost:33972/drives/1234"),
        //            },
        //            Controller = new RESTAPIControllerBase()
        //        },
        //        Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        //        {
        //            Content = new ObjectContent(typeof(ResponseBase), response, new JsonMediaTypeFormatter())
        //        }
        //    };

        //    SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
        //    {
        //        new AdmSettings { ParamName = "MaxSessionTimeMinutes", NumValue = 30 }
        //    });

        //}

        //[TestMethod]
        //public void ActiveSessionAuth_OnAuthorization_NotAuthorized_HPPAuthNotFound()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        queryMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>())).Returns<UserAuthentication>(null);

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveSessionAuthorizationFilter activeSessionAuth = new ActiveSessionAuthorizationFilter();
        //        activeSessionAuth.OnAuthorization(context);
        //    }

        //    ObjectContent content = context?.Response?.Content as ObjectContent;
        //    ResponseBase response = content?.Value as ResponseBase;
        //    customerHPIDUtils.Verify(x => x.ExecuteLogout(It.IsAny<ResponseBase>(), It.IsAny<int>(),
        //                            It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        //    Assert.AreEqual(response.ErrorList.Count, 1);
        //    Assert.IsTrue(response.ErrorList.First().ReturnCode.Equals("SessionTimeout"));
        //}

        //[TestMethod]
        //public void ActiveSessionAuth_OnAuthorization_NotAuthorized_LoginDateNull()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        UserAuthentication hppAuth = new UserAuthentication() { UserId = 123, CallerId = "cid", Token = "token", TokenMD5 = "tokenMD5" };
        //        queryMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>())).Returns(hppAuth);

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveSessionAuthorizationFilter activeSessionAuth = new ActiveSessionAuthorizationFilter();
        //        activeSessionAuth.OnAuthorization(context);
        //    }

        //    ObjectContent content = context?.Response?.Content as ObjectContent;
        //    ResponseBase response = content?.Value as ResponseBase;
        //    customerHPIDUtils.Verify(x => x.ExecuteLogout(It.IsAny<ResponseBase>(), It.IsAny<int>(),
        //                            It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        //    Assert.AreEqual(response.ErrorList.Count, 1);
        //    Assert.IsTrue(response.ErrorList.First().ReturnCode.Equals("SessionTimeout"));
        //}

        //[TestMethod]
        //public void ActiveHPIDTokenAuth_OnAuthorization_Authorized()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        UserAuthentication hppAuth = new UserAuthentication() { UserId = 123, CallerId = "cid", Token = "token", TokenMD5 = "tokenMD5", CreatedDate = DateTime.UtcNow };
        //        queryMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>())).Returns(hppAuth);

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveSessionAuthorizationFilter activeSessionAuth = new ActiveSessionAuthorizationFilter();
        //        activeSessionAuth.OnAuthorization(context);
        //    }

        //    ObjectContent content = context?.Response?.Content as ObjectContent;
        //    ResponseBase response = content?.Value as ResponseBase;
        //    Assert.AreEqual(response.ErrorList.Count, 0);
        //}

        //[TestMethod]
        //public void ActiveHPIDTokenAuth_OnAuthorization_Exception()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        queryMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>())).Throws(new Exception());

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveSessionAuthorizationFilter activeSessionAuth = new ActiveSessionAuthorizationFilter();
        //        activeSessionAuth.OnAuthorization(context);
        //    }

        //    ObjectContent content = context?.Response?.Content as ObjectContent;
        //    ResponseBase response = content?.Value as ResponseBase;
        //    customerHPIDUtils.Verify(x => x.ExecuteLogout(It.IsAny<ResponseBase>(), It.IsAny<int>(),
        //                            It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        //    Assert.AreEqual(response.ErrorList.Count, 2);
        //    Assert.IsTrue(response.ErrorList.First().ReturnCode.Equals("ServerIsBusy"));
        //    Assert.IsTrue(response.ErrorList.Last().ReturnCode.Equals("SessionTimeout"));
        //}

        //[TestMethod]
        //public void ActiveHPIDTokenAuth_OnAuthorization_Exception_LogoutEnabled()
        //{
        //    SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
        //    {
        //        new AdmSettings { ParamName = "LogoutOnNonAuthorizedRequest", NumValue = 1 }
        //    });

        //    using (ShimsContext.Create())
        //    {
        //        queryMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>())).Throws(new Exception());

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveSessionAuthorizationFilter activeSessionAuth = new ActiveSessionAuthorizationFilter();
        //        activeSessionAuth.OnAuthorization(context);
        //    }

        //    ObjectContent content = context?.Response?.Content as ObjectContent;
        //    ResponseBase response = content?.Value as ResponseBase;
        //    customerHPIDUtils.Verify(x => x.ExecuteLogout(It.IsAny<ResponseBase>(), It.IsAny<int>(),
        //                            It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        //    Assert.AreEqual(response.ErrorList.Count, 2);
        //    Assert.IsTrue(response.ErrorList.First().ReturnCode.Equals("ServerIsBusy"));
        //    Assert.IsTrue(response.ErrorList.Last().ReturnCode.Equals("SessionTimeout"));
        //}
    }
}
