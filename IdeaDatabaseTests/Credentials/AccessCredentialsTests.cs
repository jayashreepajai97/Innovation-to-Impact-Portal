using Credentials;
using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Utils;
using IdeaDatabase.Validation;
using InnovationPortalService.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Responses;
using System.Linq;
using System;

namespace DeviceDatabaseTests.Credentials
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AccessCredentialsTests
    {
        #region Test Setup
        private static Mock<IDbFieldsConstraints> dbfcMock;
        private static Mock<IIdeaDatabaseDataContext> dbdcMock;
        private static Mock<IQueryUtils> queryUtilsMock;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext context)
        {
            dbfcMock = new Mock<IDbFieldsConstraints>();
            DependencyInjector.Register(dbfcMock.Object).As<IDbFieldsConstraints>();

            DummyConstraints dc = new DummyConstraints();
            dc.StringSingleConstraints("UserAuthentications", "CallerId", 255, true, 1);
            dbfcMock.Setup(x => x.Constraints).Returns(dc.GetConstraints());
             
            dbdcMock = new Mock<IIdeaDatabaseDataContext>();
            DependencyInjector.Register(dbdcMock.Object).As<IIdeaDatabaseDataContext>();

            queryUtilsMock = new Mock<IQueryUtils>();
            DependencyInjector.Register(queryUtilsMock.Object).As<IQueryUtils>();

            queryUtilsMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(),
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new UserAuthentication() { Token = "ReturnedToken" } );

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
            dbdcMock.Reset();
            queryUtilsMock.Reset();
        }
        #endregion Test Setup

        #region Positive Scenarios
        [TestMethod]
        public void IsValidTest_ValidCallerIdCustomerIdSessionToken_LoginDate()
        {
            #region Arrange
            AccessCredentials credential = new AccessCredentials();
            credential.CallerId = "TestCallerId";
            credential.UserID = 100;
            credential.SessionToken = "TestSessionToken";
            credential.UseCaseGroup = "SANC";
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            DateTime loginDate = DateTime.UtcNow;
            queryUtilsMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(),
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new UserAuthentication() { Token = "ReturnedToken", CreatedDate = loginDate });

            bool result = credential.IsValid(response);
            #endregion Act

            #region Assert
            Assert.IsTrue(result, "The validation should not have failed as the values provided were valid");
            Assert.IsTrue(response.ErrorList.Count == 0);
            Assert.IsNotNull(credential.LoginDate);
            Assert.AreEqual(credential.LoginDate, loginDate);
            #endregion Assert
        }

        [TestMethod]
        public void IsValidTest_ValidCallerIdCustomerIdSessionToken_ShouldValidateSuccessfully()
        {
            #region Arrange
            AccessCredentials credential = new AccessCredentials();
            credential.CallerId = "TestCallerId";
            credential.UserID = 100;
            credential.SessionToken = "TestSessionToken";
            credential.UseCaseGroup = "SANC";
            DateTime loginDate = DateTime.UtcNow;
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            queryUtilsMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(),
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new UserAuthentication() { Token = "ReturnedToken", CreatedDate = loginDate });

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
        public void IsValidTest_NullCallerIdNegativeCustomerIdEmptySessionToken_ShouldReturnFaultWithSpecificErrorDetails()
        {
            #region Arrange
            AccessCredentials credential = new AccessCredentials();
            credential.CallerId = null;
            credential.UserID = -100;
            credential.SessionToken = string.Empty;
            credential.UseCaseGroup = null;
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            bool result = credential.IsValid(response);
            #endregion Act

            #region Assert
            Assert.IsTrue(!result, "The validation should have failed due to invalid UserName, Password & CallerId");
            Assert.IsTrue(response.ErrorList.Count == 3);

            // Fault object type check
            Fault fault1 = response.ErrorList.ElementAt(0);
            Fault fault2 = response.ErrorList.ElementAt(1);
            Fault fault3 = response.ErrorList.ElementAt(2);

            Assert.IsInstanceOfType(fault1, typeof(ValidationFault), "Wrong fault object type returned");
            Assert.IsInstanceOfType(fault2, typeof(ValidationFault), "Wrong fault object type returned");
            Assert.IsInstanceOfType(fault3, typeof(ValidationFault), "Wrong fault object type returned");

            #region Fault1-UserID
            // Fault properties check
            Assert.IsTrue(string.Equals(fault1.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault1.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault1.DebugStatusText, "Numeric field value out of bounds"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault1 as ValidationFault).ErrorType, "Numeric"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault1 as ValidationFault).FieldName, "UserID"), "Wrong validation field name returned");
            #endregion Fault1-UserID

            #region Fault2-SessionToken
            // Fault properties check
            Assert.IsTrue(string.Equals(fault2.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault2.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault2.DebugStatusText, "Field has wrong length"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault2 as ValidationFault).ErrorType, "Length"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault2 as ValidationFault).FieldName, "SessionToken"), "Wrong validation field name returned");
            #endregion Fault2-SessionToken

            #region Fault3-CallerId
            // Fault properties check
            Assert.IsTrue(string.Equals(fault3.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault3.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault3.DebugStatusText, "Required field is missing"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault3 as ValidationFault).ErrorType, "Required"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault3 as ValidationFault).FieldName, "CallerId"), "Wrong validation field name returned");
            #endregion Fault3-CallerId

            #endregion Assert
        }

        [TestMethod]
        public void IsValidTest_EmptyCallerIdZeroCustomerIdNullSessionToken_ShouldReturnFaultWithSpecificErrorDetails()
        {
            #region Arrange
            AccessCredentials credential = new AccessCredentials();
            credential.CallerId = string.Empty;
            credential.UserID = 0;
            credential.SessionToken = null;
            GetProfileResponse response = new GetProfileResponse();
            #endregion Arrange

            #region Act
            bool result = credential.IsValid(response);
            #endregion Act

            #region Assert
            Assert.IsTrue(!result, "The validation should have failed due to invalid UserName, Password & CallerId");
            Assert.IsTrue(response.ErrorList.Count == 3);

            // Fault object type check
            Fault fault1 = response.ErrorList.ElementAt(0);
            Fault fault2 = response.ErrorList.ElementAt(1);
            Fault fault3 = response.ErrorList.ElementAt(2);

            Assert.IsInstanceOfType(fault1, typeof(ValidationFault), "Wrong fault object type returned");
            Assert.IsInstanceOfType(fault2, typeof(ValidationFault), "Wrong fault object type returned");
            Assert.IsInstanceOfType(fault3, typeof(ValidationFault), "Wrong fault object type returned");

            #region Fault1-UserID
            // Fault properties check
            Assert.IsTrue(string.Equals(fault1.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault1.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault1.DebugStatusText, "Numeric field value out of bounds"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault1 as ValidationFault).ErrorType, "Numeric"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault1 as ValidationFault).FieldName, "UserID"), "Wrong validation field name returned");
            #endregion Fault1-UserID

            #region Fault2-SessionToken
            // Fault properties check
            Assert.IsTrue(string.Equals(fault2.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault2.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault2.DebugStatusText, "Required field is missing"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault2 as ValidationFault).ErrorType, "Required"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault2 as ValidationFault).FieldName, "SessionToken"), "Wrong validation field name returned");
            #endregion Fault2-SessionToken
            
            #region Fault3-CallerId
            // Fault properties check
            Assert.IsTrue(string.Equals(fault3.Origin, "InnovationPortal"), "Wrong fault origin returned");
            Assert.IsTrue(string.Equals(fault3.ReturnCode, "FieldValidationError"), "Wrong fault return code returned");
            Assert.IsTrue(string.Equals(fault3.DebugStatusText, "Field has wrong length"), "Wrong fault status text returned");

            // ValidationFault properties check
            Assert.IsTrue(string.Equals((fault3 as ValidationFault).ErrorType, "Length"), "Wrong validation error type returned");
            Assert.IsTrue(string.Equals((fault3 as ValidationFault).FieldName, "CallerId"), "Wrong validation field name returned");
            #endregion Fault3-CallerId

            #endregion Assert
        }

        [TestMethod]
        public void IsValidTest_ShouldReturnFaultInvalidCredentials()
        {
            AccessCredentials credential = new AccessCredentials();
            credential.CallerId = "TestCallerId";
            credential.UserID = 100;
            credential.SessionToken = "TestSessionToken";

            UserAuthentication nullAuth = null;
            queryUtilsMock.Setup(x => x.GetHPPToken(It.IsAny<IIdeaDatabaseDataContext>(),
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(nullAuth);

            GetProfileResponse response = new GetProfileResponse();

            bool result = credential.IsValid(response);

            Assert.IsFalse(result);
            Assert.IsTrue(response.ErrorList.Count == 1);
            Assert.IsTrue(response.ErrorList.ElementAt(0).ReturnCode.Equals(Faults.InvalidCredentials.ReturnCode));
        }
        #endregion Negative Scenarios
    }
}
