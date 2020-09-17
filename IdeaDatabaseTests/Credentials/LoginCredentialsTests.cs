using Microsoft.VisualStudio.TestTools.UnitTesting;
using Credentials;
using InnovationPortalService.Responses;
using System.Linq;
using Responses;
using IdeaDatabase.Validation;
using Moq;
using Hpcs.DependencyInjector;

namespace DeviceDatabaseTests.Credentials
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class LoginCredentialsTests
    {
        #region Test Setup
        public static Mock<IDbFieldsConstraints> dbfcMock;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext context)
        {
            dbfcMock = new Mock<IDbFieldsConstraints>();
            DependencyInjector.Register(dbfcMock.Object).As<IDbFieldsConstraints>();

            DummyConstraints dc = new DummyConstraints();
            dc.StringSingleConstraints("UserAuthentications", "CallerId", 255, true, 1);
            dbfcMock.Setup(x => x.Constraints).Returns(dc.GetConstraints());
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DependencyInjector.Clear();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            dbfcMock.Reset();
        }
        #endregion Test Setup

        #region Positive Scenarios
        [TestMethod]
        public void IsValidTest_ValidUserNamePasswordCallerId_ShouldValidateSuccessfully()
        {
            #region Arrange
            LoginCredentials credential = new LoginCredentials();
            credential.UserName = "superuser";
            credential.Password = "p@ssw0rd";
            credential.CallerId = "TestCallerId";
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            bool result = credential.IsValid(response);
            #endregion Act

            #region Assert
            Assert.IsTrue(result, "The validation should not have failed as the values provided were valid");
            Assert.IsTrue(response.ErrorList.Count == 0);
            #endregion Assert
        }
        #endregion Positive Scenarios

        #region Negative Scenarios
        [TestMethod]
        public void IsValidTest_NullUserName_ShouldReturnFaultWithSpecificErrorDetails()
        {
            #region Arrange
            LoginCredentials credential = new LoginCredentials();
            credential.UserName = null;
            credential.Password = "p@ssw0rd";
            credential.CallerId = "TestCallerId";
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            bool result = credential.IsValid(response);
            #endregion Act

            #region Assert
            Assert.IsTrue(response.ErrorList.Count == 1);
            Fault fault = response.ErrorList.First();

            // Fault object type check
            Assert.IsInstanceOfType(fault, typeof(ValidationFault), "Wrong fault object type returned");

            // Fault properties check
            Assert.IsTrue(string.Equals(fault.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault.DebugStatusText, "Required field is missing"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault as ValidationFault).ErrorType, "Required"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault as ValidationFault).FieldName, "UserName"), "Wrong validation field name returned");
            #endregion Assert
        }

        [TestMethod]
        public void IsValidTest_BlankPassword_ShouldReturnFaultWithSpecificErrorDetails()
        {
            #region Arrange
            LoginCredentials credential = new LoginCredentials();
            credential.UserName = "superuser";
            credential.Password = string.Empty;
            credential.CallerId = "TestCallerId";
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            bool result = credential.IsValid(response);
            #endregion Act

            #region Assert
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(!result, "The validation should have failed due to invalid Password");

            // Fault object type check
            Fault fault = response.ErrorList.First();
            Assert.IsInstanceOfType(fault, typeof(ValidationFault), "Wrong fault object type returned");

            // Fault properties check
            Assert.IsTrue(string.Equals(fault.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault.DebugStatusText, "Field has wrong length"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault as ValidationFault).ErrorType, "Length"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault as ValidationFault).FieldName, "Password"), "Wrong validation field name returned");
            #endregion Assert
        }

        [TestMethod]
        public void IsValidTest_BlankCalledId_ShouldReturnFaultWithSpecificErrorDetails()
        {
            #region Arrange
            LoginCredentials credential = new LoginCredentials();
            credential.UserName = "superuser";
            credential.Password = "p@ssw0rd";
            credential.CallerId = string.Empty;
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            bool result = credential.IsValid(response);
            #endregion Act

            #region Assert
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(!result, "The validation should have failed due to invalid CallerId");

            // Fault object type check
            Fault fault = response.ErrorList.First();
            Assert.IsInstanceOfType(fault, typeof(ValidationFault), "Wrong fault object type returned");
            
            // Fault properties check
            Assert.IsTrue(string.Equals(fault.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault.DebugStatusText, "Field has wrong length"), "Wrong fault status text returned");
            
            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault as ValidationFault).ErrorType, "Length"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault as ValidationFault).FieldName, "CallerId"), "Wrong validation field name returned");
            #endregion Assert
        }

        [TestMethod]
        public void IsValidTest_BlankUserNameNullPassword_ShouldReturnFaultWithSpecificErrorDetails()
        {
            #region Arrange
            LoginCredentials credential = new LoginCredentials();
            credential.UserName = string.Empty;
            credential.Password = null;
            credential.CallerId = string.Empty;
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            bool result = credential.IsValid(response);
            #endregion Act

            #region Assert
            Assert.IsTrue(response.ErrorList.Count == 3);
            Assert.IsTrue(!result, "The validation should have failed due to invalid UserName, Password & CallerId");

            // Fault object type check
            Fault fault1 = response.ErrorList.ElementAt(0);
            Fault fault2 = response.ErrorList.ElementAt(1);
            Assert.IsInstanceOfType(fault1, typeof(ValidationFault), "Wrong fault object type returned");
            Assert.IsInstanceOfType(fault2, typeof(ValidationFault), "Wrong fault object type returned");

            #region Fault1
            // Fault properties check
            Assert.IsTrue(string.Equals(fault1.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault1.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault1.DebugStatusText, "Field has wrong length"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault1 as ValidationFault).ErrorType, "Length"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault1 as ValidationFault).FieldName, "UserName"), "Wrong validation field name returned");
            #endregion Fault1

            #region Fault2
            // Fault properties check
            Assert.IsTrue(string.Equals(fault2.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault2.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault2.DebugStatusText, "Required field is missing"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault2 as ValidationFault).ErrorType, "Required"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault2 as ValidationFault).FieldName, "Password"), "Wrong validation field name returned");
            #endregion Fault2

            #endregion Assert
        }
        #endregion Negative Scenarios
    }


}
