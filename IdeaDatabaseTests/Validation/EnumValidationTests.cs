using IdeaDatabase.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Responses;

namespace IdeaDatabase.Validation.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class EnumValidationTests
    {
        class Dummy : ValidableObject
        {
            [EnumValidation(typeof(RESTAPILocale), false, false, true)]
            public string Locale { get; set; }
        }

        [TestMethod()]
        public void EnumValidationTest_Fails()
        {
            Dummy d = new Dummy() { Locale = "xx-XX"};
            Assert.IsFalse(d.IsValid(new ResponseBase()));            
        }

        [TestMethod()]
        public void EnumValidationTest_Success()
        {
            Dummy d = new Dummy() { Locale = "en-US" };
            Assert.IsTrue(d.IsValid(new ResponseBase()));
        }

        [TestMethod()]
        public void EnumValidationTest_Success_EmptyValue()
        {
            Dummy d = new Dummy() { Locale = "" };
            Assert.IsTrue(d.IsValid(new ResponseBase()));
        }
    }
}