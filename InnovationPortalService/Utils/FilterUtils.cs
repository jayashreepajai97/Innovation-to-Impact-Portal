using IdeaDatabase.Enums;
using IdeaDatabase.Utils;
using InnovationPortalService.Controllers;
using NLog;
using NLog.Fluent;
using Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace InnovationPortalService.Utils
{
    public class FilterUtils
    {
        private static ILogger log = LogManager.GetLogger($"InnovationPortalServiceErrorLog");
        public void LogIntoAdmLogsTable(HttpRequestMessage request, HttpResponseMessage response, HttpActionContext actionContext, int? timer, APILogLevel ApiLogLevel,bool faultOccured, bool SkipErrorLogging)
        {

            if ((ApiLogLevel == APILogLevel.All) ||
                (ApiLogLevel == APILogLevel.Faults && faultOccured == true && SkipErrorLogging == false))
            {
                AdmLog.LogApiCallDetails(request?.Headers?.Authorization?.ToString(),
              request?.Method?.Method,
              request?.RequestUri?.ToString(),
              GetRequest(actionContext),
              response?.Content?.ReadAsStringAsync()?.Result,
              timer);
            }
        }

        private string GetRequest(HttpActionContext context)
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
        public void Translation(HttpActionContext actionContext)
        {
            try
            {
                RESTAPIControllerBase baseController = actionContext.ControllerContext.Controller as RESTAPIControllerBase;

                //Return if no-rest api filter.
                if (baseController == null)
                    return;
                string CountryCode = TranslationUtils.DefaultCountryCode;
                string LanguageCode = TranslationUtils.DefaultLanguageCode;

                if (baseController.CountryCode != null || baseController.LanguageCode != null)
                {
                    //Get CountryCode, LanguageCode
                    CountryCode = baseController.CountryCode;
                    LanguageCode = baseController.LanguageCode;
                }

                //We only modify for non-test http method.
                if (actionContext.Request.Method != HttpMethod.Options)
                {
                    ObjectContent content = actionContext?.Response?.Content as ObjectContent;
                    ResponseBase response = content?.Value as ResponseBase;

                    //Check if response has any error
                    if (response != null && response.ErrorList.Count > 0)
                    {
                        //Get translation for fault item.
                        foreach (Fault item in response.ErrorList)
                        {
                            item.StatusText = string.Format($"ErrorCategory_{item.ErrorCategory.ToString()}", TranslationUtils.Locale(LanguageCode, CountryCode));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format($"ErrorCodeTranslationFilter | Error occured while localization of error code.\n{ex.ToString()}"));
            }
        }
    }
}