using Microsoft.VisualStudio.TestTools.UnitTesting;
using InnovationPortalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InnovationPortalService.Responses;
using Responses;

namespace InnovationPortalServiceTests.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RESTAPICustomerDataTests
    {
        [TestMethod()]
        public void CustomerData()
        {
            string Country = "US";
            string Language = "ro";
            string FirstName = "firstName";
            string LastName = "lastName";
            bool EmailOffers = true;
            string PrimaryUse = "Item002";
            string City = "city";
            string Company = "Company";
            bool ActiveHealth = true;

            RESTAPICustomerData register = new RESTAPICustomerData()
            {
                Country = Country,
                Language = Language,
                FirstName = FirstName,
                LastName = LastName,
                EmailOffers = EmailOffers,
                PrimaryUse = PrimaryUse,
                City = City,
                Company = Company,
                ActiveHealth = ActiveHealth

            };
            CustomerData ret = (CustomerData)register;

            Assert.AreEqual(ret.EmailAddress, null);
            Assert.AreEqual(ret.Country, Country);
            Assert.AreEqual(ret.Language, Language);
            Assert.AreEqual(ret.FirstName, FirstName);
            Assert.AreEqual(ret.LastName, LastName);
            Assert.AreEqual(ret.PrimaryUse, PrimaryUse);
            Assert.AreEqual(ret.City, City);
            Assert.AreEqual(ret.CompanyName, Company);
            Assert.AreEqual(ret.EmailConsent == "Y", EmailOffers);
            Assert.AreEqual(ret.ActiveHealth, ActiveHealth);
        }

        [TestMethod()]
        public void CustomerDataTest_InvalidCompanyName()
        {
            UpdateProfileResponse r = new UpdateProfileResponse();

            string Country = "US";
            string Language = "ro";
            string FirstName = "firstName";
            string LastName = "lastName";
            bool EmailOffers = true;
            string PrimaryUse = "Item005";
            string City = "city";
            string Company = ".HP Inc";
            bool ActiveHealth = true;

            RESTAPICustomerData requestCustData = new RESTAPICustomerData()
            {
                Country = Country,
                Language = Language,
                FirstName = FirstName,
                LastName = LastName,
                EmailOffers = EmailOffers,
                PrimaryUse = PrimaryUse,
                City = City,
                Company = Company,
                ActiveHealth = ActiveHealth
            };

            ResponseBase response = new ResponseBase();
            Assert.IsFalse(requestCustData.IsValid(response));
            Assert.AreEqual(response.ErrorList.Count, 1);
            Assert.AreEqual(response.ErrorList.Where(x => x.DebugStatusText.Contains("Field has invalid format")).Count(), 1);
        }

        [TestMethod()]
        public void CustomerDataTest_Success()
        {
            UpdateProfileResponse r = new UpdateProfileResponse();

            string Country = "US";
            string Language = "ro";
            string FirstName = "firstName";
            string LastName = "lastName";
            bool EmailOffers = true;
            string PrimaryUse = "Item002";
            string City = "city";
            string Company = "HP Inc";
            bool ActiveHealth = true;

            RESTAPICustomerData requestCustData = new RESTAPICustomerData()
            {
                Country = Country,
                Language = Language,
                FirstName = FirstName,
                LastName = LastName,
                EmailOffers = EmailOffers,
                PrimaryUse = PrimaryUse,
                City = City,
                Company = Company,
                ActiveHealth = ActiveHealth
            };

            ResponseBase response = new ResponseBase();
            Assert.IsTrue(requestCustData.IsValid(response));
            Assert.AreEqual(response.ErrorList.Count, 0);
        }
    }
}