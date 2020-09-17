using Microsoft.VisualStudio.TestTools.UnitTesting;
using SettingsRepository;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IdeaDatabase.Utils.Tests
{
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RetryCounterTests
    {
        // static class "reset" ... nah..
        private void classReset()
        {
            Type staticType = typeof(RetryCounter);
            ConstructorInfo ci = staticType.TypeInitializer;
            ci.Invoke(null, null);
        }

        [TestMethod()]
        public void getCounterTestGetDefault()
        {
            classReset();
            Assert.IsTrue(RetryCounter.DbRetryCounter == RetryCounter.defaultCounter);
        }

        [TestMethod()]
        public void getCounterTestGetAppSettingIncorrectValue()
        {            
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
            {
                new AdmSettings { ParamName = "DatabaseRetryCounter", StringValue = "not-a-number" }
            });
            classReset();
            Assert.IsTrue(RetryCounter.DbRetryCounter == RetryCounter.defaultCounter);
        }

        [TestMethod()]
        public void getCounterTestGetAppSettingNegativeValue()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
            {
                new AdmSettings { ParamName = "DatabaseRetryCounter", StringValue = "-5" }
            });
            classReset();
            Assert.IsTrue(RetryCounter.DbRetryCounter == RetryCounter.defaultCounter);
        }

        [TestMethod()]
        public void getCounterTestGetAppSettingZeroValue()
        {
            SettingRepository.SetSettingsRepositoryData(new List<AdmSettings>()
            {
                new AdmSettings { ParamName = "DatabaseRetryCounter", StringValue = "0" }
            });
            classReset();
            Assert.IsTrue(RetryCounter.DbRetryCounter == RetryCounter.defaultCounter);
        }

    }
}