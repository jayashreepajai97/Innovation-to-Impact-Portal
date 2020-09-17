using IdeaDatabase.Enums;
using InnovationPortalService;
using InnovationPortalService.HPID;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InnovationPortalServiceTests.HPID.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AddressTests
    {
        CustomerData customer;

        [TestInitialize]
        public void Init()
        {
            customer = new CustomerData();            
        }

        [TestMethod()]
        public void AddressTest_MissingPrimaryUse()
        {
            Address adr = new Address(customer);
            Assert.IsTrue(adr.Type.Equals(AddressType.other.ToString()));
        }

        [TestMethod()]
        public void AddressTest_PrimaryUseItem002()
        {
            customer.PrimaryUse = PrimaryUseType.Item002.ToString();
            Address adr = new Address(customer);
            Assert.IsTrue(adr.Type.Equals(AddressType.home.ToString()));
        }

        [TestMethod()]
        public void AddressTest_PrimaryUseItem003()
        {
            customer.PrimaryUse = PrimaryUseType.Item003.ToString();
            Address adr = new Address(customer);
            Assert.IsTrue(adr.Type.Equals(AddressType.business.ToString()));
        }
    }
}