using System;
using Responses;
using Responses.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResponsesTests
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class FaultTests
    {
        [TestMethod]
        public void FaultTest_Constr1_Default()
        {
            Fault f = new Fault("ReturnCode", "Message");
            Assert.IsNull(f.StatusText);
            Assert.AreEqual(f.DebugStatusText, f.DebugStatusText);
            Assert.AreEqual(f.ErrorCategory, ErrorCategory.General);
        }

        

        [TestMethod]
        public void FaultTest_Constr2_Default()
        {
            Fault f = new Fault("Origin", "ReturnCode", "Message");
            Assert.IsNull(f.StatusText);
            Assert.AreEqual(f.DebugStatusText, f.DebugStatusText);
            Assert.AreEqual(f.ErrorCategory, ErrorCategory.General);
        }

       

        [TestMethod]
        public void FaultTest_Constr3_Default()
        {
            Fault f = new Fault("ReturnCode", "Message");
            Fault fx = new Fault(f, new Exception("Inner error."));
            Assert.IsNull(f.StatusText);
            Assert.AreEqual(f.DebugStatusText, f.DebugStatusText);
            Assert.AreEqual(fx.ErrorCategory, ErrorCategory.General);
        }

       
    }
}
