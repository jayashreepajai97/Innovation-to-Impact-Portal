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
using IdeaDatabase.DataContext;

namespace InnovationPortalService.Filters
{
    public class CustomAuthorizationFilter : System.Web.Http.AuthorizeAttribute
    {
        private static ILogger log = LogManager.GetLogger($"InnovationPortalServiceErrorLog");         
        FilterUtils filterUtils = new FilterUtils();
        private void ValidateAppVersion(ResponseBase response, AccessCredentials credentials)
        {
            if (String.IsNullOrEmpty(credentials.Platform) || String.IsNullOrEmpty(credentials.AppVersion))
            {
                return;
            }

            if (!credentials.Platform.ToUpper().Equals(RESTAPIPlatform.web.ToString().ToUpper()))
            {
                // Application version validation is applied only 
                // for Android and iOS platforms as of now
                return;
            }          
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            bool faultOccured = false;
            ResponseBase r = new ResponseBase();
            try
            {
                base.OnAuthorization(actionContext);
                if (actionContext.ActionDescriptor.GetCustomAttributes<CredentialsHeaderAttribute>().Count == 0)
                {
                    return;
                }
                string credentialsValue = actionContext.Request.Headers.GetValues("Authorization").ElementAt(0);

                AccessCredentials credentials = new AccessCredentials();
                ParseAuthorizationHeader(credentialsValue, credentials, r);
               

                if (r.ErrorList.Count > 0)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, r, GlobalConfiguration.Configuration);
                    faultOccured = true;

                    return;
                }

                validateUserIdWithToken(r, credentials);

                if (!credentials.IsValid(r))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, r, GlobalConfiguration.Configuration);
                    faultOccured = true;

                    return;
                }



                var Controller = actionContext.ControllerContext.Controller as Controllers.RESTAPIControllerBase;
                Controller.UserID = credentials.UserID;
                Controller.SessionToken = credentials.SessionToken;
                Controller.CallerId = credentials.CallerId;
                Controller.LanguageCode = credentials.LanguageCode;
                Controller.CountryCode = credentials.CountryCode;
                Controller.Token = credentials.Token;                 
                Controller.LoginDate = credentials.LoginDate;

                if (credentials.Platform != null)
                {
                   var isPlatformDefined = Enum.IsDefined(typeof(RESTAPIPlatform), credentials.Platform);
                    if(isPlatformDefined)
                    {
                        Controller.ClientPlatform = (RESTAPIPlatform)Enum.Parse(typeof(RESTAPIPlatform), credentials.Platform);
                    }
                    else
                    {
                        Controller.ClientPlatform = RESTAPIPlatform.notsupported;
                    }
                }
                if (r.ErrorList.Count == 0)
                {
                    IsAuthorized(actionContext);
                }


                return;
            }
            catch (Exception)
            {
                r.ErrorList.Add(Faults.InvalidCredentials);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, r, GlobalConfiguration.Configuration);
                faultOccured = true;

            }
            finally
            {
                APILogLevel apiLogLevel;
                Enum.TryParse(SettingRepository.Get<string>("LogAPICalls", "None"), out apiLogLevel);
                if (faultOccured)
                {
                    filterUtils.Translation(actionContext);
                    filterUtils.LogIntoAdmLogsTable(actionContext.Request, actionContext.Response, actionContext, null, apiLogLevel, faultOccured, false);
                }

            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            base.IsAuthorized(actionContext);
            return true;
        }

        private void ParseAuthorizationHeader(string credentialsValue, AccessCredentials credentials, ResponseBase response)
        {
            const string CredentialRegExPattern = "SessionToken *= *\"([^\\s]*)\" *, *CallerId *= *\"([^\\s]*)\" *";
          
            Match match = Regex.Match(credentialsValue, CredentialRegExPattern);
            if (!match.Success)
            {
                response.ErrorList.Add(new FormatValidationFault("Authorization header is not in correct format", CredentialRegExPattern));
                return;
            }

            credentials.SessionToken = match.Groups[1].ToString();
            credentials.CallerId = match.Groups[2].ToString();            
            
        }

        private void validateUserIdWithToken(ResponseBase r,AccessCredentials credentials)
        {
            DatabaseWrapper.databaseOperation(r, (context, query) => {
                UserAuthentication userAuthentication = query.GetAuthenticationByToken(context, credentials.SessionToken, credentials.CallerId);
                if (userAuthentication == null)
                {
                  r.ErrorList.Add(new FormatValidationFault("Authorization header is not valid", "Authorizationheader"));
                    return;
                }

                 credentials.UserID= userAuthentication.UserId;                 
                 credentials.CallerId= userAuthentication.CallerId;               
                 credentials.Token=userAuthentication.Token;
                 credentials.LoginDate = userAuthentication.CreatedDate;
            }, readOnly: true);

        }

    }
}
