using IdeaDatabase.Utils;
using Hpcs.DependencyInjector;
using InnovationPortalService.Utils;
using Responses;
using SettingsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace InnovationPortalService.Filters
{
    public static class ExtendedAuthorizedAttribute
    {
        public static void ExecuteLogout(this AuthorizeAttribute authAttribute, HttpActionContext actionContext, ResponseBase r, Controllers.RESTAPIControllerBase Controller)
        {
            Fault f = new Fault(Responses.Faults.HPIDSessionTimeout, null);
            f.StatusText = string.Format($"ErrorCategory_{f.ErrorCategory.ToString()}", TranslationUtils.Locale(Controller.LanguageCode, Controller.CountryCode));
            r.ErrorList.Add(f);

            if (SettingRepository.Get<bool>("LogoutOnNonAuthorizedRequest", false))
            {
                ICustomerHPIDUtils customerHPIDUtils = DependencyInjector.Get<ICustomerHPIDUtils, CustomerHPIDUtils>();
                customerHPIDUtils.ExecuteLogout(r, Controller.UserID, Controller.SessionToken, Controller.CallerId);
            }
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, r, GlobalConfiguration.Configuration);
        }
    }
}