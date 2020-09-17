using System.IO;
using Newtonsoft.Json;
using SettingsRepository;
using NLog;

namespace InnovationPortalService.Filters
{
    public static class APIActionsFilter
    {
        private static ILogger log = LogManager.GetLogger($"InnovationPortalServiceErrorLog");

        private class APIActions
        {
            public RestrictedMethod[] RestrictedMethods { get; set; }
        }

        public class RestrictedMethod
        {
            public string ActionName { get; set; }
            public string Method { get; set; }
        }

        private static string APIActionsListFilePath = SettingRepository.Get<string>("APIActionsListFilePath", string.Empty);

        private static RestrictedMethod[] restrictedActions;

        public static RestrictedMethod[] RestrictedActions { get { return restrictedActions; } }

        static APIActionsFilter()
        {
            LoadActionsFromFile();
        }

        private static void LoadActionsFromFile()
        {
            if (!File.Exists(APIActionsListFilePath))
            {
                return;
            }
            try
            {
                string apiActionsJson = File.ReadAllText(APIActionsListFilePath);
                if (string.IsNullOrEmpty(apiActionsJson))
                {
                    return;
                }
                else
                {
                    APIActions apiActions = JsonConvert.DeserializeObject<APIActions>(apiActionsJson);
                    restrictedActions = apiActions.RestrictedMethods;
                }
            }
            catch (JsonException jex)
            {
                log.Error($"Method LoadActionsFromFile FilePath : {APIActionsListFilePath} | Error {jex} occured while reading restricted actions file");
            }
            catch (System.Exception ex)
            {
                log.Error($"Method LoadActionsFromFile FilePath : {APIActionsListFilePath} | Error {ex} occured while reading restricted actions file");
            }
        }
    }
}