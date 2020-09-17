using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SettingsRepository;
using System.Collections.Generic;

namespace SettingsRepositoryTests
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SettingRepositoryTests
    {

        [TestMethod]
        public void GetSuccess()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", StringValue = "ParmString" } });
            string res = SettingRepository.Get<string>("ParmKey");
            Assert.AreEqual(res, "ParmString");
        }

        [TestMethod]
        public void GetStringFromDecimal()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", NumValue = 99.9m } });
            string res = SettingRepository.Get<string>("ParmKey");
            Assert.AreEqual(res, "99.9");
        }

        [TestMethod]
        public void GetStringFromDateTime()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", DateValue = new DateTime(2018, 11, 19) } });
            string res = SettingRepository.Get<string>("ParmKey");
            Assert.AreEqual(res, new DateTime(2018, 11, 19).ToString());
        }        

        [TestMethod]
        public void TryGetSuccess()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", StringValue = "ParmString" } });

            string strVal;
            bool res = SettingRepository.TryGet<string>("ParmKey", out strVal);
            Assert.AreEqual(res, true);
            Assert.AreEqual(strVal, "ParmString");
        }

        [TestMethod]
        public void TryGetStringFromDecimal()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", NumValue = 99.9m } });

            string strVal;
            bool res = SettingRepository.TryGet<string>("ParmKey", out strVal);
            Assert.AreEqual(res, true);
            Assert.AreEqual(strVal, "99.9");
        }

        [TestMethod]
        public void TryGetDecimalFromString()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", StringValue = "ParmString" } });
            
            decimal numVal;
            bool res = SettingRepository.TryGet<decimal>("ParmKey", out numVal);
            Assert.AreEqual(res, false);
            Assert.AreEqual(numVal, 0);
        }

        [TestMethod]
        public void TryGetStringFromDateTime()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", DateValue = new DateTime(2018, 11, 19) } });
            
            string strVal;
            bool res = SettingRepository.TryGet<string>("ParmKey", out strVal);
            Assert.AreEqual(res, true);
            Assert.AreEqual(strVal, new DateTime(2018, 11, 19).ToString());
        }

        [TestMethod]
        public void TryGetDateTimeFromString()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", StringValue = "ParmString" } });
           
            DateTime dateVal;
            bool res = SettingRepository.TryGet<DateTime>("ParmKey", out dateVal);
            Assert.AreEqual(res, false);
            Assert.AreEqual(dateVal, default(DateTime));
        }

        [TestMethod]
        public void TryGetBooleanFromString()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", StringValue = "false" } });

            bool boolVal;
            bool res = SettingRepository.TryGet<bool>("ParmKey", out boolVal);
            Assert.AreEqual(res, true);
            Assert.AreEqual(boolVal, false);
        }

        [TestMethod]
        public void TryGetBooleanFromStringFail()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", StringValue = "boolVal" } });

            bool boolVal;
            bool res = SettingRepository.TryGet<bool>("ParmKey", out boolVal);
            Assert.AreEqual(res, false);
            Assert.AreEqual(boolVal, false);
        }


        [TestMethod]
        public void GetDefaultSuccess()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>() { new AdmSettings() { ParamName = "ParmKey", StringValue = "" } });

            string res = SettingRepository.Get<string>("ParmKey", "Default");
            Assert.AreEqual(res, "Default");
        }

    }
}
