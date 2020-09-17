using NLog;
using System.Web.Http.Filters;
using InnovationPortalService.Utils;

namespace InnovationPortalService.Filters
{
    public class ErrorCodeTranslationFilter : ActionFilterAttribute
    {
        private static ILogger log = LogManager.GetLogger($"InnovationPortalServiceErrorLog");
        FilterUtils filterUtils = new FilterUtils();
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            filterUtils.Translation(actionExecutedContext.ActionContext);
        }
    }
}