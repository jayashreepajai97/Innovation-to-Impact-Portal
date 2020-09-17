using SettingsRepository;

namespace IdeaDatabase.Utils
{
    public class RetryCounter
    {
        public static int defaultCounter = 10;
        private static int _dbRetryCounter = defaultCounter;
        private static int _obligationRetryCounter = defaultCounter;
        private static int _hPPRetryCounter = defaultCounter;
        private static int _gNRetryCounter = defaultCounter;
        private static int _sNRRetryCounter = defaultCounter;

        private static bool _isDbRetryCounterLoaded = false;
        private static bool _isObligationRetryCounterLoaded = false;
        private static bool _isHPPRetryCounterLoaded = false;
        private static bool _isGNRetryCounterLoaded = false;
        private static bool _isSNRRetryCounterLoaded = false;

        public static int DbRetryCounter
        {
            get
            {
                if (!_isDbRetryCounterLoaded)
                    return _dbRetryCounter = loadCounter("DatabaseRetryCounter");
                else
                    return _dbRetryCounter;
            }
        }
        public static int ObligationRetryCounter
        {
            get
            {
                if (!_isObligationRetryCounterLoaded)
                    return _obligationRetryCounter = loadCounter("ObligationRetryCounter");
                else
                    return _obligationRetryCounter;
            }
        }
        public static int HPPRetryCounter
        {
            get
            {
                if (!_isHPPRetryCounterLoaded)
                    return _hPPRetryCounter = loadCounter("HPPRetryCounter");
                else
                    return _hPPRetryCounter;
            }
        }
        public static int GNRetryCounter
        {
            get
            {
                if (!_isGNRetryCounterLoaded)
                    return _gNRetryCounter = loadCounter("GNRetryCounter");
                else
                    return _gNRetryCounter;
            }
        }

        public static int GNewtonRetryCounter
        {
            get
            {
                if (!_isGNRetryCounterLoaded)
                    return _gNRetryCounter = loadCounter("GNewtonRetryCounter");
                else
                    return _gNRetryCounter;
            }
        }

        public static int SNRRetryCounter
        {
            get
            {
                if (!_isSNRRetryCounterLoaded)
                    return _sNRRetryCounter = loadCounter("SNRRetryCounter");
                else
                    return _sNRRetryCounter;
            }
        }

        private static int loadCounter(string name)
        {
            int retryCounter;
            try
            {
                if (SettingRepository.IsSettingRepositoryLoaded)
                {
                    retryCounter = SettingRepository.Get<int>(name);
                    if (retryCounter <= 0)
                        retryCounter = defaultCounter;
                    SetIsLoadedFlag(name);
                }
                else
                    return defaultCounter;
            }
            catch
            {
                retryCounter = defaultCounter;
            }
            return retryCounter;
        }

        private static void SetIsLoadedFlag(string name)
        {
            switch (name)
            {
                case "DatabaseRetryCounter":
                    _isDbRetryCounterLoaded = true;
                    break;
                case "ObligationRetryCounter":
                    _isObligationRetryCounterLoaded = true;
                    break;
                case "HPPRetryCounter":
                    _isHPPRetryCounterLoaded = true;
                    break;
                case "GNRetryCounter":
                    _isGNRetryCounterLoaded = true;
                    break;
                case "SNRRetryCounter":
                    _isSNRRetryCounterLoaded = true;
                    break;
                default:
                    break;
            }
        }
    }
}