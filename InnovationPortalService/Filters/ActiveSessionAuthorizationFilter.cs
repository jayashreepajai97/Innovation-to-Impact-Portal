using IdeaDatabase.DataContext;
using IdeaDatabase.Utils;
using Hpcs.DependencyInjector;
using InnovationPortalService.Utils;
using Responses;
using SettingsRepository;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace InnovationPortalService.Filters
{
    public class ActiveSessionAuthorizationFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            ResponseBase r = new ResponseBase();
            var Controller = actionContext.ControllerContext.Controller as Controllers.RESTAPIControllerBase;
            try
            {
                UserAuthentication auth = new UserAuthentication();
                DatabaseWrapper.databaseOperation(r, (context, query) => { auth = query.GetHPPToken(context, Controller.UserID, Controller.CallerId); }, readOnly: true);
                int maxSessionTime = SettingRepository.Get<int>("MaxSessionTimeMinutes", 60);

                if (auth.CreatedDate !=null || auth.CreatedDate.AddMinutes(maxSessionTime) < DateTime.UtcNow)
                {
                    this.ExecuteLogout(actionContext, r, Controller);
                    return;
                }
                if (r.ErrorList.Count == 0)
                {
                    IsAuthorized(actionContext);
                }
                return;
            }
            catch (Exception)
            {
                this.ExecuteLogout(actionContext, r, Controller);
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            base.IsAuthorized(actionContext);
            return true;
        }
    }
}