using Filters;
using InnovationPortalService.Controllers;
using InnovationPortalService.Requests;
using InnovationPortalService.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Responses;
using SettingsRepository;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace InnovationPortalServiceTests.Filters
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ApiLoggerFilterTests
    {
        HttpActionExecutedContext context; //= new HttpActionExecutedContext();

        [TestInitialize]
        public void Init()
        {
            string requestStr = "{\"DeviceProperty\": {\"SerialNumber\": \"CN35E25H9805VC\"}}";
            ResponseBase response = new ResponseBase()
            {
                ErrorList = new HashSet<Fault>()
            };
            response.ErrorList.Add(Faults.LegacyProductError);

     
            context = new HttpActionExecutedContext()
            {
                ActionContext = new HttpActionContext()
                {
                    ControllerContext = new HttpControllerContext()
                    {
                        Request = new HttpRequestMessage()
                        {
                            Method = HttpMethod.Post,
                            RequestUri = new Uri("http://localhost:33972/GetSSDeviceInfo"),
                            Content = new StringContent(requestStr, Encoding.UTF8, "application/json"),
                        }
                    },
                    Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new ObjectContent(typeof(ResponseBase), response, new JsonMediaTypeFormatter())
                    },
                    ActionDescriptor = new ReflectedHttpActionDescriptor()
                    {
                        ActionBinding = new HttpActionBinding()                       
                    }
                }
            };
            
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
            {
                new AdmSettings { ParamName = "LogAPICalls", StringValue = "Faults" }
            });
        }

        //[TestMethod]
        //public void ApiLogger_OnActionExecutedTest_SkipLogging()
        //{

        //    ApiLoggerFilter apiLoggerFilter = new ApiLoggerFilter();
        //    apiLoggerFilter.OnActionExecuted(context);

        //    Assert.AreEqual(System.Net.HttpStatusCode.OK, context.Response.StatusCode);
        //}


        //[TestMethod]
        //public void ApiLogger_OnActionExecutedTest_NotSkipLogging()
        //{
        //    ResponseBase response = new ResponseBase()
        //    {
        //        ErrorList = new HashSet<Fault>()
        //    };
        //    response.ErrorList.Add(Faults.SerialNumberNotRecognized);

        //    context.ActionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        //    {
        //        Content = new ObjectContent(typeof(ResponseBase), response, new JsonMediaTypeFormatter())
        //    };


        //    ApiLoggerFilter apiLoggerFilter = new ApiLoggerFilter();
        //    apiLoggerFilter.OnActionExecuted(context);


        //    Assert.AreEqual(System.Net.HttpStatusCode.OK, context.Response.StatusCode);
        //}
    }
}
