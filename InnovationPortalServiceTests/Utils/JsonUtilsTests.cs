using InnovationPortalService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovationPortalServiceTests.Utils
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class JsonUtilsTests
    {
        private const string jsonPayload
            = "{\"EmpId\":\"123456\", \"Name\":\"Hello World\", \"Grade\":\"A\", \"Address\": {\"City\":\"Bangalore\", \"State\":\"Karnataka\", \"Country\":\"IN\"}}";
        private const string jsonPayload_EmptyValue
            = "{\"EmpId\":\"123456\", \"Name\":\"     \", \"Grade\":\"A\", \"Address\": {\"City\":\"Bangalore\", \"State\":\"Karnataka\", \"Country\":\"IN\"}}";
        private const string jsonPayload_MissingElement
            = "{\"EmpId\":\"123456\", \"Grade\":\"A\", \"Address\": {\"City\":\"Bangalore\", \"State\":\"Karnataka\", \"Country\":\"IN\"}}";
        private const string jsonPayload_WithArray
            = "{\"EmpId\":\"123456\", \"Grade\":\"A\", \"Contact\":[{},{\"ContactType\":\"Secondary\",\"EmailAddress\":\"johny.walker@pluto.com\"}]}";
        private const string jsonPayload_MissingArrayElements
            = "{\"EmpId\":\"123456\", \"Grade\":\"A\", \"Contact\":[{},{}]}";

        [TestMethod()]
        public void Test_RemoveField_ShouldReturnJsonWithoutTheGivenField()
        {
            #region Arrange
            JObject inObj = JObject.Parse(jsonPayload);
            string[] fieldsToRemove = new string[] { "Grade", "EmpId" };
            #endregion Arrange

            #region Act
            JObject objWithoutFields = JsonUtils.RemoveField(inObj, fieldsToRemove);
            #endregion Act

            #region Assert
            Assert.IsTrue(inObj.Children().Count() == 4, "The original object should not have been modified");
            Assert.IsTrue(objWithoutFields.Children().Count() == 2, "Invalid number of child nodes found in the output");

            JProperty prop = objWithoutFields.Property(fieldsToRemove[0]);
            Assert.IsTrue(prop == null, "The field shouldn't have been present in the output");
            prop = objWithoutFields.Property(fieldsToRemove[1]);
            Assert.IsTrue(prop == null, "The field shouldn't have been present in the output");
            #endregion Assert
        }

        [TestMethod()]
        public void Test_RemoveField_ShouldRemoveOnlyFromRoot()
        {
            #region Arrange
            string jsonPayload = "{\"EmpId\":\"123456\", \"Name\":\"Hello World\", \"Grade\":\"A\",\"City\":\"Pune\", \"Address\": {\"City\":\"Bangalore\", \"State\":\"Karnataka\", \"Country\":\"IN\"}}";
            JObject inObj = JObject.Parse(jsonPayload);
            string[] fieldsToRemove = new string[] { "Grade", "City" };
            #endregion Arrange

            #region Act
            JObject objWithoutFields = JsonUtils.RemoveField(inObj, fieldsToRemove);
            #endregion Act

            #region Assert
            Assert.AreEqual(objWithoutFields?.SelectToken("City")?.Value<string>(), null);
            Assert.AreEqual(objWithoutFields?.SelectToken("Address.City")?.Value<string>(), "Bangalore");
            #endregion Assert
        }

        [TestMethod()]
        public void Test_RemoveField_ShouldRemoveWithJsonPath()
        {
            #region Arrange
            string jsonPayload = "{\"EmpId\":\"123456\", \"Name\":\"Hello World\", \"Grade\":\"A\",\"City\":\"Pune\", \"Address\": {\"City\":\"Bangalore\", \"State\":\"Karnataka\", \"Country\":\"IN\"}}";
            JObject inObj = JObject.Parse(jsonPayload);
            string[] fieldsToRemove = new string[] { "Grade", "Address.State" };
            #endregion Arrange

            #region Act
            JObject objWithoutFields = JsonUtils.RemoveField(inObj, fieldsToRemove);
            #endregion Act

            #region Assert
            Assert.AreEqual(objWithoutFields?.SelectToken("Address.City")?.Value<string>(), "Bangalore");
            Assert.AreEqual(objWithoutFields?.SelectToken("Address.State")?.Value<string>(), null);
            #endregion Assert
        }

        [TestMethod()]
        public void Test_ExtractField_ShouldReturnJsonWithOnlyTheGivenField()
        {
            #region Arrange
            JObject inObj = JObject.Parse(jsonPayload);
            string[] fieldsToExtract = new string[] { "Name", "EmpId" };
            #endregion Arrange

            #region Act
            JObject objWithExtratcedFields = JsonUtils.ExtractField(inObj, fieldsToExtract);
            #endregion Act

            #region Assert
            Assert.IsTrue(inObj.Children().Count() == 4, "The original object should not have been modified");
            Assert.IsTrue(objWithExtratcedFields.Children().Count() == 2, "Invalid number of child nodes found in the output");

            JProperty prop = objWithExtratcedFields.Property(fieldsToExtract[0]);
            Assert.IsTrue(prop != null, "The field should have been present in the output");
            prop = objWithExtratcedFields.Property(fieldsToExtract[1]);
            Assert.IsTrue(prop != null, "The field should have been present in the output");
            #endregion Assert
        }

        [TestMethod()]
        public void Test_DoesContainValidValue_ShouldReturnTruForValidValuesInArray()
        {
            #region Arrange
            JObject inObj = JObject.Parse(jsonPayload_WithArray);
            JArray contact = inObj["Contact"] as JArray;
            #endregion Arrange

            #region Act
            bool result = JsonUtils.DoesContainValidValue(contact);
            #endregion Act

            #region Assert
            Assert.IsTrue(result, "The Contact should have contained valid values");
            #endregion Assert
        }

        [TestMethod()]
        public void Test_DoesContainValidValue_ShouldReturnFalseForMissingValuesInArray()
        {
            #region Arrange
            JObject inObj = JObject.Parse(jsonPayload_MissingArrayElements);
            JArray contact = inObj["Contact"] as JArray;
            #endregion Arrange

            #region Act
            bool result = JsonUtils.DoesContainValidValue(contact);
            #endregion Act

            #region Assert
            Assert.IsFalse(result, "The Contact should not have contained valid values");
            #endregion Assert
        }

        [TestMethod()]
        public void Test_DoesContainValidStringValue_ShouldReturnFalseForEmptyField()
        {
            #region Arrange
            JObject inObj = JObject.Parse(jsonPayload_EmptyValue);
            #endregion Arrange

            #region Act
            bool result = JsonUtils.DoesContainValidStringValue(inObj, "Name");
            #endregion Act

            #region Assert
            Assert.IsFalse(result, "The Name should not have been present");
            #endregion Assert
        }

        [TestMethod()]
        public void Test_DoesContainValidStringValue_ShouldReturnFalseForMissingField()
        {
            #region Arrange
            JObject inObj = JObject.Parse(jsonPayload_MissingElement);
            #endregion Arrange

            #region Act
            bool result = JsonUtils.DoesContainValidStringValue(inObj, "Name");
            #endregion Act

            #region Assert
            Assert.IsFalse(result, "The Name should not have been present");
            #endregion Assert
        }
    }
}
