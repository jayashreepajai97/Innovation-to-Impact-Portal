using Microsoft.VisualStudio.TestTools.UnitTesting;
using Responses;

namespace IdeaDatabase.Validation.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass()]
    public class StringValidationTests
    {
        class Dummy: ValidableObject
        {
            [StringValidation(Required: true, Unicode: false, PasswordCheck: true, MatchRegexp: "(?=.{8,})((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])).*")]
            public string foo { get; set; }
        }

        [TestMethod()]
        public void StringValidationTest()
        {
            Dummy d = new Dummy();

            if(d.IsValid(new ResponseBase()))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void StringValidationTest_UnicodeFails()
        {
            Dummy d = new Dummy() { foo = "Ą"};
            Assert.IsFalse(d.IsValid(new ResponseBase()));            
        }

        [TestMethod()]
        public void StringValidationTest_PasswordFails()
        {
            Dummy d = new Dummy() { foo = "Abcd" };
            Assert.IsFalse(d.IsValid(new ResponseBase()));
        }
    }
}