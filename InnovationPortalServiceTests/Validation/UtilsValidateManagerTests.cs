using Moq;
using System;
using System.Linq;
using DependenyInjector;
using Newtonsoft.Json.Linq;
using HPSAProfileService.Responses;
using HPSAProfileService.Validation;
using HPSAProfileService.Contract.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HPSAProfileService.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UtilsValidateManagerTests
    {
       private static Mock<ICRMUtilsValidator> crmUtilsValidator;
       private static Mock<ICDAXUtilsValidator> cdaxUtilsValidator;

        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            crmUtilsValidator = new Mock<ICRMUtilsValidator>();
            DependencyInjector.Register(crmUtilsValidator.Object).As<ICRMUtilsValidator>();

            cdaxUtilsValidator = new Mock<ICDAXUtilsValidator>();
            DependencyInjector.Register(cdaxUtilsValidator.Object).As<ICDAXUtilsValidator>();
        }

        [TestMethod()]
        public void UtilsValidateManagerTests_If_ShouldAllowCaseCreation_Is_false_Then_Validate_Should_return_false()
        {
            #region Arrange
            string reqStr = "{  \"CaseExchange\": {    \"SubjectMissing\": \"1 Customer?has?a?problem?with?the?booting?of?the?notebook.\",    \"Description\": \"\",    \"CustomerB2BName\": \"AMS_HPSA_C\",    \"Account\": {      \"AccountName\": \"1 Text?Talwaker\",      \"Address\": {        \"Country\": \"US\"      }    },    \"Contact\": [      {        \"ContactType\": \"Primary\",        \"FirstName\": \"TestUser\",        \"LastName\": \"User\",        \"PrimaryEmailAddress\": \"Test.user@hp.com\",        \"Address\": {          \"Country\": \"US\"        }      }    ],    \"Asset\": {      \"SerialNumber\": \"CND5325YN2\",      \"ProductNumber\": \"N8M28PA\"    }  }}";
            object req = Newtonsoft.Json.JsonConvert.DeserializeObject(reqStr);

            RESTAPICRMCreateCaseResponseExt response = new RESTAPICRMCreateCaseResponseExt();

            crmUtilsValidator.Setup(x => x.ShouldAllowCaseCreation(It.IsAny<RESTAPICRMCreateCaseResponseExt>(), It.IsAny<int>(), It.IsAny<object>())).Returns(false);


            #endregion Arrange

            #region Act
            var tupleResults = ValidationManager.CRMCases.Validate(req, response, 1234);
            #endregion Act

            #region Assert
            Assert.AreEqual(tupleResults.Item1, false , "Validate should return false");
            #endregion Assert
        }

        [TestMethod()]
        public void UtilsValidateManagerTests_If_ValidateCDAX_Is_false_Then_Validate_Should_return_false()
        {
            #region Arrange
            JObject req = JObject.Parse("{ \"UnknownNode\": {} }");

            RESTAPICRMCreateCaseResponseExt response = new RESTAPICRMCreateCaseResponseExt();

            crmUtilsValidator.Setup(x => x.ShouldAllowCaseCreation(It.IsAny<RESTAPICRMCreateCaseResponseExt>(), It.IsAny<int>(), It.IsAny<object>())).Returns(true);

            response.FaultItemList.Add(Faults.MissingCdaxRequestField);
            cdaxUtilsValidator.Setup(x => x.ValidateCDAX(It.IsAny<object>(), It.IsAny<int>()))
                                .Returns(new Tuple<bool, RESTAPICRMCreateCaseResponseExt>(false, response));

            #endregion Arrange

            #region Act
            var tupleResults = ValidationManager.CRMCases.Validate(req, response, 1234);
            #endregion Act

            #region Assert
            Assert.AreEqual(tupleResults.Item1, false, "Validate should return false");
            Assert.AreEqual(1, response.FaultItemList.Count, "Invalid faults count received");
            Assert.AreEqual(Faults.MissingCdaxRequestField, response.FaultItemList.First(), "Invalid fault type received");

            #endregion Assert
        }

        [TestMethod()]
        public void UtilsValidateManagerTests_If_ShouldAllowCaseCreation_Is_True_ValidateCDAX_Is_True_Then_Validate_Should_return_True()
        {
            #region Arrange
            string reqStr = "{\"CaseExchange\": {\"EventType\": \"Case\",\"EventSubType\": \"Retrieve\",\"TransactionID\": \"868d5a18-2ef5-4503-babf-f1fa089fc9d0\",\"TransactionStatus\": \"Success\",\"MessageCode\": \"200\",\"StatusLine\": \"HTTP/1.1 200 OK\",\"IncomingChannel\": \"HPSA_Phone\",\"CaseID\": \"5150158393\",\"ID\": \"4fa055f5-ed59-e911-a834-000d3a1bd09a\",\"CaseType\": \"HW Delivery\",\"Subject\": \"Battery Not working\",\"CaseStatus\": \"New\",\"CreatedDateTime\": \"2019-04-08\",\"Account\": {\"AccountName\": \"John\",\"Address\": {\"Region\": \"AMERICAS\",\"Country\": \"United States\"}}}}";
            object req = Newtonsoft.Json.JsonConvert.DeserializeObject(reqStr);

            RESTAPICRMCreateCaseResponseExt response = new RESTAPICRMCreateCaseResponseExt();

            crmUtilsValidator.Setup(x => x.ShouldAllowCaseCreation(It.IsAny<RESTAPICRMCreateCaseResponseExt>(), It.IsAny<int>(), It.IsAny<object>())).Returns(true);
            cdaxUtilsValidator.Setup(x => x.ValidateCDAX(It.IsAny<object>(), It.IsAny<int>()))
                                .Returns(new Tuple<bool, RESTAPICRMCreateCaseResponseExt>(true, response));

            #endregion Arrange

            #region Act
            var tupleResults = ValidationManager.CRMCases.Validate(req, response, 1234);
            #endregion Act

            #region Assert
            Assert.AreEqual(tupleResults.Item1, true, "Validate should return true");
            Assert.AreEqual(0, response.FaultItemList.Count, "Fault Count should be Zero");
            #endregion Assert
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            crmUtilsValidator.Reset();
            cdaxUtilsValidator.Reset();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DependencyInjector.Clear();
        }
    }
}