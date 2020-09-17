
using InnovationPortalService;
using InnovationPortalService.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovationPortalServiceTests.Responses
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RESTAPIProfileResponseTests
    {
        private static string FirstName = "Jonh";
        private static string LastName = "Doe";
        private static string Email = "jdoe@does.com";
        private static string City = "Arlen";
        private static string PrimaryUse = "Item002";
        private static string Country = "US";
        private static string Language = "en";
        private static string Company = "Company";
        private static Tuple<string, bool> EmailConsent = new Tuple<string, bool>("Y", true);

        private GetProfileResponse setupTestObject(Fault f)
        {
            GetProfileResponse response = new GetProfileResponse();
            response.CustomerProfileObject = new CustomerProfile();
            response.CustomerProfileObject.FirstName = FirstName;
            response.CustomerProfileObject.LastName = LastName;
            response.CustomerProfileObject.EmailAddress = Email;
            response.CustomerProfileObject.City = City;
            response.CustomerProfileObject.EmailConsent = EmailConsent.Item1;
            response.CustomerProfileObject.PrimaryUse = PrimaryUse;
            response.CustomerProfileObject.Country = Country;
            response.CustomerProfileObject.Language = Language;
            response.CustomerProfileObject.CompanyName = Company;
            response.ErrorList = new HashSet<Fault>();
            if (f != null)
            {
                response.ErrorList.Add(f);
            }
            return response;

        }
        [TestMethod()]
        public void RESTAPIProfileResponseCastingNoErrorsTest()
        {
            GetProfileResponse response = setupTestObject(null);

            RESTAPIProfileResponse r = new RESTAPIProfileResponse(response);

            Assert.AreEqual(r.FirstName, FirstName);
            Assert.AreEqual(r.LastName, LastName);
            Assert.AreEqual(r.Email, Email);
            Assert.AreEqual(r.City, City);
            Assert.AreEqual(r.EmailOffers, EmailConsent.Item2);
            Assert.AreEqual(r.PrimaryUse, PrimaryUse);
            Assert.AreEqual(r.Country, Country);
            Assert.AreEqual(r.Language, Language);
            Assert.AreEqual(r.ErrorList.Count, 0);
            Assert.AreEqual(r.Company, Company);
        }
        [TestMethod()]
        public void RESTAPIProfileResponseCastingErrorsExistTest()
        {
            Fault fault = new Fault("o", "e", "m");
            GetProfileResponse response = setupTestObject(fault);

            RESTAPIProfileResponse r = new RESTAPIProfileResponse(response);
            Assert.AreEqual(r.FirstName, null);
            Assert.AreEqual(r.LastName, null);
            Assert.AreEqual(r.Email, null);
            Assert.AreEqual(r.City, null);
            Assert.AreEqual(r.EmailOffers, false);
            Assert.AreEqual(r.PrimaryUse, null);
            Assert.AreEqual(r.Country, null);
            Assert.AreEqual(r.Language, null);
            Assert.AreEqual(r.ErrorList.First(), fault);
            Assert.AreEqual(r.Company, null);
        }

    }

}






