using SettingsRepository;

namespace Responses
{
    public class ConfigurationUtils
    {
        private static bool _showOnlyOneServerIsBusyError;
        private static bool _showStackTrace;
        private static bool _captchaRequired;
        private static bool _legacyAPI;

        private static bool _isShowOnlyOneServerIsBusyErrorLoaded = false;
        private static bool _isShowStackTraceLoaded = false;
        private static bool _isCaptchaRequiredLoaded = false;
        private static bool _isLegacyAPILoaded = false;

        public static bool ShowOnlyOneServerIsBusyError
        {
            get
            {
                if (!_isShowOnlyOneServerIsBusyErrorLoaded)
                    return _showOnlyOneServerIsBusyError = loadConfiguration("ShowOnlyOneServerIsBusyError", true);
                else
                    return _showOnlyOneServerIsBusyError;
            }
        }
        public static bool ShowStackTrace
        {
            get
            {
                if (!_isShowStackTraceLoaded)
                    return _showStackTrace = loadConfiguration("ShowStackTrace", false);
                else
                    return _showStackTrace;
            }
        }

        public static bool CaptchaRequired
        {
            get
            {
                if (!_isCaptchaRequiredLoaded)
                    return _captchaRequired = loadConfiguration("CaptchaEnabled", false);
                else
                    return _captchaRequired;
            }
        }
        public static bool LegacyAPI
        {
            get
            {
                if (!_isLegacyAPILoaded)
                    return _legacyAPI = loadConfiguration("LegacyAPI", true);
                else
                    return _legacyAPI;
            }
        }

        public static bool loadConfiguration(string name, bool defaultValue = true)
        {
            if (SettingRepository.IsSettingRepositoryLoaded)
            {
                SetIsLoadedFlag(name);
                return SettingRepository.Get<bool>(name, defaultValue);
            }
            else
                return defaultValue;
        }

        private static void SetIsLoadedFlag(string name)
        {
            switch (name)
            {
                case "ShowOnlyOneServerIsBusyError":
                    _isShowOnlyOneServerIsBusyErrorLoaded = true;
                    break;
                case "ShowStackTrace":
                    _isShowStackTraceLoaded = true;
                    break;
                case "CaptchaEnabled":
                    _isCaptchaRequiredLoaded = true;
                    break;
                case "LegacyAPI":
                    _isLegacyAPILoaded = true;
                    break;
                default:
                    break;
            }
        }
    }
}