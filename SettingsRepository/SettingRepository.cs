using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Configuration;

namespace SettingsRepository
{
    public static class SettingRepository
    {
        private static List<AdmSettings> setting;
        public static bool IsSettingRepositoryLoaded = false;
        private static string ConfigVersion = WebConfigurationManager.AppSettings["Version"];
        public static void SetSettingsRepositoryData(List<AdmSettings> _setting)
        {
            setting = _setting;
            IsSettingRepositoryLoaded = true;
        }

        public static bool TryGet<T>(string keyName, out T value)
        {
            AdmSettings setting = GetAdmSettingByKey(keyName);

            try
            {
                if (!string.IsNullOrWhiteSpace(setting?.StringValue))
                {
                    value = (T)Convert.ChangeType(setting.StringValue, typeof(T));
                    return true;
                }
                else if (setting?.NumValue != null)
                {
                    value = (T)Convert.ChangeType(setting.NumValue, typeof(T));
                    return true;
                }
                else if (setting?.DateValue != null)
                {
                    value = (T)Convert.ChangeType(setting.DateValue, typeof(T));
                    return true;
                }
            }
            catch
            {

            }

            value = default(T);
            return false;
        }

        public static T Get<T>(string keyName)
        {
            AdmSettings setting = GetAdmSettingByKey(keyName);

            if (!string.IsNullOrWhiteSpace(setting?.StringValue))
            {
                return (T)Convert.ChangeType(setting.StringValue, typeof(T));
            }
            else if (setting?.NumValue != null)
            {
                return (T)Convert.ChangeType(setting.NumValue, typeof(T));
            }
            else if (setting?.DateValue != null)
            {
                return (T)Convert.ChangeType(setting.DateValue, typeof(T));
            }

            return default(T);
        }

        public static T Get<T>(string keyName, T defaultValue)
        {
            AdmSettings setting = GetAdmSettingByKey(keyName);

            try
            {
                if (!string.IsNullOrWhiteSpace(setting?.StringValue))
                {
                    return (T)Convert.ChangeType(setting.StringValue, typeof(T));
                }
                else if (setting?.NumValue != null)
                {
                    return (T)Convert.ChangeType(setting.NumValue, typeof(T));
                }
                else if (setting?.DateValue != null)
                {
                    return (T)Convert.ChangeType(setting.DateValue, typeof(T));
                }
            }
            catch { }

            return defaultValue;
        }

        public static T Get<T>(string keyName, string KeyType, T defaultValue)
        {
            AdmSettings setting = GetAdmSettingByKey(keyName, KeyType);

            try
            {
                if (!string.IsNullOrWhiteSpace(setting?.StringValue))
                {
                    return (T)Convert.ChangeType(setting.StringValue, typeof(T));
                }
                else if (setting?.NumValue != null)
                {
                    return (T)Convert.ChangeType(setting.NumValue, typeof(T));
                }
                else if (setting?.DateValue != null)
                {
                    return (T)Convert.ChangeType(setting.DateValue, typeof(T));
                }
            }
            catch { }

            return defaultValue;
        }

        private static AdmSettings GetAdmSettingByKey(string keyName)
        {
            if (setting == null || setting.Count == 0)
                return new AdmSettings();

            return GetSingleSetting(setting.Where(x => x.ParamName.Trim().ToUpper() == keyName.Trim().ToUpper()).ToList());
        }

        private static AdmSettings GetAdmSettingByKey(string keyName, string keyType)
        {
            if (setting == null || setting.Count == 0)
                return new AdmSettings();

            return GetSingleSetting(setting.Where(x => x.ParamName.Trim().ToUpper() == keyName.Trim().ToUpper() && x.ParamType.Trim().ToUpper() == keyType.Trim().ToUpper()).ToList());
        }

        private static AdmSettings GetSingleSetting(List<AdmSettings> settings)
        {
            AdmSettings setting = new AdmSettings();
            
            if (string.IsNullOrWhiteSpace(ConfigVersion) || settings.Where(x => x.Version == ConfigVersion).Count() == 0)
            {
                setting = settings.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Version));
            }
            else
            {
                foreach (AdmSettings set in settings)
                {
                    string[] versions = set?.Version?.Split(new char[] { ',' });
                    if (versions != null && versions.Contains(ConfigVersion))
                    {
                        setting = set;
                    }
                }
            }

            return setting;
        }
    }
}
