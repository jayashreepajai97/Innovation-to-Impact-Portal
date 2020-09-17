using Hpcs.DependencyInjector;
using IdeaDatabase.Credentials;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Utils;
using InnovationPortalService.Filters;
using InnovationPortalService.Requests;
using InnovationPortalService.Responses;
using InnovationPortalService.Utils;
using Responses;
using SettingsRepository;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http;
using IdeaDatabase.Responses;
using IdeaDatabase.Requests;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using System;

namespace InnovationPortalService.Controllers
{
    [HPIDEnableAttribute]
    public class RESTAPIProfileController : RESTAPIControllerBase
    {
        private ICustomerUtils customerUtils = DependencyInjector.Get<ICustomerUtils, CustomerUtils>();
        private IRoleUtils roleUtils = DependencyInjector.Get<IRoleUtils, RoleUtils>();
        private IStatusUtils statusUtils = DependencyInjector.Get<IStatusUtils, StatusUtils>();
        private ICustomerHPIDUtils HPIDUtils = DependencyInjector.Get<ICustomerHPIDUtils, CustomerHPIDUtils>();

        /// <summary>
        /// Allows login to HPID profile using username and password.
        /// </summary>
        /// <remarks>No authentication required</remarks>
        /// <param name="req">Request must contain deviceToken, username password other fields are optional. Other fields are optional</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Tags = new[] { "Restricted" })]
        public RESTAPILoginResponse Login(RESTAPILoginCredentials req)
        {
            GetProfileResponse response = new GetProfileResponse();
            UserAuthenticationInterchange hppAuthInterchange = new UserAuthenticationInterchange()
            {
                UserName = req.UserName,
                Password = req.Password,
                CallerId = req.CallerId,
                Token = null,
                LanguageCode = req.LanguageCode,
                CountryCode = req.CountryCode,
                UseCaseGroup = req.ClientViewer,
                Platform = req.Platform,
                ClientApplication = UserAuthenticationInterchange.MapPlatformToClientApplication(req.Platform)
            };
            if (SettingRepository.Get<bool>("TESTLoginEnabled", true))
            {
                response = HPIDUtils.GetCustomerProfileByTestLogin(hppAuthInterchange, false, APIMethods.None);
            }
            else
            {
                if (SettingRepository.Get<bool>("HPIDEnabled", true))
                {
                    response = HPIDUtils.GetCustomerProfileByLogin(hppAuthInterchange, false, APIMethods.None);
                }
                //else
                //{
                //    response = customerUtils.GetCustomerProfile(hppAuthInterchange, false);
                //}
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

                if (response.LoginDate.HasValue)
                {
                    //loginResponse.TimeOut = DateTimeFormatUtils.GetIso8601String(response.LoginDate.Value.AddMinutes(SettingRepository.Get<int>("MaxSessionTimeMinutes", 2)));
                }
                loginResponse.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
            }
            else
            {
                loginResponse.ErrorList = response.ErrorList;
                loginResponse.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
            }
            return loginResponse;
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

        /// <summary>
        /// This API is used for fetching Role.
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RESTAPIGetRolesResponse</returns>
        [HttpGet]
        [Route("roles")]
        [SwaggerOperation(Tags = new[] { "Restricted" })]
        [CredentialsHeader]
        public RESTAPIGetRolesResponse GetUserRoles()
        {
            RESTAPIGetRolesResponse response = new RESTAPIGetRolesResponse();
            statusUtils.GetRoles(response);
            return response;
        }

        /// <summary>
        /// This API is used for Adding Role.
        /// </summary>
        /// <param name="req">Request must required the parameter RoleName and In Authorization  Feild must requird SessionToken and CallerID which is generated by Authenticate API.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIAddRoleResponse</returns>
        [HttpPost]
        [Route("roles")]
        [SwaggerOperation(Tags = new[] { "Restricted" })]
        [CredentialsHeader]
        public RestAPIAddRoleResponse AddRole(RestAPIAddRolesRequest req)
        {
            RestAPIAddRoleResponse response = new RestAPIAddRoleResponse();
            roleUtils.InsertRole(response, req.RoleName);
            return response;
        }


        [HttpOptions]
        [Route("login")]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        [SwaggerOperation(Tags = new[] { "Restricted" })]
        public void Login() { }

        [HttpOptions]
        [Route("logout")]
        [SwaggerOperation(Tags = new[] { "Profile" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void LogoutOptions() { }

        [HttpOptions]
        [Route("roles")]
        [SwaggerOperation(Tags = new[] { "Restricted" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetuserRoles() { }

        [HttpOptions]
        [Route("addrole")]
        [SwaggerOperation(Tags = new[] { "Restricted" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void AddRole() { }



    }
}
