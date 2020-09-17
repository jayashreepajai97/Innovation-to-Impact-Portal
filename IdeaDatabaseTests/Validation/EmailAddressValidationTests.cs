using System;
using Responses;
using System.Linq;
using IdeaDatabase.Validation;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeviceDatabaseTests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EmailAddressValidationTests
    {
        class Dummy : ValidableObject
        {
            [EmailAddressValidation(1, 60, true, Unicode: false, ForbiddenChars: "&*|\"")]
            public string Email { get; set; }
        }

        [TestMethod()]
        public void EmailAddressValidationTest_ValidEmailAddress()
        {
            Dummy d = new Dummy();
            string[] validSN = new string[]
            {
                "test@hp.com",
                "test.cent@hp.com",
                "test34.cent@hp.com"
            };

            foreach (string s in validSN)
            {
                d.Email = s;
                Assert.IsTrue(d.IsValid(new ResponseBase()));
            }
        }

        [TestMethod()]
        public void EmailAddressValidationTest_InValidSN()
        {
            Dummy d = new Dummy();
            string[] validSN = new string[]
            {
                "test22@.com",
                "test22@",
                "1234567890123456",
                "aassasAA---",
                "@ddd.com",
                ""
            };

            foreach (string s in validSN)
            {
                d.Email = s;
                Assert.IsFalse(d.IsValid(new ResponseBase()));
            }
        }

        [TestMethod()]
        public void EmailAddressValidationTest_TestEmail()
        {
            Dummy d = new Dummy();
            ResponseBase response = new ResponseBase();

            Assert.IsFalse(d.IsValid(response));
            Assert.IsTrue(response.ErrorList.First().ReturnCode == "FieldValidationError");
        }

        [TestMethod()]
        public void EmailAddressValidationTest_TestEmptyEmail()
        {
            Dummy d = new Dummy();
            d.Email = string.Empty;
            ResponseBase response = new ResponseBase();

            Assert.IsFalse(d.IsValid(response));
            Assert.IsTrue(response.ErrorList.Where(X => X.DebugStatusText == "Field has wrong length").Count() == 1);
            Assert.IsTrue(response.ErrorList.Where(X => X.DebugStatusText == "Field has invalid format").Count() == 1);
        }

        [TestMethod()]
        public void EmailAddressValidationTest_TestForbiddenChars()
        {
            Dummy d = new Dummy();
            d.Email = "test1&@hp.com";
            ResponseBase response = new ResponseBase();

            Assert.IsFalse(d.IsValid(response));
            Assert.IsTrue(response.ErrorList.Where(X => X.DebugStatusText == "Field contains invalid characters").Count() == 1);
        }
    }
}
