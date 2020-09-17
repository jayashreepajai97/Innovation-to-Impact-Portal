using IdeaDatabase.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdeaDatabase.Utils.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class EnumUtilsTests
    {
        private enum TestEnum
        {
            [System.ComponentModel.Description("val1_descr")]
            Val1,
            [System.ComponentModel.Description("val2_descr")]
            Val2,
        }

        [TestMethod()]
        public void ValidateEnumTest()
        {
            // should pass
            Assert.IsTrue(EnumUtils.Validate(typeof(TestEnum), "Val1"));
            Assert.IsTrue(EnumUtils.Validate(typeof(TestEnum), "Val2"));
            Assert.IsTrue(EnumUtils.Validate(typeof(TestEnum), "Val1"));

            // should fail
            Assert.IsFalse(EnumUtils.Validate(typeof(TestEnum), "Val0"));
            Assert.IsFalse(EnumUtils.Validate(typeof(TestEnum), null));
        }

        [TestMethod()]
        public void ValidateEnumTest_ByDescription()
        {
            Assert.IsTrue(EnumUtils.Validate(typeof(TestEnum), "val2_descr", true));
            Assert.IsFalse(EnumUtils.Validate(typeof(TestEnum), "Val2", true));
        }

      
    }
}