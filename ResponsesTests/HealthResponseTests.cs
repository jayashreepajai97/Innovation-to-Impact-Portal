using Microsoft.VisualStudio.TestTools.UnitTesting;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responses.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass()]
    public class HealthResponseTests
    {
        [TestMethod()]
        public void HealthResponseTest()
        {
            HealthResponse r = new HealthResponse();
            Assert.IsNotNull(r);
        }
    }
}