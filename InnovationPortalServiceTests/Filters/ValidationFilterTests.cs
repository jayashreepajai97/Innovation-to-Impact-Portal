using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Http.Controllers;
using Moq;
using Hpcs.DependencyInjector;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using IdeaDatabase.Validation;

namespace Filters.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ValidationFilterTests
    {
        private static Mock<IDbFieldsConstraints> dbfc;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext tc)
        {
            dbfc = new Mock<IDbFieldsConstraints>();
            DependencyInjector.Register(dbfc.Object).As<IDbFieldsConstraints>();
        }

        //[TestMethod()]
        //public void OnActionExecutingTest()
        //{
        //    DummyConstraints dc = new DummyConstraints();
        //    dc.StringSingleConstraints("Browsers", "Name", 256, false, 0);
        //    dbfc.Setup(x => x.Constraints).Returns(dc.GetConstraints());

        //    using (ShimsContext.Create())
        //    {
        //        HttpResponseMessage m = new HttpResponseMessage();
        //        HttpRequestMessage req = new HttpRequestMessage();
                
        //        ModelStateDictionary msList = new ModelStateDictionary();
        //        msList.AddModelError("error", new Exception("dummy"));                

        //        ShimHttpActionContext.AllInstances.RequestGet = (x) =>
        //        { return req; };
        //        ShimHttpActionContext.AllInstances.ResponseSetHttpResponseMessage = (x, y) =>
        //        { m = y; };
        //        ShimHttpActionContext.AllInstances.ModelStateGet = (x) =>
        //        { return msList; };
                
        //        ValidationFilter vf = new ValidationFilter();

        //        HttpActionContext ac = new HttpActionContext();                
        //        ac.ActionArguments.Add("req", "no content");
        //        vf.OnActionExecuting(ac);
                
        //        ResponseBase resp = new ResponseBase();
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("InvalidJSON"));


        //        m = new HttpResponseMessage();
        //        req = new HttpRequestMessage();
        //        req.Method = HttpMethod.Post;
        //        ModelStateDictionary msListValid = new ModelStateDictionary();

        //        ShimHttpActionContext.AllInstances.RequestGet = (x) =>
        //        { return req; };
        //        ShimHttpActionContext.AllInstances.ResponseSetHttpResponseMessage = (x, y) =>
        //        { m = y; };
        //        ShimHttpActionContext.AllInstances.ModelStateGet = (x) =>
        //        { return msListValid; };

        //        vf = new ValidationFilter();
        //        ac = new HttpActionContext();
        //        ac.ActionArguments.Add("req", null);
        //        ac.ActionDescriptor = new TestReflectedHttpActionDescriptor();
                
        //        vf.OnActionExecuting(ac);

        //        resp = new ResponseBase();
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("MissingRequestContent"));



        //        vf = new ValidationFilter();
        //        ac = new HttpActionContext();
        //        ac.ActionArguments.Add("req", "no content");
        //        vf.OnActionExecuting(ac);
        //        resp = new ResponseBase();
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("MissingRequestContent"));


                           
        //    }
        //}

        //[TestMethod()]
        //public void OnActionExecutingTestLoginCredentials()
        //{
        //    DummyConstraints dc = new DummyConstraints();
        //    dc.StringSingleConstraints("UserAuthentications", "CallerId", 255, true, 1);
        //    dbfc.Setup(x => x.Constraints).Returns(dc.GetConstraints());

        //    using (ShimsContext.Create())
        //    {
        //        HttpResponseMessage m = new HttpResponseMessage();
        //        HttpRequestMessage req = new HttpRequestMessage();
        //        req.Method = HttpMethod.Post;

        //        ValidationFilter vf = new ValidationFilter();
        //        HttpActionContext ac = new HttpActionContext();               
        //        ResponseBase resp = new ResponseBase();
               
        //        ModelStateDictionary msListValid = new ModelStateDictionary();
        //        ShimHttpActionContext.AllInstances.RequestGet = (x) =>
        //        { return req; };
        //        ShimHttpActionContext.AllInstances.ResponseSetHttpResponseMessage = (x, y) =>
        //        { m = y; };
        //        ShimHttpActionContext.AllInstances.ModelStateGet = (x) =>
        //        { return msListValid; };
                
        //        vf = new ValidationFilter();
        //        ac = new HttpActionContext();
        //        resp = new ResponseBase();
        //        LoginCredentials loginCredentials = new LoginCredentials(); 
        //        ac.ActionArguments.Add("req", loginCredentials);
        //        vf.OnActionExecuting(ac);
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("FieldValidationError"));

        //        vf = new ValidationFilter();
        //        ac = new HttpActionContext();
        //        resp = new ResponseBase();
        //        loginCredentials = new LoginCredentials()
        //        {
        //            UserName = null,
        //            Password = "",
        //            CallerId = ""
        //        };
        //        ac.ActionArguments.Add("req", loginCredentials);
        //        vf.OnActionExecuting(ac);
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("FieldValidationError"));

        //        vf = new ValidationFilter();
        //        ac = new HttpActionContext();
        //        resp = new ResponseBase();
        //        loginCredentials = new LoginCredentials()
        //        {
        //            UserName = "",
        //            Password = "password",
        //            CallerId = "callerId"
        //        };
        //        ac.ActionArguments.Add("req", loginCredentials);
        //        vf.OnActionExecuting(ac);                
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("FieldValidationError"));

        //        //vf = new ValidationFilter();
        //        //ac = new HttpActionContext();
        //        //resp = new ResponseBase();
        //        //loginCredentials = new LoginCredentials() { UserName = "userName", Password = "password", CallerId = "callerIdToLongForVerification" };
        //        //ac.ActionArguments.Add("req", loginCredentials);
        //        //vf.OnActionExecuting(ac);                
        //        //m.TryGetContentValue<ResponseBase>(out resp);
        //        //Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("FieldValidationError"));
        //    }
        //}

        //[TestMethod()]
        //public void OnActionExecutingTestAccessCredentials()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        Mock<IQueryUtils> query = new Mock<IQueryUtils>();
        //        DependencyInjector.Register(query.Object).As<IQueryUtils>();

        //        DummyConstraints dc = new DummyConstraints();
        //        dc.StringSingleConstraints("UserAuthentications", "CallerId", 255, true, 1);
        //        dbfc.Setup(x => x.Constraints).Returns(dc.GetConstraints());

        //        UserAuthentication token = new UserAuthentication() { Token = "token"};
        //        query.Setup(f => f.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(token);
        //        HttpResponseMessage m = new HttpResponseMessage();
        //        HttpRequestMessage req = new HttpRequestMessage();
        //        req.Method = HttpMethod.Post;
        //        ValidationFilter vf = new ValidationFilter();
        //        HttpActionContext ac = new HttpActionContext();
        //        ResponseBase resp = new ResponseBase();

        //        ModelStateDictionary msListValid = new ModelStateDictionary();
        //        ShimHttpActionContext.AllInstances.RequestGet = (x) =>
        //        { return req; };
        //        ShimHttpActionContext.AllInstances.ResponseSetHttpResponseMessage = (x, y) =>
        //        { m = y; };
        //        ShimHttpActionContext.AllInstances.ModelStateGet = (x) =>
        //        { return msListValid; };

        //        AccessCredentials accessCredentials = new AccessCredentials() { SessionToken ="asdf"};

        //        token = null;
        //        query.Setup(f => f.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(token);

        //        vf = new ValidationFilter();
        //        ac = new HttpActionContext();
        //        accessCredentials = new AccessCredentials() { UserID = 0, SessionToken = "", CallerId = "" };
        //        ac.ActionArguments.Add("req", accessCredentials);
        //        vf.OnActionExecuting(ac);
        //        resp = new ResponseBase();
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("FieldValidationError"));

        //        vf = new ValidationFilter();
        //        ac = new HttpActionContext();
        //        accessCredentials = new AccessCredentials() { UserID = 0, SessionToken = "sessionToken", CallerId = "" };
        //        ac.ActionArguments.Add("req", accessCredentials);
        //        vf.OnActionExecuting(ac);
        //        resp = new ResponseBase();
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(resp.ErrorList.First().ReturnCode.Equals("FieldValidationError"));                
        //    }
        //}

        //[TestMethod()]
        //public void OnActionExecutingTestLocaleCredentials()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        Mock<IIdeaDatabaseDataContext> dbdc = new Mock<IIdeaDatabaseDataContext>();
        //        DependencyInjector.Register(dbdc.Object).As<IIdeaDatabaseDataContext>();
        //        Mock<IQueryUtils> query = new Mock<IQueryUtils>();                
        //        DependencyInjector.Register(query.Object).As<IQueryUtils>();

        //        DummyConstraints dc = new DummyConstraints();
        //        dc.StringSingleConstraints("UserAuthentications", "CallerId", 255, true, 1);
        //        dbfc.Setup(x => x.Constraints).Returns(dc.GetConstraints());

        //        HttpResponseMessage m = new HttpResponseMessage();
        //        HttpRequestMessage req = new HttpRequestMessage();
        //        req.Method = HttpMethod.Post;
        //        ResponseBase resp = new ResponseBase();

        //        ModelStateDictionary msListValid = new ModelStateDictionary();
        //        ShimHttpActionContext.AllInstances.RequestGet = (x) =>
        //        { return req; };
        //        ShimHttpActionContext.AllInstances.ResponseSetHttpResponseMessage = (x, y) =>
        //        { m = y; };
        //        ShimHttpActionContext.AllInstances.ModelStateGet = (x) =>
        //        { return msListValid; };

        //        UserAuthentication auth = new UserAuthentication() { LanguageCode = "pl", CountryCode = "PL" };
        //        query.Setup(f => f.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(auth);
        //        ValidationFilter vf = new ValidationFilter();
        //        HttpActionContext ac = new HttpActionContext();
        //        AccessCredentials accessCredentials = new AccessCredentials() { UserID = 1, SessionToken = "sessionToken", CallerId = "callerId" };
        //        ac.ActionArguments.Add("req", accessCredentials);
        //        vf.OnActionExecuting(ac);
        //        resp = new ResponseBase();
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(accessCredentials.CountryCode.Equals("PL"));
        //        Assert.IsTrue(accessCredentials.LanguageCode.Equals("pl"));

        //        auth = new UserAuthentication() { LanguageCode = null, CountryCode = null };
        //        query.Setup(f => f.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(auth);
        //        vf = new ValidationFilter();
        //        ac = new HttpActionContext();
        //        accessCredentials = new AccessCredentials() { UserID = 1, SessionToken = "sessionToken", CallerId = "callerId" };
        //        ac.ActionArguments.Add("req", accessCredentials);
        //        vf.OnActionExecuting(ac);
        //        m.TryGetContentValue<ResponseBase>(out resp);
        //        Assert.IsTrue(accessCredentials.CountryCode.Equals("US"));
        //        Assert.IsTrue(accessCredentials.LanguageCode.Equals("en"));
        //    }
        //}
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TestReflectedHttpActionDescriptor : HttpActionDescriptor
    {
        private Collection<Object> parameters = new Collection<Object>();

        public override string ActionName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Type ReturnType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Task<object> ExecuteAsync(HttpControllerContext controllerContext, IDictionary<string, object> arguments, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Collection<HttpParameterDescriptor> GetParameters()
        {
            throw new NotImplementedException();
        }

        public override Collection<T> GetCustomAttributes<T>()
        {
            Collection<T> collection = new Collection<T>();
            foreach(var obj in parameters)
                if(obj is T)
                    collection.Add((T)obj);

            return collection;
        }

        public void SetCustomAttributes<T>(Collection<T> list)
        {
            parameters.Clear();
            foreach (var obj in list)
                parameters.Add(obj);
        }
    }
}