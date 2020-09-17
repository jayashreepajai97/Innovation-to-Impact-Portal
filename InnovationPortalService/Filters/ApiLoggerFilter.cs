using IdeaDatabase.Enums;
using InnovationPortalService.Filters;
using InnovationPortalService.Utils;
using Responses;
using SettingsRepository;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace Filters
{
    public class ApiLoggerFilter : ActionFilterAttribute
    {
        private Stopwatch timer = new Stopwatch();
        private bool SkipErrorLogging = false;
        FilterUtils filterUtils = new FilterUtils();

        private string GetRequest(HttpActionExecutedContext context)
        {
            string request;
            using (var stream = context.Request.Content.ReadAsStreamAsync().Result)
            {
                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }
                request = context.Request.Content.ReadAsStringAsync().Result;
            }
            return request;
        }

        private void ConfigureSkipLogging(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<SkipLoggingAttribute>().Count > 0)
            {
                SkipLoggingAttribute skipLoggingAttrb = actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<SkipLoggingAttribute>().First();
                string error = skipLoggingAttrb.ErrorCode;

                if (!string.IsNullOrWhiteSpace(error))
                {
                    ObjectContent content = actionExecutedContext.ActionContext?.Response?.Content as ObjectContent;
                    ResponseBase response = content?.Value as ResponseBase;

                    if (response.ErrorList.Count == 1 && response.ErrorList.Any(x => x.ReturnCode == error))
                    {
                        SkipErrorLogging = true;
                    }
                }
                else
                {
                    SkipErrorLogging = true;
                }
            }
        }

        public override void OnActionExecuting(HttpActionContext actionExecutingContext)
        {
            base.OnActionExecuting(actionExecutingContext);
            timer.Reset();
            timer.Start();
            SkipErrorLogging = false;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            timer.Stop();

            ConfigureSkipLogging(actionExecutedContext);

            bool faultOccured = false;
            try
            {
                if (actionExecutedContext.Request.Method != HttpMethod.Options)
                {
                    // Check for presence of fault(s) in response or invalid return status code
                    {
                        ObjectContent content = actionExecutedContext.ActionContext?.Response?.Content as ObjectContent;
                        ResponseBase response = content?.Value as ResponseBase;
                        if (response != null && response.ErrorList.Count != 0)
                        {
                            faultOccured = true;
                        }

                        if (actionExecutedContext.Response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            faultOccured = true;
                        }
                    }

                    APILogLevel ApiLogLevel;
                    Enum.TryParse(SettingRepository.Get<string>("LogAPICalls", "None"), out ApiLogLevel);

 //                   filterUtils.LogIntoAdmLogsTable(actionExecutedContext.ActionContext.Request, actionExecutedContext.ActionContext.Response, actionExecutedContext.ActionContext, (int)timer.ElapsedMilliseconds, ApiLogLevel, faultOccured, SkipErrorLogging);

                }
            }

            catch (Exception)
            {
                // Ignore the exception in logging
            }

            base.OnActionExecuted(actionExecutedContext);
        }

    }
}