﻿using Hpcs.DependencyInjector;
using IdeaDatabase.Credentials;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Utils;
using InnovationPortalService.Filters;
using InnovationPortalService.Responses;
using InnovationPortalService.Utils;
using Responses;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using InnovationPortalService.Contract.Utils;
using SettingsRepository;
using System;

namespace InnovationPortalService.Controllers
{
    [HPIDEnableAttribute]
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class RESTAPIAuthController : RESTAPIControllerBase
    {
        private ICustomerHPIDUtils AuthUtils = DependencyInjector.Get<ICustomerHPIDUtils, CustomerHPIDUtils>();
        private ICustomerHPIDUtils HPIDUtils = DependencyInjector.Get<ICustomerHPIDUtils, CustomerHPIDUtils>();



        /// <summary>
        /// Exchanges access code obtained from central forms for a session token generated by the Profile API. The refresh token from HPID is stored in DB and used subsequently for authenticating operations in Methone and HPID.
        /// </summary>
        /// <remarks>No authentication required</remarks>
        /// <param name="req">Request must contain accesscode and redirecturl. Other parameters are optional.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        [HttpPost]
        [Route("authenticate")]
        [SwaggerOperation(Tags = new[] { "Profile" })]
        public RESTAPILoginResponse Authenticate(RESTAPIAuthCredentials req)
        {
            UserAuthenticationInterchange hppAuthInterchange = new UserAuthenticationInterchange()
            {
                CallerId = req.CallerId,
                LanguageCode = req.LanguageCode,
                CountryCode = req.CountryCode,            
                Platform = req.Platform,
                ClientId = req.ClientId,
                UserId = req.UserId
            };

            SetClientAppInfo(req.Platform, hppAuthInterchange);

            GetProfileResponse response = new GetProfileResponse();

            if (SettingRepository.Get<bool>("TESTLoginEnabled", true))
            {
                //response = HPIDUtils.GetCustomerProfileByTestLogin(hppAuthInterchange, false, APIMethods.None);
                response = HPIDUtils.GetCustomerProfileByDefaultUserLogin(response,hppAuthInterchange, false, APIMethods.None);
            }
            else {
                response = AuthUtils.GetCustomerProfileByAuthentication(hppAuthInterchange, false, req.AccessCode, req.RedirectUrl, APIMethods.POSTAuthenticate, req.ClientId);
            }

            RESTAPILoginResponse loginResponse = new RESTAPILoginResponse();
            if (response.ErrorList.Count == 0)
            {
                loginResponse.UserID = response.Credentials.UserID;
                loginResponse.SessionToken = response.Credentials.SessionToken;
                loginResponse.CallerId = response.Credentials.CallerId;
                loginResponse.Roles = response.Credentials.Roles;
                loginResponse.Locale = response.CustomerProfileObject.Locale;
                loginResponse.FirstName = response.CustomerProfileObject.FirstName;
                loginResponse.LastName = response.CustomerProfileObject.LastName;
                loginResponse.Emailaddress = response.CustomerProfileObject.EmailAddress;
                loginResponse.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);

                //if (string.IsNullOrEmpty(req.DeviceToken))
                //{
                //    loginResponse.ErrorList.Add(Faults.EmptyOrNullDevicetoken);
                //}                
                if (response.LoginDate.HasValue)
                {
                    //loginResponse.TimeOut = DateTimeFormatUtils.GetIso8601String(response.LoginDate.Value.AddMinutes(SettingRepository.Get<int>("MaxSessionTimeMinutes", 60)));
                }
            }
            else
            {
                loginResponse.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                loginResponse.ErrorList = response.ErrorList;
            }
             
            return loginResponse;
        }
        private static void SetClientAppInfo(string platform, UserAuthenticationInterchange hppAuthInterchange)
        {
            if (string.IsNullOrEmpty(platform))
                return;

            RESTAPIPlatform enumPlatform;
            bool isEnum = System.Enum.TryParse(platform, out enumPlatform);

            if (!isEnum)
                return;

            switch (enumPlatform)
            {
                case RESTAPIPlatform.IOS:
                case RESTAPIPlatform.ios:
                case RESTAPIPlatform.Android:
                case RESTAPIPlatform.android:
                case RESTAPIPlatform.web:
                    hppAuthInterchange.ClientApplication = "InnovationWebPortal";
                    hppAuthInterchange.ClientVersion = "1.0.0.0";
                    break;              
                default:
                    break;
            }
        }

        /// <summary>
        /// Invalidates the users HPID session. This requires the user to login on that device for using the active session.
        /// </summary>
        /// <remarks>Authentication required</remarks>
        /// <response code="200">Successfully processed - Check FaultItemList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        [HttpPost]
        [Route("logout")]
        [CredentialsHeaderAttribute]
        [AllowEmptyBodyAttribute]
        [SwaggerOperation(Tags = new[] { "Profile" })]
        public ResponseBase Logout()
        {
            ResponseBase response = new ResponseBase();
            HPIDUtils.ExecuteLogout(response, UserID, SessionToken, CallerId);
            return response;
        }

        [HttpOptions]
        [Route("authenticate")]
        [SwaggerOperation(Tags = new[] { "Profile" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void Authenticate() { }

        [HttpOptions]
        [Route("logout")]
        [SwaggerOperation(Tags = new[] { "Profile" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void LogoutOptions() { }
    }
}
