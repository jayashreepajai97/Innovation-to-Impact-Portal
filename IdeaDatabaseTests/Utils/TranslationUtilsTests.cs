using Microsoft.VisualStudio.TestTools.UnitTesting;
using IdeaDatabase.Utils;

namespace DeviceDatabaseTests.Utils
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TranslationUtilsTests
    {        
        [TestMethod]
        public void TestLocale_NullLanguageCodeInvalidCountryCode_ShouldReturnSameCountryCodeUpperCase()
        {
            string languageCode = null;
            string countryCode = "xy";

            string newLocale = TranslationUtils.Locale(languageCode, countryCode);

            Assert.AreEqual(newLocale, countryCode.ToUpper(), 
                "Invalid country code returned. Expected was the input country code");
        }

        [TestMethod]
        public void TestLocale_InvalidLanguageCodeNullCountryCode_ShouldReturnSameLanguageCodeLowerCase()
        {
            string languageCode = "XY";
            string countryCode = null;

            string newLocale = TranslationUtils.Locale(languageCode, countryCode);

            Assert.AreEqual(newLocale, languageCode.ToLower(),
                "Invalid language code returned. Expected was the input language code");
        }

        [TestMethod]
        public void TestSplitLocale_CountryOnlyLocale_ShouldReturnDefaultLanguageCountryCode()
        {
            string inputLocale = "en";
            string languageCode;
            string countryCode;

            TranslationUtils.SplitLocale(inputLocale, out languageCode, out countryCode);

            Assert.AreEqual(languageCode, "en", "Invalid language code returned");
            Assert.AreEqual(countryCode, "US", "Invalid country code returned");
        }

        [TestMethod]
        public void TestSplitLocale_NullLocale_ShouldReturnDefaultLanguageCountryCode()
        {
            string inputLocale = null;
            string languageCode;
            string countryCode;

            TranslationUtils.SplitLocale(inputLocale, out languageCode, out countryCode);

            Assert.AreEqual(languageCode, "en", "Invalid language code returned");
            Assert.AreEqual(countryCode, "US", "Invalid country code returned");
        }

        [TestMethod]
        public void TestSplitLocale_ValidLocale_ShouldReturnValidLanguageCountryCode()
        {
            string inputLanguageCode = "XY";
            string inputCountryCode = "ab";
            string inputlocale = $"{inputLanguageCode}-{inputCountryCode}";
            string languageCode;
            string countryCode;

            TranslationUtils.SplitLocale(inputlocale, out languageCode, out countryCode);

            Assert.AreEqual(languageCode, inputLanguageCode.ToLower(), "Invalid language code returned");
            Assert.AreEqual(countryCode, inputCountryCode.ToUpper(), "Invalid country code returned");
        }
    }
}
