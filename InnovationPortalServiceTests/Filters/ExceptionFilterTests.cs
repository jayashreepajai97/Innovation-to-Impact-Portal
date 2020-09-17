using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filters.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ExceptionFilterTests
    {
        [TestMethod()]
        public void OnExceptionTest()
        {
            //using (ShimsContext.Create())
            //{
            //    HttpResponseMessage m = new HttpResponseMessage();
            //    HttpRequestMessage req = new HttpRequestMessage();

            //    ShimHttpActionExecutedContext.AllInstances.ExceptionGet = (x) =>
            //        { return new HttpResponseException(HttpStatusCode.UnsupportedMediaType); };
            //    ShimHttpActionExecutedContext.AllInstances.RequestGet = (x) =>
            //        { return req; };
            //    ShimHttpActionExecutedContext.AllInstances.ResponseSetHttpResponseMessage = (x, y) =>
            //        { m = y; };

            //    ExceptionFilter ef = new ExceptionFilter();
            //    ef.OnException(new System.Web.Http.Filters.HttpActionExecutedContext());
            //    ResponseBase resp;
            //    m.TryGetContentValue<ResponseBase>(out resp);
            //    Assert.IsTrue(resp.ErrorList.Contains(Faults.InvalidContentType));

            //    ShimHttpActionExecutedContext.AllInstances.ExceptionGet = (x) =>
            //    { return new System.Exception("asdf"); };

            //    ef.OnException(new System.Web.Http.Filters.HttpActionExecutedContext());
            //    m.TryGetContentValue<ResponseBase>(out resp);
            //    Assert.IsTrue((resp.ErrorList.First()).ReturnCode.Equals("UnknownError"));
            //}            
        }
    }
}