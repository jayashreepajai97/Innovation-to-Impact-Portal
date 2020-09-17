using NLog;
using System;
using System.Linq;
using System.Web.Http;
using SettingsRepository;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using InnovationPortalService.Filters;

namespace InnovationPortalService.Utils
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ServiceRESTApiExplorer : ApiExplorer
    {
        private static bool NewRESTApi = SettingRepository.Get<bool>("NewRESTAPI", false);
        private static ILogger log = LogManager.GetLogger($"InnovationPortalServiceErrorLog");

        public ServiceRESTApiExplorer(HttpConfiguration configuration) : base(configuration)
        {
            if (APIActionsFilter.RestrictedActions != null)
            {
                foreach (var restrictedAction in APIActionsFilter.RestrictedActions)
                {
                    configuration.Routes.IgnoreRoute($"{restrictedAction.Method}-{restrictedAction.ActionName}", restrictedAction.ActionName);
                }
            }
        }

        public override bool ShouldExploreAction(string actionVariableValue, HttpActionDescriptor actionDescriptor, IHttpRoute route)
        {
            bool IsValid = true;

            if (APIActionsFilter.RestrictedActions == null)
                return IsValid;

            try
            {
                //string ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;     //"RESTAPIKaaS"
                string RouteName = ((HttpRoute)route)?.RouteTemplate?.ToString();                   
                string Method = actionDescriptor?.SupportedHttpMethods[0].Method;                   //"Get"

                int? restrictedMethods = APIActionsFilter.RestrictedActions?.Where(x =>
                        String.Compare(x.ActionName, RouteName, StringComparison.OrdinalIgnoreCase) == 0 &&
                        String.Compare(x.Method, Method, StringComparison.OrdinalIgnoreCase) == 0)?.Count();

                if (restrictedMethods != null && restrictedMethods > 0)
                    return false;
            }
            catch (Exception ex)
            {
                log.Error($"ShouldExploreAction ({actionVariableValue ?? string.Empty}) | Exception occured while filtering action.\n{ex.ToString()}");
            }

            return IsValid;
        }

        public override bool ShouldExploreController(string controllerVariableValue, HttpControllerDescriptor controllerDescriptor, IHttpRoute route)
        {
            var hpidAttribute = controllerDescriptor.GetCustomAttributes<HPIDEnableAttribute>().FirstOrDefault();
            if (hpidAttribute != null)
            {
                return SettingRepository.Get<bool>("HPIDEnabled", true);
            }

            var swaggerAttribute = controllerDescriptor.GetCustomAttributes<SwaggerEnableAttribute>().FirstOrDefault();
            if (swaggerAttribute != null)
            {
                return NewRESTApi;
            }
            return true;
        }
    }
}