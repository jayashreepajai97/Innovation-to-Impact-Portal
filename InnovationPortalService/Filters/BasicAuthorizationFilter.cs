using Credentials;
using IdeaDatabase.Credentials;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using Responses;
using SettingsRepository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Threading;
using System.Threading.Tasks;
using Filters;
using IdeaDatabase.Utils;
using InnovationPortalService.Controllers;
using NLog;
using InnovationPortalService.Utils;
using System.Text;

namespace InnovationPortalService.Filters
{
    public class BasicAuthorizationFilter : System.Web.Http.AuthorizeAttribute
    {
        private static ILogger log = LogManager.GetLogger($"CDAXNotificationLog");        

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                base.OnAuthorization(actionContext);
                if (actionContext.ActionDescriptor.GetCustomAttributes<BasicAuthorizationEnableAttribute>().Count == 0)
                {
                    return;
                }

                var authHeader = actionContext.Request.Headers.Authorization;

                if (authHeader != null)
                {
                    var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                    var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                    var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                    var userName = usernamePasswordArray[0];
                    var password = usernamePasswordArray[1];

                    string userNameFromDB = SettingRepository.Get<string>("CDAXNotficationAuthUserName");
                    string passwordFromDB = SettingRepository.Get<string>("CDAXNotficationAuthPassword");
        
        
                    if (userName == userNameFromDB && password == passwordFromDB)
                    {
                        log.Debug("/cases/status: credentials accepted");
                        IsAuthorized(actionContext);
                        return;
                    }

                }
            }
            catch(Exception)
            {
            }
            log.Error("/cases/status: wrong or missing credentials");
            NotAuthorized(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            base.IsAuthorized(actionContext);
            return true;
        }
        static void NotAuthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            // do not request password from browser
            //actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
        }
    }
}
