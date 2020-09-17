using System;
using Responses;
using Credentials;
using Newtonsoft.Json;
using SettingsRepository;
using IdeaDatabase.Enums;
using IdeaDatabase.Utils;
using InnovationPortalService.HPID;
using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;
using InnovationPortalService.Responses;
using Newtonsoft.Json.Serialization;
using static InnovationPortalService.HPID.HPIDUtils;
using System.Threading.Tasks;
using InnovationPortalService.Contract.Utils;
using IdeaDatabase.Utils.Interface;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Responses;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace InnovationPortalService.Utils
{
    public class CustomerHPIDUtils : ICustomerHPIDUtils
    {
        private IUserUtils userUtils = DependencyInjector.Get<IUserUtils, UserUtils>();
        private IAUTHUtils hpidUtils = DependencyInjector.Get<IAUTHUtils, HPIDUtils>();
        private IRoleUtils roleUtils = DependencyInjector.Get<IRoleUtils, RoleUtils>();
        private IStatusUtils statusUtils = DependencyInjector.Get<IStatusUtils, StatusUtils>();

        private static Logger logger = LogManager.GetLogger("InnovationPortalServiceLog");
   

        private ICustomerUtils customerUtils = DependencyInjector.Get<ICustomerUtils, CustomerUtils>();
      
        private static bool SynchronizeIsaacOnGetProfile = false;

        static CustomerHPIDUtils()
        {
            SettingRepository.TryGet<bool>("SynchronizeIsaacOnGetProfile", out SynchronizeIsaacOnGetProfile);
        }

        public GetProfileResponse GetCustomerProfileByAuthentication(UserAuthenticationInterchange UserAuthInterchange, bool RetainOldValues, string AccessCode, string RedirectUrl, APIMethods apiRetainOldValues, string ClientId = null)
        {
            GetProfileResponse response = new GetProfileResponse();
            if (string.IsNullOrEmpty(AccessCode) || string.IsNullOrEmpty(RedirectUrl))
            {
                response.ErrorList.Add(Responses.Faults.InvalidCredentials);
                return response;
            }

            TokenDetails sessionToken = hpidUtils.GetHPIDSessionToken((int)TokenScopeType.userAuthenticate, AccessCode, RedirectUrl, response, ClientId);
            if (response.ErrorList.Count > 0)
            {
                return response;
            }

            response = GetCustomerProfileforHPID(UserAuthInterchange, sessionToken, RetainOldValues, apiRetainOldValues);
            if (response.ErrorList.Count > 0)
            {
                foreach (var fault in response.ErrorList)
                {
                    var error = string.Format("origin={0},Return code={1},status Text={2}", fault.Origin, fault.ReturnCode, fault.DebugStatusText);
                    log.Debug(string.Format($"ProfileByAuth: Accesscode={AccessCode}, RedirectURL={RedirectUrl}, Exception={error}"));
                }
            }

            return response;
        }

        public void ExecuteLogout(ResponseBase response, int userID, string tokenMD5, string callerId)
        {
            //mobileUtils.DeleteMobileDevice(response, customerId, callerId);
            customerUtils.UpdateLogoutDate(response, userID, callerId, tokenMD5);            
        }

        public GetProfileResponse GetCustomerProfileforHPID(UserAuthenticationInterchange UserAuthInterchange, TokenDetails sessionTokenDetails, bool RetainOldValues, APIMethods apiRetainOldValues)
        {
            logger.Info($"GetCustomerProfileforHPID={sessionTokenDetails.AccessToken}");
            GetProfileResponse response = new GetProfileResponse();
            TokenDetails sessionToken = sessionTokenDetails;


            // try to get HPID profile with existing session token
            CustomerIds idS = new CustomerIds();
            if (GetProfileBySessionToken(response, sessionToken, idS))
            {
                return GetCustomerProfileFromHPIDAndDatabase(response, UserAuthInterchange, sessionToken, idS, RetainOldValues, apiRetainOldValues);
            }

            // if get profile failed not by expired token, but because of other errors, then do not use refresh token 
            if (!response.ErrorList.Contains(Responses.Faults.HPIDInvalidToken))
            {
                return response;
            }

            // if profile it recognized by access token, then do not use refresh token       
            if (sessionToken.tokenScopeType == TokenScopeType.apiProfileGetByTokenCall)
            {
                response.ErrorList.Clear();
                response.ErrorList.Add(Responses.Faults.InvalidCredentials);
                return response;
            }


            // try to get HPID profile with refresh token
            return GetProfileByRefreshToken(response, sessionToken, UserAuthInterchange.UserId, UserAuthInterchange.CallerId);
        }

        public GetProfileResponse GetCustomerProfileforHPIDTEST(UserAuthenticationInterchange UserAuthInterchange, TokenDetails sessionTokenDetails, bool RetainOldValues, APIMethods apiRetainOldValues)
        {
             
            GetProfileResponse response = new GetProfileResponse();
            AccessCredentials accessCredentials = new AccessCredentials();
            accessCredentials.AppVersion = "1.0.0.0";
            accessCredentials.CallerId = "TESTLOGIN1.0.0.0";
            accessCredentials.LoginDate = DateTime.UtcNow;
            response.Credentials = accessCredentials;
        
            CustomerProfile customerProfile = new CustomerProfile();
            CustomerData customerData= new CustomerData()
            {
                Locale =TranslationUtils.Locale(UserAuthInterchange.LanguageCode,UserAuthInterchange.CountryCode), ActiveHealth=true, Country = "US",
                CompanyName = "HP", DisplayName = "Ramdeo Angh", EmailAddress = "ramdeo.angh@hp.com",
                FirstName="Ramdeo",LastName="Angh"
            };
            customerProfile.Country = customerData.Country;
            customerProfile.CompanyName = customerData.CompanyName;
            customerProfile.DisplayName = customerData.DisplayName;
            customerProfile.EmailAddress = customerData.EmailAddress;
            customerProfile.ActiveHealth = customerData.ActiveHealth;
            customerProfile.Locale = customerData.Locale;
            customerProfile.FirstName = customerData.FirstName;
            customerProfile.LastName = customerData.LastName;




            response.CustomerProfileObject = customerProfile;
            response.LoginDate = DateTime.UtcNow;

          sessionTokenDetails = new TokenDetails();
            sessionTokenDetails.AccessToken =(Guid.NewGuid().ToString());
            sessionTokenDetails.RefreshToken =(Guid.NewGuid().ToString());
            sessionTokenDetails.RefreshTokenType =(int)TokenScopeType.userLogin;


            TokenDetails sessionToken = sessionTokenDetails;


            // try to get HPID profile with existing session token
            CustomerIds idS = new CustomerIds();
            idS.HPIDid = "INNOVATIONPORTAL12345!@#$%";
            
            return GetCustomerProfileFromHPIDAndDatabase(response, UserAuthInterchange, sessionToken, idS, RetainOldValues, apiRetainOldValues);           

        }



        private bool GetProfileBySessionToken(GetProfileResponse response, TokenDetails sessionToken, CustomerIds idS)
        {
            if (sessionToken == null || string.IsNullOrEmpty(sessionToken.AccessToken))
            {
                response.ErrorList.Add(Responses.Faults.InvalidCredentials);
                return false;
            }

            return hpidUtils.GetIdsAndProfile(idS, sessionToken.AccessToken, response);
        }

         

        private GetProfileResponse GetProfileByRefreshToken(GetProfileResponse response, TokenDetails sessionToken, int UserID, string calledId)
        {
            // try to get IsacUsers record and refresh token
            response.ErrorList.Clear();
            User isaacUser = userUtils.GetRefreshToken(calledId, sessionToken);
            if (isaacUser == null || string.IsNullOrEmpty(isaacUser.RefreshToken))
            {
                ExecuteLogout(response, UserID, QueryUtils.GetMD5(sessionToken.AccessToken), calledId);
                response.ErrorList.Add(Responses.Faults.HPIDSessionTimeout);
                return response;
            }


            // renew access token to HPID using refresh token
            sessionToken.RefreshToken = isaacUser.RefreshToken;
            sessionToken.RefreshTokenType = Convert.ToInt32(isaacUser.RefreshTokenType);

            //fetch clientId from DB
            UserAuthentication hppAuth = null;
            DatabaseWrapper.databaseOperation(response, (context, query) =>
             {
                 hppAuth = query.GetHPPToken(context, UserID, calledId);

             }, readOnly: true);

            string clientId = hppAuth?.ClientId;

            sessionToken = hpidUtils.GetHPIDSessionToken((int)TokenScopeType.userRefreshToken, "", sessionToken.RefreshToken, response, clientId, sessionToken.RefreshTokenType);

            if (sessionToken == null || string.IsNullOrEmpty(sessionToken.AccessToken))
            {
                ExecuteLogout(response, UserID, QueryUtils.GetMD5(sessionToken.AccessToken), calledId);
                response.ErrorList.Add(Responses.Faults.HPIDSessionTimeout);
                return response;
            }

            // try to get HPID profile using renewed access token
            CustomerIds idS = new CustomerIds();
            if (GetProfileBySessionToken(response, sessionToken, idS))
            {
                return GetProfileDataFromDatabase(response, isaacUser);
            }

            return response;
        }


        private GetProfileResponse GetCustomerProfileFromHPIDAndDatabase(GetProfileResponse response, UserAuthenticationInterchange hppAuthInterchange, TokenDetails sessionToken, CustomerIds idS, bool RetainOldValues, APIMethods apiRetainOldValues)
        {
            User profile = null;
            List<RoleMapping> roleMappings = null;
            try
            {
                // check is done based on profile, customerId is also generated for a new profile
                bool IsNewCustomer = false;

                RequestFindOrInsertHPIDProfile requestFindOrInsertHPID = new RequestFindOrInsertHPIDProfile();
                requestFindOrInsertHPID.Locale =
                    string.IsNullOrEmpty(response?.CustomerProfileObject?.Locale)?TranslationUtils.Locale(hppAuthInterchange.LanguageCode,hppAuthInterchange.CountryCode):response?.CustomerProfileObject?.Locale;
                requestFindOrInsertHPID.HPIDprofileId = idS.HPIDid;
                requestFindOrInsertHPID.HPPprofileId = idS.HPPid;
                requestFindOrInsertHPID.tokenDetails = sessionToken;
                requestFindOrInsertHPID.clientId = hppAuthInterchange.ClientId;
                requestFindOrInsertHPID.apiRetainOldValues = apiRetainOldValues;
                requestFindOrInsertHPID.EmailAddrees = response?.CustomerProfileObject?.EmailAddress;
                requestFindOrInsertHPID.CompanyName = response?.CustomerProfileObject?.CompanyName;
                requestFindOrInsertHPID.ActiveHealth = response.CustomerProfileObject.ActiveHealth;
                requestFindOrInsertHPID.FirstName = response?.CustomerProfileObject?.FirstName;
                requestFindOrInsertHPID.LastName = response.CustomerProfileObject.LastName;





                profile = userUtils.FindOrInsertHPIDProfile(response, requestFindOrInsertHPID, out IsNewCustomer);                

                if (response.ErrorList.Count > 0)
                    return response;

                if (profile.RoleMappings.Count == 0)
                    roleMappings = roleUtils.InsertRoleMapping(response, profile.UserId);
                else
                    roleMappings = profile.RoleMappings.ToList();                


                hppAuthInterchange.UserId = Convert.ToInt32(profile.UserId);
                hppAuthInterchange.Token = sessionToken.AccessToken;
                hppAuthInterchange.IsHPID = true;
                
               

                // Register profile & session token in database
                customerUtils.InsertOrUpdateHPPToken(response, (UserAuthentication)hppAuthInterchange, RetainOldValues);

                List<int> roleids = roleMappings.Select(r => r.RoleId).ToList();
                List<UserRoles> userRoles = new List<UserRoles>();
             
                RESTAPIGetRolesResponse rolesResponse = new RESTAPIGetRolesResponse();
   
                statusUtils.GetRoles(rolesResponse);

                if (rolesResponse.RolesList.Count != 0)
                {
                    var rolResponseList = rolesResponse.RolesList.Where(r => roleids.Contains( r.RoleId)).ToList();
                    foreach (var roles in rolResponseList)
                    {
                        userRoles.Add(new UserRoles() { Id = roles.RoleId, Name = roles.RoleName });
                    }
                }


                response.Credentials = new AccessCredentials()
                {
                    UserID = Convert.ToInt32(profile.UserId),
                    SessionToken = QueryUtils.GetMD5(sessionToken.AccessToken),
                    CallerId = hppAuthInterchange.CallerId,
                    Token = sessionToken.AccessToken,
                    Roles= userRoles
                };

                response = GetProfileDataFromDatabase(response, profile);
                response.CustomerProfileObject.IsNewCustomer = IsNewCustomer;
                response.LoginDate = hppAuthInterchange.LoginDate;
            }
            catch (Exception ex)
            {
                response.ErrorList.Add(new Fault("GetCustomerProfileFailed", ex.Message));
            }          

            return response;
        }

        private GetProfileResponse GetProfileDataFromDatabase(GetProfileResponse response, User userData)
        {
            response.CustomerProfileObject.IsNewCustomer = false;
            response.CustomerProfileObject.ActiveHealth = userData.ActiveHealth;
            response.CustomerProfileObject.EmailConsent = userData.EmailConsent==true ? EmailConsentType.Y.ToString() : EmailConsentType.N.ToString();
            response.CustomerProfileObject.PrimaryUse = userData.PrimaryUse;
            if (string.IsNullOrEmpty(response.CustomerProfileObject.CompanyName))
                response.CustomerProfileObject.CompanyName = userData.CompanyName;

            return response;
        }

        public GetProfileResponse GetCustomerProfileByLogin(UserAuthenticationInterchange UserAuthInterchange, bool RetainOldValues, APIMethods apiRetainOldValues)
        {
            GetProfileResponse response = new GetProfileResponse();

            if (string.IsNullOrEmpty(UserAuthInterchange.UserName) || string.IsNullOrEmpty(UserAuthInterchange.Password))
            {
                response.ErrorList.Add(Responses.Faults.InvalidCredentials);
                return response;
            }

            TokenDetails sessionToken = hpidUtils.GetHPIDSessionToken((int)TokenScopeType.userLogin, UserAuthInterchange.UserName, UserAuthInterchange.Password, response, null);
            if (response.ErrorList.Count > 0)
            {
                return response;
            }
            response = GetCustomerProfileforHPID(UserAuthInterchange, sessionToken, RetainOldValues, apiRetainOldValues);
            foreach (var fault in response.ErrorList)
            {
                var error = string.Format("origin={0},Return code={1},status Text={2}", fault.Origin, fault.ReturnCode, fault.DebugStatusText);
                log.Debug(string.Format($"ProfileByLogin: Caller={UserAuthInterchange.CallerId},Exception={error}"));
            }
            return response;
        }


        public GetProfileResponse GetCustomerProfileByTestLogin(UserAuthenticationInterchange UserAuthInterchange, bool RetainOldValues, APIMethods apiRetainOldValues)
        {
            GetProfileResponse response = new GetProfileResponse();
            UserAuthInterchange.UserName = "ramdeo.angh@hp.com";
            UserAuthInterchange.Password = "Ramdeo123!@#";
            //UserAuthInterchange.UserName = UserAuthInterchange.UserName;
            //UserAuthInterchange.Password = UserAuthInterchange.Password;
            //UserAuthInterchange.UserId = UserAuthInterchange.UserId;

            if (string.IsNullOrEmpty(UserAuthInterchange.UserName) || string.IsNullOrEmpty(UserAuthInterchange.Password))
            {
                response.ErrorList.Add(Responses.Faults.InvalidCredentials);
                return response;
            }

            TokenDetails sessionToken = null;
            response = GetCustomerProfileforHPIDTEST(UserAuthInterchange, sessionToken, RetainOldValues, apiRetainOldValues);
            foreach (var fault in response.ErrorList)
            {
                var error = string.Format("origin={0},Return code={1},status Text={2}", fault.Origin, fault.ReturnCode, fault.DebugStatusText);
                log.Debug(string.Format($"ProfileByLogin: Caller={UserAuthInterchange.CallerId},Exception={error}"));
            }
            return response;
        }

        public GetProfileResponse GetCustomerProfileByDefaultUserLogin(GetProfileResponse response,UserAuthenticationInterchange UserAuthInterchange, bool RetainOldValues, APIMethods apiRetainOldValues)
        {
            User user = null;
            List<RoleMapping> roles = null;

            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {
                            user = query.GetUserDetails(context, UserAuthInterchange.UserId);
                            roles = query.GetUserRoles(context, UserAuthInterchange.UserId);
                        }
                        , readOnly: true
                    );


            List<int> roleids = roles.Select(r => r.RoleId).ToList();
            List<UserRoles> userRoles = new List<UserRoles>();

            RESTAPIGetRolesResponse rolesResponse = new RESTAPIGetRolesResponse();

            statusUtils.GetRoles(rolesResponse);

            if (rolesResponse.RolesList.Count != 0)
            {
                var rolResponseList = rolesResponse.RolesList.Where(r => roleids.Contains(r.RoleId)).ToList();
                foreach (var role in rolResponseList)
                {
                    userRoles.Add(new UserRoles() { Id = role.RoleId, Name = role.RoleName });
                }
            }


            response.Credentials = new AccessCredentials()
            {
                UserID = Convert.ToInt32(user.UserId),
                SessionToken = (user.SessionToken),
                CallerId = UserAuthInterchange.CallerId,
                Token = user.SessionToken,
                Roles = userRoles
                //RoleID = roles.Select(x=> x.RoleID).ToList(),
            };

            response.CustomerProfileObject = new CustomerProfile()
            {
                Locale = user.Locale,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress
            };


            //if (string.IsNullOrEmpty(UserAuthInterchange.UserName) || string.IsNullOrEmpty(UserAuthInterchange.Password))
            //{
            //    response.ErrorList.Add(Responses.Faults.InvalidCredentials);
            //    return response;
            //}



            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Responses.Faults.ServerIsBusy);
            }

            return response;
        }


        public GetProfileByTokenResponse GetCustomerProfileByTokenforHPID(UserAuthenticationInterchange UserAuthInterchange, TokenDetails sessionTokenDetails, string languageCode, string countryCode, APIMethods apiRetainOldValues)
        {
            throw new NotImplementedException();
        }
    }
}