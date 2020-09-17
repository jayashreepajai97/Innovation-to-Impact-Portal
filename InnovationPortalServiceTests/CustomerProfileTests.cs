using Microsoft.VisualStudio.TestTools.UnitTesting;
using InnovationPortalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using InnovationPortalService.HPPService;

namespace InnovationPortalServiceTests.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CustomerProfileTests
    {
        //[TestMethod()]
        //public void CustomerProfileTestPrimaryUseNullLanguageIsANumber()
        //{
        //    string FirstName = "firstName", LastName = "lastName", Email = "aaa@aa.com";
        //    string Language = "3", EmailConsent = "Y", UserName = "userName";
        //    string SegmentName = null;
            
        //    getUserResponse hppData = new getUserResponse();
        //    hppData.getUserResponseElement = new getUserResultType()
        //    {
        //        profileIdentity = new profileIdentityType()
        //        {
        //            userId = UserName
        //        },
        //        profileCore = new profileCoreType() {
        //            firstName = FirstName,
        //            email = Email,
        //            lastName = LastName,
        //            langCode = Language,
        //            contactPrefEmail = EmailConsent,
        //            segmentName = SegmentName
        //        },
        //        profileExtended = new profileExtendedType()
        //    };

        //    CustomerProfile cust = hppData.getUserResponseElement;

        //    Assert.IsTrue(cust.FirstName.CompareTo(FirstName) == 0);
        //    Assert.IsTrue(cust.LastName.CompareTo(LastName) == 0);
        //    Assert.IsTrue(cust.EmailAddress.CompareTo(Email) == 0);
        //    Assert.IsTrue(cust.EmailConsent.CompareTo(EmailConsent) == 0);
        //    Assert.IsTrue(cust.FirstName.CompareTo(FirstName) == 0);
        //    Assert.IsTrue(cust.UserName.CompareTo(UserName) == 0);
        //    Assert.IsNull(cust.PrimaryUse);
        //}

        //[TestMethod()]
        //public void CustomerProfileTestHome()
        //{
        //    string FirstName = "firstName", LastName = "lastName", Email = "aaa@aa.com";
        //    string Language = "en", EmailConsent = "Y", UserName = "userName";
        //    string SegmentName = "002", City = "city";
        //    string CountryCode = "US";

        //    getUserResponse hppData = new getUserResponse();
        //    hppData.getUserResponseElement = new getUserResultType()
        //    {
        //        profileIdentity = new profileIdentityType()
        //        {
        //            userId = UserName
        //        },
        //        profileCore = new profileCoreType()
        //        {
        //            firstName = FirstName,
        //            email = Email,
        //            lastName = LastName,
        //            langCode = Language,
        //            contactPrefEmail = EmailConsent,
        //            segmentName = SegmentName
        //        },
        //        profileExtended = new profileExtendedType()
        //        {
        //            homeCity = City,
        //            homeCountryCode = CountryCode
        //        }
        //    };

        //    CustomerProfile cust = hppData.getUserResponseElement;

        //    Assert.IsTrue(cust.FirstName.CompareTo(FirstName) == 0);
        //    Assert.IsTrue(cust.LastName.CompareTo(LastName) == 0);
        //    Assert.IsTrue(cust.EmailAddress.CompareTo(Email) == 0);
        //    Assert.IsTrue(cust.Language.CompareTo(Language) == 0);
        //    Assert.IsTrue(cust.EmailConsent.CompareTo(EmailConsent) == 0);
        //    Assert.IsTrue(cust.FirstName.CompareTo(FirstName) == 0);
        //    Assert.IsTrue(cust.UserName.CompareTo(UserName) == 0);
        //    Assert.IsTrue(cust.PrimaryUse.CompareTo("Item002") == 0);            
        //    Assert.IsTrue(cust.Country.CompareTo(CountryCode) == 0);
        //    Assert.IsTrue(cust.City.CompareTo(City) == 0);
        //}

        //[TestMethod()]
        //public void CustomerProfileTestBusiness()
        //{
        //    string FirstName = "firstName", LastName = "lastName", Email = "aaa@aa.com";
        //    string Language = "en", EmailConsent = "Y", UserName = "userName";
        //    string SegmentName = "003", City = "city", CompanyName = "CompanyNam";
        //    string CountryCode = "US";

        //    getUserResponse hppData = new getUserResponse();
        //    hppData.getUserResponseElement = new getUserResultType()
        //    {
        //        profileIdentity = new profileIdentityType()
        //        {
        //            userId = UserName
        //        },
        //        profileCore = new profileCoreType()
        //        {
        //            firstName = FirstName,
        //            email = Email,
        //            lastName = LastName,
        //            langCode = Language,
        //            contactPrefEmail = EmailConsent,
        //            segmentName = SegmentName
        //        },
        //        profileExtended = new profileExtendedType()
        //        {
        //            busCity = City,                    
        //            busCountryCode = CountryCode,
        //            busCompanyName = CompanyName,
        //        }
        //    };

        //    CustomerProfile cust = hppData.getUserResponseElement;

        //    Assert.IsTrue(cust.FirstName.CompareTo(FirstName) == 0);
        //    Assert.IsTrue(cust.LastName.CompareTo(LastName) == 0);
        //    Assert.IsTrue(cust.EmailAddress.CompareTo(Email) == 0);
        //    Assert.IsTrue(cust.Language.CompareTo(Language) == 0);
        //    Assert.IsTrue(cust.EmailConsent.CompareTo(EmailConsent) == 0);
        //    Assert.IsTrue(cust.FirstName.CompareTo(FirstName) == 0);
        //    Assert.IsTrue(cust.UserName.CompareTo(UserName) == 0);
        //    Assert.IsTrue(cust.PrimaryUse.CompareTo("Item003") == 0);
        //    Assert.IsTrue(cust.Country.CompareTo(CountryCode) == 0);
        //    Assert.IsTrue(cust.City.CompareTo(City) == 0);
        //    Assert.IsTrue(cust.CompanyName.CompareTo(CompanyName) == 0);
        //}
    }
}