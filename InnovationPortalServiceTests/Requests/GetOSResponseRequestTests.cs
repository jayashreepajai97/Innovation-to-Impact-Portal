using Responses;
using HPSAProfileService.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HPSAProfileServiceTests.Requests
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GetOSResponseRequestTests
    {
        [TestMethod]
        public void GetOSResponseRequestTest_ValidationError()
        {
            GetOSResponseRequest req = new GetOSResponseRequest()
            {
                ClientId = "HP",
                Devices = new System.Collections.Generic.List<HPSAProfileService.DeviceObligation>()
                {
                    new HPSAProfileService.DeviceObligation()
                    {
                        CountryCode = "US",
                        ProductNumber = "ProductNumber",
                        SerialNumber = "SerialNumber"
                    }
                }
            };

            ResponseBase response = new ResponseBase();
            req.IsValid(response);
            
            Assert.AreEqual(response.FaultItemList.Count, 1);
            EnumValidationFault validationFault = response.FaultItemList.FirstOrDefault() as EnumValidationFault;
            Assert.AreEqual(validationFault.FieldName, "ClientId");
            Assert.AreEqual(validationFault.StatusText, "Field has invalid value");
            Assert.AreEqual(validationFault.AcceptedValues, "HPSF, HPWC, DROID, hpi-os-toronto");
        }

        [TestMethod]
        public void GetOSResponseRequestTest_Success()
        {
            GetOSResponseRequest req = new GetOSResponseRequest()
            {
                ClientId = "hpi-os-toronto",
                Devices = new System.Collections.Generic.List<HPSAProfileService.DeviceObligation>()
                {
                    new HPSAProfileService.DeviceObligation()
                    {
                        CountryCode = "US",
                        ProductNumber = "ProductNumber",
                        SerialNumber = "SerialNumber"
                    }
                }
            };

            ResponseBase response = new ResponseBase();
            req.IsValid(response);

            Assert.AreEqual(response.FaultItemList.Count, 0);
        }
    }
}
