using System;
using Responses;
using System.Linq;
using Responses.Enums;
using System.Collections.Generic;
using InnovationPortalService.Responses;
//using InnovationPortalService.HPPService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InnovationPortalService;

namespace InnovationPortalServiceTests.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ExceptionMappingTests
    {
        //[TestMethod()]
        //public void MapHPPGenericFaultsTest()
        //{
        //    ExceptionMapping m = new ExceptionMapping();
        //    ResponseBase b = new ResponseBase();

        //    // Two faults with rules that should be ignored
        //    InnovationPortalService.HPPService.genericFaultType f1 = new  InnovationPortalService.HPPService.genericFaultType();
        //    f1.ruleNumber = 473;
        //     InnovationPortalService.HPPService.genericFaultType f2 = new  InnovationPortalService.HPPService.genericFaultType();
        //    f2.ruleNumber = 807;

        //    // 2 faults that should be reported
        //     InnovationPortalService.HPPService.genericFaultType f3 = new  InnovationPortalService.HPPService.genericFaultType();
        //    f3.ruleNumber = 1;
        //     InnovationPortalService.HPPService.genericFaultType f4 = new  InnovationPortalService.HPPService.genericFaultType();
        //    f4.ruleNumber = 2;

        //    //SessionTimeout
        //     InnovationPortalService.HPPService.genericFaultType f5 = new  InnovationPortalService.HPPService.genericFaultType();
        //    f5.ruleNumber = 235;

        //     InnovationPortalService.HPPService.genericFaultType[] emptyList = { };
        //     InnovationPortalService.HPPService.genericFaultType[] faultsToSkip = { f1, f2 };
        //     InnovationPortalService.HPPService.genericFaultType[] faultsToReport = { f3, f4 };
        //     InnovationPortalService.HPPService.genericFaultType[] mixedList = { null, f1, f3, null, f4, f2, null, null, f5 };
        //    // null checks
        //    try
        //    {
        //        m.MapHPPGenericFaults(null, null); // 4 nulls are ok, since error list is empty ResponseBase will not be reached
        //    }
        //    catch (Exception)
        //    {
        //        Assert.Fail();
        //    }

        //    try
        //    {
        //        m.MapHPPGenericFaults(null, faultsToReport);
        //        Assert.Fail(); // exception should be thrown
        //    }
        //    catch (NullReferenceException)
        //    {
        //        // should get here
        //    }
        //    catch (Exception)
        //    {
        //        Assert.Fail(); // Should be caught by previous catch
        //    }

        //    //empty list check - just to be sure if handled correctly
        //    b = new ResponseBase();
        //    m.MapHPPGenericFaults(b, emptyList);
        //    Assert.IsTrue(b.ErrorList.Count == 0);

        //    // check if faults expected to be ignored are ignored
        //    b = new ResponseBase(); // reset the response
        //    m.MapHPPGenericFaults(b, faultsToSkip);
        //    Assert.IsTrue(b.ErrorList.Count == 0);


        //    // check if faults expected to be reported are reported
        //    b = new ResponseBase(); // reset the response
        //    m.MapHPPGenericFaults(b, faultsToReport);
        //    Assert.IsTrue(b.ErrorList.Count == 2);

        //    // mix everything
        //    b = new ResponseBase(); // reset the response
        //    m.MapHPPGenericFaults(b, mixedList);
        //    Assert.IsTrue(b.ErrorList.Count == 3);
        //    Assert.IsTrue(b.ErrorList.Where(x => x.ErrorCategory == ErrorCategory.SessionTimeout).Count() == 1);

        //    //Test specific HPP error codes separately
        //    foreach (var error in Faults.HppErrorMap)
        //    {
        //         InnovationPortalService.HPPService.genericFaultType fault = new  InnovationPortalService.HPPService.genericFaultType();
        //        fault.ruleNumber = error.Key;
        //        b = new ResponseBase(); // reset the response
        //         InnovationPortalService.HPPService.genericFaultType[] Faultlist = { fault };
        //        m.MapHPPGenericFaults(b, Faultlist);
        //        Assert.IsTrue(error.Value != null);
        //        Assert.IsTrue(b.ErrorList.Contains(error.Value));
        //    }

        //    //Test specific HPP error codes alltogether in one list
        //    b = new ResponseBase(); // reset the response

        //    foreach (var error in Faults.HppErrorMap)
        //    {//get all errors to one ErrorList
        //         InnovationPortalService.HPPService.genericFaultType fault = new  InnovationPortalService.HPPService.genericFaultType();
        //        fault.ruleNumber = error.Key;
        //         InnovationPortalService.HPPService.genericFaultType[] Faultlist = { fault };
        //        m.MapHPPGenericFaults(b, Faultlist);
        //    }

        //    b.ErrorList.Reverse();//lets make it interesting
        //    foreach (var error in Faults.HppErrorMap)
        //    {
        //        Assert.IsTrue(error.Value != null);
        //        Assert.IsTrue(b.ErrorList.Contains(error.Value));
        //    }

        //    // test for error 237
        //    genericFaultType f237 = new genericFaultType();
        //    f237.ruleNumber = 237;
        //    genericFaultType[] serviceError = { f237 };
        //    b = new ResponseBase();
        //    m.MapHPPGenericFaults(b, serviceError);
        //    Assert.IsTrue(b.ErrorList.Count == 1);
        //}

        //[TestMethod()]
        //public void MapHPPGenericFaultsTestGenericError()
        //{
        //    ExceptionMapping m = new ExceptionMapping();
        //    ResponseBase b = new ResponseBase();

        //    genericFaultType fault = new genericFaultType();
        //    fault.ruleNumber = 238;
        //    genericFaultType[] serviceError = { fault };
        //    b = new ResponseBase();
        //    m.MapHPPGenericFaults(b, serviceError);
        //    Assert.IsTrue(b.ErrorList.Count == 1);
        //    Assert.IsTrue(b.ErrorList.First().ReturnCode.CompareTo(Faults.GenericError.ReturnCode) == 0);
        //}

        [TestMethod()]
        public void MapReturnCodeTest()
        {

            ResponseBase r = new ResponseBase();
            int errorCount = 0;

            ExceptionMapping m = new ExceptionMapping();

            // Check for unknown error
            m.MapReturnCode("dummyString", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.UnMappedError));

            // 
            m.MapReturnCode("<ns:FileldID>userId</ns:FileldID><ns:Desc>User ID contains invalid special characters.</ns:Desc>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);

            // 
            m.MapReturnCode("><ns:FileldID>password</ns:FileldID><ns:Desc>Password cannot be the same as User ID.</ns:Desc>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.MustUseDiffPasswordOrUserName));



            // Check for existing email
            m.MapReturnCode("<ns:FileldID>email</ns:FileldID>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.EmailExists));

            // Check for ??? user id - not sure when this error occurs
            m.MapReturnCode("<fieldName>userId</fieldName><code>field.same</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.UserNameIsTheSame));

            // Check for same password
            m.MapReturnCode("<fieldName>password</fieldName><code>field.same</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.PasswordIsTheSame));

            // Check for short password
            m.MapReturnCode("<fieldName>password</fieldName><code>field.short</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.PasswordTooShort));


            // Check for email not found
            m.MapReturnCode("<fieldName>email</fieldName><code>field.nomatch</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.EmailAddressNotFound));


            r.ErrorList = new HashSet<Fault>();
            errorCount = 0;
            // Check for profileId field missing
            m.MapReturnCode("<fieldName>profileId</fieldName><code>field.notfound</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            //Assert.IsTrue(r.ErrorList.Contains(Faults.ProfileIdNotFound));
            Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCredentials));

            // Check for reset guid not found
            m.MapReturnCode("<fieldName>guid</fieldName><code>field.notfound</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.ResetGuidNotFound));

            // Check for bad user or password
            r.ErrorList = new HashSet<Fault>();
            errorCount = 0;
            m.MapReturnCode("HPP_CUSTOMER_001", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            //Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidUserIdOrPwd));
            Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCredentials));

            m.MapReturnCode("HPP_CUSTOMER_001 HPP Authentication Service not available", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.AuthServiceNotAvailable));

            // Check for bad token
            r.ErrorList = new HashSet<Fault>();
            errorCount = 0;
            m.MapReturnCode("<ns:FaultCode>token.invalid</ns:FaultCode>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCredentials));

            // Check for product not found
            m.MapReturnCode("<ns:FaultDesc>Customer or Product not found</ns:FaultDesc>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.ProductNotFound));

            // Check for bad current pwd
            r.ErrorList = new HashSet<Fault>();
            errorCount = 0;
            m.MapReturnCode("<ruleNumber>422</ruleNumber><fieldName>currentPassword</fieldName>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            //Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCurrentPassword));
            Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCredentials));


            //Reset Everything and check second options since some Faults has two (or more possibilities
            r = new ResponseBase();
            errorCount = 0;

            // Check for user id exist
            m.MapReturnCode("<fieldName>userId</fieldName><code>field.duplicate</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.UserIdExists));

            // Check for short passwd
            m.MapReturnCode("<ns:FileldID>password</ns:FileldID><ns:Desc>Password must contain at least 6 characters.</ns:Desc>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.PasswordTooShort));

            // Check for userId not found
            m.MapReturnCode("<ruleNumber>228</ruleNumber><fieldName>userId</fieldName>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            //Assert.IsTrue(r.ErrorList.Contains(Faults.UserIdNotFound));
            Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCredentials));

            // Check for token invalid
            r.ErrorList = new HashSet<Fault>();
            errorCount = 0;
            m.MapReturnCode("<ruleNumber>235</ruleNumber><fieldName>changeUserID</fieldName><code>token.expired</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCredentials));

            r.ErrorList = new HashSet<Fault>();
            errorCount = 0;
            m.MapReturnCode("<fieldName>userId</fieldName><code>field.nomatch</code>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            //Assert.IsTrue(r.ErrorList.Contains(Faults.UserIdNotFound));
            Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCredentials));

            // Check for HPP internal error
            r.ErrorList = new HashSet<Fault>();
            errorCount = 0;
            m.MapReturnCode("<ns:FaultCode>BW-HTTP-100300</ns:FaultCode>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.HPPInternalError));

            // Check for HPP internal error
            r.ErrorList = new HashSet<Fault>();
            errorCount = 0;
            m.MapReturnCode("ns:FaultDesc>Technical Problem has occurred while serving the request</ns:FaultDesc>", r);
            Assert.IsTrue(r.ErrorList.Count == ++errorCount);
            Assert.IsTrue(r.ErrorList.Contains(Faults.HPPInternalError));
        }

        [TestMethod()]
        public void MapGenericFaultsTest()
        {
            ResponseBase r = new ResponseBase();
            ExceptionMapping em = new ExceptionMapping();

            em.MapGenericFaults(r, null);
            Assert.IsTrue(r.ErrorList.Count == 0);

            em.MapGenericFaults(r, "");
            Assert.IsTrue(r.ErrorList.Count == 0);

            em.MapGenericFaults(r, "<ns:ErrorInfo xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:ns=\"http://isac.hp.com/schema/registrations/CustomeErrorSchema.xsd\" xmlns:ns0=\"http://www.w3.org/2003/05/soap-envelope\"><ns:HppFaultDetails><ns:SystemFault><ns:MsgCode>HPP_CUSTOMER_001</ns:MsgCode><ns:Desc>userid or password is wrongly keyed</ns:Desc></ns:SystemFault></ns:HppFaultDetails></ns:ErrorInfo>");
            Assert.IsTrue(r.ErrorList.Contains(Faults.InvalidCredentials));

            r = new ResponseBase();
            em.MapGenericFaults(r, "<ns:ErrorInfo xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:ns=\"http://isac.hp.com/schema/registrations/CustomeErrorSchema.xsd\" xmlns:ns0=\"http://www.w3.org/2003/05/soap-envelope\"><ns:HppFaultDetails><ns:SystemFault><ns:MsgCode>H</ns:MsgCode><ns:Desc>P</ns:Desc></ns:SystemFault></ns:HppFaultDetails></ns:ErrorInfo>");
            Assert.IsTrue(r.ErrorList.Count == 0);

            r = new ResponseBase();
            em.MapGenericFaults(r, "<ns:ErrorInfo xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:ns=\"http://isac.hp.com/schema/registrations/CustomeErrorSchema.xsd\" xmlns:ns0=\"http://www.w3.org/2003/05/soap-envelope\"><ns:HppFaultDetails><ns:SystemFault><ns:MsgCode></ns:MsgCode><ns:Desc></ns:Desc></ns:SystemFault></ns:HppFaultDetails></ns:ErrorInfo>");
            Assert.IsTrue(r.ErrorList.Count == 0);

        }
    }
}