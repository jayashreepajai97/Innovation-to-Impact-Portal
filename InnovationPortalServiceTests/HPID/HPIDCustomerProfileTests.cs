using System;
using Newtonsoft.Json;
using InnovationPortalService;
using InnovationPortalService.HPID;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InnovationPortalServiceTests.HPID
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HPIDCustomerProfileTests
    {
        [TestMethod()]
        public void HPIDCustomerProfileTests_Ok()
        {
            CustomerProfile CustProfile = new CustomerProfile()
            {
                CompanyName = "CompanyName",
                Country = "US",
                Language = "en",
                FirstName = "FirstName",
                LastName = "LastName",
                EmailAddress = "new@mail.com",
                City = "New",
                ActiveHealth = true,
                DisplayName = "DisplayName",
                EmailConsent = "Y",
                Gender = "F",
                Id = "asdsa",
                PrimaryUse = "Item002",
                Addresses = new System.Collections.Generic.List<Address>()
                {
                    new Address()
                    {
                        Country = "US",
                        Type = AddressType.home.ToString(),
                        Role = "other",
                        Primary = true
                    }
                },
                PhoneNumbers = new System.Collections.Generic.List<PhoneNumber>()
                {
                    new PhoneNumber()
                    {
                        Primary = true,
                        CountryCode = "US",
                        Type = AddressType.home.ToString(),
                    }
                }
            };

            HPIDCustomerProfile hpapiProfile = new HPIDCustomerProfile(CustProfile);

            Assert.AreEqual(hpapiProfile.gender, CustProfile.Gender);
            Assert.AreEqual(hpapiProfile.displayName, CustProfile.DisplayName);
            Assert.AreEqual(hpapiProfile.addresses.Count, CustProfile.Addresses.Count);
            Assert.AreEqual(hpapiProfile.addresses.Count, CustProfile.Addresses.Count);
        }

        [TestMethod()]
        public void PhoneNumberTests_Serialize_True()
        {
            PhoneNumber pNo = new PhoneNumber()
            {
                Primary = true,
                CountryCode = "US",
                Type = AddressType.home.ToString(),
                AccountRecovery = true,
                AreaCode = "AreaCode",
                Id = "Id",
                Number = "Number",
                SerializeForPriviteFields = true,
                Verified = true
            };

            string json = JsonConvert.SerializeObject(pNo);

            Assert.IsTrue(json.Contains("AccountRecovery"));
            Assert.IsTrue(json.Contains("Verified"));
        }

        [TestMethod()]
        public void PhoneNumberTests_Serialize_DefaultIsTrue()
        {
            PhoneNumber pNo = new PhoneNumber()
            {
                Primary = true,
                CountryCode = "US",
                Type = AddressType.home.ToString(),
                AccountRecovery = true,
                AreaCode = "AreaCode",
                Id = "Id",
                Number = "Number",
                Verified = true
            };

            string json = JsonConvert.SerializeObject(pNo);

            Assert.IsTrue(json.Contains("AccountRecovery"));
            Assert.IsTrue(json.Contains("Verified"));
        }

        [TestMethod()]
        public void PhoneNumberTests_Serialize_False()
        {
            PhoneNumber pNo = new PhoneNumber()
            {
                Primary = true,
                CountryCode = "US",
                Type = AddressType.home.ToString(),
                AccountRecovery = true,
                AreaCode = "AreaCode",
                Id = "Id",
                Number = "Number",
                Verified = true,
                SerializeForPriviteFields = false
            };

            string json = JsonConvert.SerializeObject(pNo);

            Assert.IsFalse(json.Contains("AccountRecovery"));
            Assert.IsFalse(json.Contains("Verified"));
        }
    }
}
