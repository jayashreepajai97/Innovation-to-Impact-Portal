using InnovationPortalService.HPID;
using InnovationPortalService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Controllers;

namespace InnovationPortalServiceTests.Filters
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ActiveHPIDTokenAuthorizationFilterTests
    {
        //HttpActionContext context; //= new HttpActionExecutedContext();
        //Mock<IAUTHUtils> hpidAuthUtils;
        //Mock<ICustomerHPIDUtils> customerHPIDUtils;
  
    
        //[TestInitialize]
        //public void Init()
        //{
        //    ShimsContext.Create();

        //    hpidAuthUtils = new Mock<IAUTHUtils>();
        //    DependencyInjector.Register(hpidAuthUtils.Object).As<IAUTHUtils>();
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

        //}

        //[TestMethod]
        //public void ActiveHPIDTokenAuth_OnAuthorization_NotAuthorized()
        //{
        //    ShimsContext.Create();
        //    using (ShimsContext.Create())
        //    {
        //        hpidAuthUtils.Setup(x => x.GetProfile(It.IsAny<string>())).Returns<HPIDCustomerProfile>(null);

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveHPIDTokenAuthorizationFilter activeHPIDAuth = new ActiveHPIDTokenAuthorizationFilter();
        //        activeHPIDAuth.OnAuthorization(context);
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
        //        hpidAuthUtils.Setup(x => x.GetProfile(It.IsAny<string>())).Returns(new HPIDCustomerProfile());

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveHPIDTokenAuthorizationFilter activeHPIDAuth = new ActiveHPIDTokenAuthorizationFilter();
        //        activeHPIDAuth.OnAuthorization(context);
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
        //        hpidAuthUtils.Setup(x => x.GetProfile(It.IsAny<string>())).Throws(new Exception());

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveHPIDTokenAuthorizationFilter activeHPIDAuth = new ActiveHPIDTokenAuthorizationFilter();
        //        activeHPIDAuth.OnAuthorization(context);
        //    }

        //    ObjectContent content = context?.Response?.Content as ObjectContent;
        //    ResponseBase response = content?.Value as ResponseBase;
        //    customerHPIDUtils.Verify(x => x.ExecuteLogout(It.IsAny<ResponseBase>(), It.IsAny<int>(),
        //                            It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        //    Assert.AreEqual(response.ErrorList.Count, 1);
        //    Assert.IsTrue(response.ErrorList.First().ReturnCode.Equals("SessionTimeout"));
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
        //        hpidAuthUtils.Setup(x => x.GetProfile(It.IsAny<string>())).Throws(new Exception());

        //        System.Web.Http.Fakes.ShimAuthorizeAttribute.AllInstances.OnAuthorizationHttpActionContext = (info, context) => { };

        //        ActiveHPIDTokenAuthorizationFilter activeHPIDAuth = new ActiveHPIDTokenAuthorizationFilter();
        //        activeHPIDAuth.OnAuthorization(context);
        //    }

        //    ObjectContent content = context?.Response?.Content as ObjectContent;
        //    ResponseBase response = content?.Value as ResponseBase;
        //    customerHPIDUtils.Verify(x => x.ExecuteLogout(It.IsAny<ResponseBase>(), It.IsAny<int>(),
        //                            It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        //    Assert.AreEqual(response.ErrorList.Count, 1);
        //    Assert.IsTrue(response.ErrorList.First().ReturnCode.Equals("SessionTimeout"));
        // }
    }
}
