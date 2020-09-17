using Hpcs.DependencyInjector;
using InnovationPortalService.HPID;
using InnovationPortalService.Utils;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace InnovationPortalService.Filters
{
    public class ActiveHPIDTokenAuthorizationFilter : AuthorizeAttribute
    {
        private IAUTHUtils hpidUtils = DependencyInjector.Get<IAUTHUtils, HPIDUtils>();
        
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            ResponseBase r = new ResponseBase();
            var Controller = actionContext.ControllerContext.Controller as Controllers.RESTAPIControllerBase;
            try
            {
                if (hpidUtils.GetProfile(Controller.Token) == null)
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