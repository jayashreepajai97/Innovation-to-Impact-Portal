using IdeaDatabase.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Responses;
using System;
using System.Linq;

namespace DeviceDatabaseTests.Validation
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass()]
    public class PLCodeValidationTests
    {
        class Dummy : ValidableObject
        {
            [PLCodeValidation(2,2,50,false)]
            public string PLCode { get; set; }

            public bool? IsHPProduct { get; set; }
        }

        [TestMethod()]
        public void PLCodeValidationTest_IsHPProduct_Fails()
        {
            Dummy d = new Dummy() { PLCode="HPInc", IsHPProduct=true};

            ResponseBase r = new ResponseBase();
            Assert.IsFalse(d.IsValid(r));

            Assert.AreEqual(r.ErrorList.Count, 1);
            Assert.IsTrue(r.ErrorList.Where(X => X.DebugStatusText == "Field has wrong length").Count() == 1);
        }

        [TestMethod()]
        public void PLCodeValidationTest_IsHPProduct_Passes()
        {
            Dummy d = new Dummy() { PLCode = "HP", IsHPProduct = true };
            ResponseBase r = new ResponseBase();
            Assert.IsTrue(d.IsValid(r));

            Assert.AreEqual(r.ErrorList.Count, 0);
        }

        [TestMethod()]
        public void PLCodeValidationTest_IsNonHPProduct_Passes()
        {
            Dummy d = new Dummy() { PLCode = "IDEAPAD", IsHPProduct = false };
            ResponseBase r = new ResponseBase();
            Assert.IsTrue(d.IsValid(r));

            Assert.AreEqual(r.ErrorList.Count, 0);
        }

        [TestMethod()]
        public void PLCodeValidationTest_IsNonHPProduct_Fails()
        {
            Dummy d = new Dummy() { PLCode = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been fun.",
                                    IsHPProduct = false };

            ResponseBase r = new ResponseBase();
            Assert.IsFalse(d.IsValid(r));

            Assert.AreEqual(r.ErrorList.Count, 1);
            Assert.IsTrue(r.ErrorList.Where(X => X.DebugStatusText == "Field has wrong length").Count() == 1);
        }
    } 
}
