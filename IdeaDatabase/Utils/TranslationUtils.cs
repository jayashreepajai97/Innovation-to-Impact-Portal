using NLog;
using System;
using Responses;
using System.Linq;
using SettingsRepository;
using System.Globalization;
using Hpcs.DependencyInjector;
using System.Collections.Generic;
using IdeaDatabase.DataContext;

namespace IdeaDatabase.Utils
{
    public static class TranslationUtils
    {
        public static string DefaultResourceSet = SettingRepository.Get<string>("TranslationDefaultResourceSet");
        private static ILogger log = LogManager.GetLogger($"InnovationPortalServiceErrorLog");

        public const string DefaultLanguageCode = "en";
        public const string DefaultCountryCode = "US";
        public const string DefaultEmptyText = "<EMPTY>";
        public static string DefaultLocale = $"{DefaultLanguageCode}-{DefaultCountryCode}";

        public static string SharedTranslations = "SharedTranslations";

        private static string ValidateLocalization(string localeId, string translateToken)
        {
            if (string.IsNullOrEmpty(localeId))
            {
                return localeId;
            }

            if (localeId.Length != 2 && localeId.Length != 5)
            {
                log.Error(string.Format("Invalid localization : token=\"{0}\", localeId=\"{1}\"", translateToken, localeId));
                return DefaultLocale;
            }

            try
            {
                CultureInfo ci = new CultureInfo(localeId);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Invalid localization : token=\"{0}\", localeId=\"{1}\", ex=\"{2}\"", translateToken, localeId, ex.Message));
                return DefaultLocale;
            }

            return localeId;
        }

        
 

        /// <summary>
        /// Checks Language and Country codes and builds proper 'Locale' string
        /// </summary>
        /// <param name="LanguageCode"></param>
        /// <param name="CountryCode"></param>
        /// <returns>If Language and Country are proper, then returns locale based on those values. Otherwise returns default 'en-US'</returns>
        public static string Locale(string LanguageCode, string CountryCode)
        {
            string ll = (!string.IsNullOrEmpty(LanguageCode) && LanguageCode.Length == 2)
                ? LanguageCode.ToLower() : null;
            string CC = (!string.IsNullOrEmpty(CountryCode) && CountryCode.Length == 2)
                ? CountryCode.ToUpper() : null;

            if (ll != null && CC != null) { return $"{ll}-{CC}"; }
            if (ll != null) { return ll; }
            if (CC != null) { return CC; }

            return DefaultLocale;
        }

        /// <summary>
        /// Splits the locale string in language and country code. Defaults to "en" and "US" if locale is not valid
        /// </summary>
        /// <param name="locale">The locale string to split</param>
        /// <param name="languageCode">Extracted language code from the input locale</param>
        /// <param name="countryCode">Extracted country code from the input locale</param>
        public static void SplitLocale(string locale, out string languageCode, out string countryCode)
        {
            if (string.IsNullOrEmpty(locale))
            {
                languageCode = DefaultLanguageCode;
                countryCode = DefaultCountryCode;
                return;
            }

            string[] languageCountryCode = locale.Split('-');

            if (languageCountryCode.Count() != 2)
            {
                languageCode = DefaultLanguageCode;
                countryCode = DefaultCountryCode;
                return;
            }

            languageCode = languageCountryCode[0].ToLower();
            countryCode = languageCountryCode[1].ToUpper();
        }

    }
}