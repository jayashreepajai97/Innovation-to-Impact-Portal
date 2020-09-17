using InnovationPortalService;
using InnovationPortalService.HPID;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace InnovationPortalServiceTests.HPID.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ScimUpdateHelperTests
    {
        ScimUpdateHelper sch;
        private CustomerData customer;
        private CustomerProfile profile;
        private HPIDCustomerProfile hpidProfile;

        [TestInitialize]
        public void Init()
        {
            sch = new ScimUpdateHelper();

            customer = new CustomerData()
            {
                CompanyName = "Company Name",
                Country = "XX",
                Language = "xx",
                FirstName = "FirstName",
                LastName = "LastName",
                EmailAddress = "user@mail.com",
                City = "City"
            };

            profile = new CustomerProfile()
            {
                CompanyName = "Company Name",
                Country = "XX",
                Language = "xx",
                FirstName = "FirstName",
                LastName = "LastName",
                EmailAddress = "user@mail.com",
                City = "City"
            };

            hpidProfile = new HPIDCustomerProfile(profile);
        }

        [TestMethod()]
        public void SelectOperationTest_EmptyInputData()
        {
            Assert.IsFalse(sch.IsHPIDCustomerProfileUpdateRequired(null, null, true));
        }

        #region updateProfile_OldAPICall

        [TestMethod()]
        public void SelectOperationTest_NoDifferences_OldAPI()
        {
            Assert.IsFalse(sch.IsHPIDCustomerProfileUpdateRequired(customer, hpidProfile, false));
        }

        [TestMethod()]
        public void SelectOperationTest_ReplacementsRequired_OldAPI()
        {
            CustomerData changed = new CustomerData()
            {
                CompanyName = "New",
                Country = "NN",
                Language = "nn",
                FirstName = "New",
                LastName = "New",
                EmailAddress = "new@mail.com",
                City = "New"
            };


            Assert.IsTrue(sch.IsHPIDCustomerProfileUpdateRequired(changed, hpidProfile, false));
            Assert.IsTrue(sch.AddOperationsCount == 0);
            Assert.IsTrue(sch.ReplaceOperationsCount == 8);
            Assert.IsTrue(sch.RemoveOperationsCount == 0);
        }

        [TestMethod()]
        public void SelectOperationTest_RemovalsRequired_OldAPI()
        {
            CustomerData changed = customer;
            changed.CompanyName = null;
            changed.City = null;

            Assert.IsTrue(sch.IsHPIDCustomerProfileUpdateRequired(changed, hpidProfile, false));
            Assert.IsTrue(sch.AddOperationsCount == 0);
            Assert.IsTrue(sch.ReplaceOperationsCount == 0);
            Assert.IsTrue(sch.RemoveOperationsCount == 1);
        }

        [TestMethod()]
        public void SelectOperationTest_NewPositionsAdded_OldAPI()
        {
            HPIDCustomerProfile orig = new HPIDCustomerProfile();

            Assert.IsTrue(sch.IsHPIDCustomerProfileUpdateRequired(customer, orig, false));
            Assert.IsTrue(sch.AddOperationsCount == 5);
            Assert.IsTrue(sch.ReplaceOperationsCount == 0);
            Assert.IsTrue(sch.RemoveOperationsCount == 0);
        }

        [TestMethod()]
        public void SelectOperationTest_NewAddressAdded_OldAPI()
        {
            hpidProfile.addresses = new List<Address>() { new Address() { Primary = false } };

            Assert.IsTrue(sch.IsHPIDCustomerProfileUpdateRequired(customer, hpidProfile, false));
            Assert.IsTrue(sch.AddOperationsCount == 1);
            Assert.IsTrue(sch.ReplaceOperationsCount == 0);
            Assert.IsTrue(sch.RemoveOperationsCount == 0);
        }

        [TestMethod()]
        public void SelectOperationTest_NoChangesRequired_OldAPI()
        {
            hpidProfile.name = null;
            hpidProfile.addresses = new List<Address>() { new Address() { Primary = false } };
            hpidProfile.hpp_organizationName = null;

            CustomerData changed = new CustomerData();

            Assert.IsFalse(sch.IsHPIDCustomerProfileUpdateRequired(changed, hpidProfile, false));
            Assert.IsTrue(sch.AddOperationsCount == 0);
            Assert.IsTrue(sch.ReplaceOperationsCount == 0);
            Assert.IsTrue(sch.RemoveOperationsCount == 0);
        }

        [TestMethod()]
        public void GetJsonTest_ScimOperationsAddAndReplace_OldAPI()
        {
            hpidProfile.hpp_organizationName = null;

            CustomerData changed = new CustomerData()
            {
                CompanyName = "New",
                Country = "NN",
                Language = "nn",
                FirstName = "New",
                LastName = "New",
                EmailAddress = "new@mail.com",
                City = "New"
            };

            sch.IsHPIDCustomerProfileUpdateRequired(changed, hpidProfile, false);

            string jsonF = sch.GetJson();
            Assert.IsNotNull(jsonF);

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonF);
            Assert.IsNotNull(jsonObj["Operations"]);

            JArray childs = jsonObj["Operations"];
            Assert.IsTrue(childs.Count == (sch.AddOperationsCount + sch.ReplaceOperationsCount));
        }

        #endregion updateProfile_OldAPICall

        [TestMethod()]
        public void SelectOperationTest_NoDifferences_NewAPI()
        {
            Assert.IsTrue(sch.IsHPIDCustomerProfileUpdateRequired(CreateTestCustomerData(), CreateTestCustomerProfile(), true));
        }

        [TestMethod()]
        public void SelectOperationTest_ReplacementsRequired()
        {
            CustomerData changed = CreateTestCustomerData();
            changed.CompanyName = "New";
            changed.Country = "NN";
            changed.Language = "nn";
            changed.FirstName = "New";
            changed.LastName = "New";
            changed.EmailAddress = "new@mail.com";
            changed.City = "New";
            changed.DisplayName = "NewDisplayName";
            changed.Gender = "M";
            changed.Addresses[0].Country = "CN";
            changed.Addresses[0].Type = AddressType.business.ToString();
            changed.Addresses[0].District = "District";
            changed.PhoneNumbers[0].AreaCode = "NewAreaCode";
            changed.PhoneNumbers[0].CountryCode = "CN";
            changed.PhoneNumbers[0].Number = "Number";

            Assert.IsTrue(sch.IsHPIDCustomerProfileUpdateRequired(changed, CreateTestCustomerProfile(), true));
            Assert.IsTrue(sch.AddOperationsCount == 0);
            Assert.IsTrue(sch.ReplaceOperationsCount == 9);
            Assert.IsTrue(sch.RemoveOperationsCount == 0);
        }

        [TestMethod()]
        public void SelectOperationTest_RemovalsRequired()
        {
            CustomerData changed = CreateTestCustomerData();
            changed.CompanyName = null;
            changed.City = null;
            changed.Gender = null;
            changed.DisplayName = null;
            changed.Addresses = null;
            changed.PhoneNumbers = null;

            Assert.IsTrue(sch.IsHPIDCustomerProfileUpdateRequired(changed, CreateTestCustomerProfile(), true));
            Assert.IsTrue(sch.AddOperationsCount == 0);
            Assert.IsTrue(sch.ReplaceOperationsCount == 0);
            Assert.IsTrue(sch.RemoveOperationsCount == 1);
        }

        [TestMethod()]
        public void SelectOperationTest_NoNewAddressAndPhoneNumberAdded()
        {
            CustomerData changed = CreateTestCustomerData();
            changed.Addresses.Add(new Address()
            {
                Id = "222222",
                Country = "US",
                Type = AddressType.business.ToString(),
                Role = "other",
                Primary = true,
                District = "Rampur",
                PostalCode = "426133",
                Line1 = "asdsa",
            });
            changed.PhoneNumbers.Add(new PhoneNumber()
            {
                Id = "22222222",
                Primary = true,
                CountryCode = "US",
                Type = AddressType.business.ToString(),
                AreaCode = "222222",
                Number = "2222222"
            });

            bool result = sch.IsHPIDCustomerProfileUpdateRequired(changed, CreateTestCustomerProfile(), true);

            Assert.IsTrue(result);
            Assert.IsTrue(sch.AddOperationsCount == 0);
            Assert.IsTrue(sch.ReplaceOperationsCount == 1);
            Assert.IsTrue(sch.RemoveOperationsCount == 0);
        }

        [TestMethod()]
        public void GetJsonTest_NoDataFromScim()
        {
            string jsonF = sch.GetJson();
            Assert.IsNotNull(jsonF);

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonF);
            Assert.IsNotNull(jsonObj["Operations"]);

            JArray childs = jsonObj["Operations"];
            Assert.IsTrue(childs.Count == 0);
        }

        [TestMethod()]
        public void GetJsonTest_ScimOperationsAddAndReplace()
        {
            hpidProfile.hpp_organizationName = null;

            CustomerData changed = new CustomerData()
            {
                CompanyName = "New",
                Country = "NN",
                Language = "nn",
                FirstName = "New",
                LastName = "New",
                EmailAddress = "new@mail.com",
                City = "New"
            };

            sch.IsHPIDCustomerProfileUpdateRequired(CreateTestCustomerData(), CreateTestCustomerProfile(), true);

            string jsonF = sch.GetJson();
            Assert.IsNotNull(jsonF);

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonF);
            Assert.IsNotNull(jsonObj["Operations"]);

            JArray childs = jsonObj["Operations"];
            Assert.IsTrue(childs.Count == (sch.AddOperationsCount + sch.ReplaceOperationsCount));
        }

        [TestMethod()]
        public void GetJsonTest_ScimOperationsRemove()
        {
            CustomerData changed = customer;
            changed.CompanyName = null;

            sch.IsHPIDCustomerProfileUpdateRequired(changed, hpidProfile, true);

            string jsonF = sch.GetJson();
            Assert.IsNotNull(jsonF);

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonF);
            Assert.IsNotNull(jsonObj["Operations"]);

            JArray childs = jsonObj["Operations"];
            Assert.IsTrue(childs.Count == (sch.RemoveOperationsCount));
        }

        private CustomerData CreateTestCustomerData()
        {
            CustomerData CustData = new CustomerData()
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
                        Id = "XSDJDNDLSD",
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
                        Id = "PBSDHDJDNDLSD",
                        Primary = true,
                        CountryCode = "US",
                        Type = AddressType.home.ToString(),
                    }
                }
            };

            return CustData;
        }

        private HPIDCustomerProfile CreateTestCustomerProfile()
        {
            HPIDCustomerProfile hpidCustProfile = new HPIDCustomerProfile()
            {
                name = new Name() { familyName = "LastName", givenName = "FirstName" },
                countryResidence = "US",
                locale = $"en_US",
                displayName = "DisplayName",
                gender = "F",
                hpp_organizationName = "CompanyName",
                addresses = new System.Collections.Generic.List<Address>()
                {
                    new Address()
                    {
                        Id = "XSDJDNDLSD",
                        Country = "US",
                        Type = AddressType.home.ToString(),
                        Role = "other",
                        Primary = true
                    }
                },
                phoneNumbers = new System.Collections.Generic.List<PhoneNumber>()
                {
                    new PhoneNumber()
                    {
                        Id = "PBSDHDJDNDLSD",
                        Primary = true,
                        CountryCode = "US",
                        Type = AddressType.home.ToString(),
                    }
                }
            };

            return hpidCustProfile;
        }
    }
}