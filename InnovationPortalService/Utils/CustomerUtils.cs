using Credentials;
using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Utils;
using IdeaDatabase.Validation;
//using InnovationPortalService.HPPService;
using InnovationPortalService.Responses;
using Newtonsoft.Json;
using NLog;
using Responses;
using SettingsRepository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace InnovationPortalService.Utils
{
    [JsonObject]
    public class CustomerUtils : ICustomerUtils
    {
        //private ICustProdRegistrationsserviceagent svc;
        private ICommonHelper commonHelper;
        
   
        private IAuthenticationUtils authUtils;
        private IUserUtils isacUserUtils;
        
    
        private static int supportLocationsCallRefreshCount;
       
        //private static bool SynchronizeIsaacOnGetProfile = false;
        private static ConcurrentQueue<string> SupportLocationsCacheData;
        

        private static Logger logger = LogManager.GetLogger("DatabaseSubmitChanges");
        private static Logger ilogger = LogManager.GetLogger("ISAACSynchronization");

        public CustomerUtils()
        {
           // svc = DependencyInjector.Get<ICustProdRegistrationsserviceagent, CustProdRegistrationsserviceagent>();
            commonHelper = DependencyInjector.Get<ICommonHelper, CommonHelper>();
           
           
            authUtils = DependencyInjector.Get<IAuthenticationUtils, AuthenticationUtils>();
            isacUserUtils = DependencyInjector.Get<IUserUtils, UserUtils>();
            
            
          
           // svc.Setup();
        }

        static CustomerUtils()
        {
            // read once from web.config
            SettingRepository.TryGet<int>("supportLocationsCallRefreshCount", out supportLocationsCallRefreshCount);
            SupportLocationsCacheData = new ConcurrentQueue<string>();
           // SettingRepository.TryGet<bool>("SynchronizeIsaacOnGetProfile", out SynchronizeIsaacOnGetProfile);
        }

        public void InsertOrUpdateHPPToken(GetProfileResponse response, UserAuthentication hppAuthentication, bool RetainOldValues)
        {
            DatabaseWrapper.databaseOperation(response, (ctx, query) =>
            {
                UserAuthentication hppAuth = query.GetHPPToken(ctx, hppAuthentication.UserId, hppAuthentication.CallerId);

                if (hppAuth != null)
                {
                    this.RetainOldValues(RetainOldValues, hppAuth, hppAuthentication);
                    ctx.UserAuthentications.Remove(hppAuth);
                }

                query.SetHPPToken(ctx, hppAuthentication);
            },
            readOnly: false);
        }

        //public GetProfileResponse GetCustomerProfile(string UserName, string Password, UserAuthenticationInterchange authInterchange)
        //string CallerId, string Token, string LangaugeCode = null, string CountryCode = null, string UseCaseGroup = null, string Platform = null
        //public GetProfileResponse GetCustomerProfile(UserAuthenticationInterchange authInterchange, bool RetainOldValues)
        //{
        //    GetProfileResponse response = new GetProfileResponse();
        //    string sessionToken = "";
        //    getUserResponse userHppData = null;
        //    int retryCount = 0;

        //    while (retryCount < RetryCounter.HPPRetryCounter)
        //    {
        //        try
        //        {
        //            if (String.IsNullOrEmpty(authInterchange.Token))
        //            {
        //                sessionToken = GetTokenFromHPP(authInterchange.UserName, authInterchange.Password, authInterchange.CallerId, response);
        //                if (response.ErrorList.Count > 0)
        //                    return response;
        //            }
        //            else
        //            {
        //                sessionToken = authInterchange.Token;
        //            }
        //            DocLiteral client = DependencyInjector.Get<DocLiteral, HPPClient>();
        //            userHppData = commonHelper.GetUserProfile(client, sessionToken, response);

        //            break;
        //        }
        //        catch (FaultException<genericFault> ex)
        //        {
        //            IExceptionMapping em = DependencyInjector.Get<IExceptionMapping, ExceptionMapping>();
        //            em.MapHPPGenericFaults(response, ex.Detail.fault);
        //            return response;
        //        }
        //        catch (Exception ex)
        //        {
        //            retryCount++;
        //            if (commonHelper.HandleHPPException(retryCount, ex, new Fault("GetCustomerProfileFailed", ex.Message), response))
        //            {
        //                return response;
        //            }
        //        }
        //    }

        //    try
        //    {
        //        // check is done based on profile ... customerId is also generated for a new profile
        //        User isacProfile = isacUserUtils.FindOrInsertProfile(response, userHppData.getUserResponseElement.profileIdentity.profileId);
        //        if (response.ErrorList.Count > 0)
        //            return response;

        //        //Update the datbase value based on service  
        //       // UpdateIsacFromOnlineCustomerProfile(response, userHppData.getUserResponseElement, isacProfile);

        //        authInterchange.UserId = Convert.ToInt32(isacProfile.UserId);
        //        authInterchange.Token = sessionToken;
        //        authInterchange.IsHPID = false;


        //        /* Register profile & token in database */
        //        InsertOrUpdateHPPToken(response, (UserAuthentication)authInterchange, RetainOldValues);

        //        AccessCredentials creds = new AccessCredentials()
        //        {
        //            UserID = Convert.ToInt32(isacProfile.UserId),
        //            SessionToken = sessionToken,
        //            CallerId = authInterchange.CallerId
        //        };
        //        response.Credentials = creds;
        //        response.CustomerProfileObject = userHppData.getUserResponseElement;
        //        response.CustomerProfileObject.ActiveHealth = isacProfile.ActiveHealth;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.ErrorList.Add(new Fault("GetCustomerProfileFailed", ex.Message));
        //    }
     
        //    return response;
        //}
 
         

        //public GetProfileResponse RefreshToken(string UserName, string Password, string CallerId, int UserID, bool RetainOldValues)
        //{
        //    GetProfileResponse response = new GetProfileResponse();

        //    String sessionToken = GetTokenFromHPP(UserName, Password, CallerId, response);
        //    if (response.ErrorList.Count > 0)
        //    {
        //        return response;
        //    }

        //    /* Register profile & token in database */
        //    AccessCredentials creds = new AccessCredentials();
        //    creds.UserID = UserID;
        //    creds.SessionToken = sessionToken;
        //    creds.CallerId = CallerId;

        //    response.Credentials = creds;

        //    UserAuthenticationInterchange hppAuth = new UserAuthenticationInterchange()
        //    {
        //        UserId = Convert.ToInt32(creds.UserID),                
        //        Token = creds.SessionToken,
        //        CallerId = CallerId,
        //    };

        //    InsertOrUpdateHPPToken(response, (UserAuthentication)hppAuth, RetainOldValues);

        //    return response;
        //}

        //private String GetTokenFromHPP(string UserName, string Password, string CallerId, GetProfileResponse response)
        //{
        //    /* HP Passport Service */
        //    String sessionToken;
        //    int retryCount = 0;

        //    while (retryCount < RetryCounter.HPPRetryCounter)
        //    {
        //        try
        //        {
        //            DocLiteral client = DependencyInjector.Get<DocLiteral, HPPClient>();

        //            loginRequest loginReq = DependencyInjector.Get<ICommonHelper, CommonHelper>().GetLoginRequest(UserName, Password);

        //            loginResponse r = client.login(loginReq);

        //            sessionToken = r.loginResponseElement.sessionToken;

        //            return sessionToken;
        //        }
        //        catch (FaultException<genericFault> ex)
        //        {
        //            IExceptionMapping em = DependencyInjector.Get<IExceptionMapping, ExceptionMapping>();
        //            em.MapHPPGenericFaults(response, ex.Detail.fault);
        //            return null;
        //        }
        //        catch (Exception ex)
        //        {
        //            retryCount++;
        //            if (commonHelper.HandleHPPException(retryCount, ex, Faults.HPPInternalError, response))
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    return null;
        //}

        private void RetainOldValues(bool RetainOldValues, UserAuthentication oldHPPAuth, UserAuthentication newHPPAuth)
        {
            if (RetainOldValues)
            {
                

                if (oldHPPAuth.ClientApplication != null)
                    newHPPAuth.ClientApplication = oldHPPAuth.ClientApplication;

                if (oldHPPAuth.ClientVersion != null)
                    newHPPAuth.ClientVersion = oldHPPAuth.ClientVersion;

                if (oldHPPAuth.ClientPlatform != null)
                    newHPPAuth.ClientPlatform = oldHPPAuth.ClientPlatform;

                newHPPAuth.LanguageCode = oldHPPAuth.LanguageCode;
                newHPPAuth.CountryCode = oldHPPAuth.CountryCode;
                newHPPAuth.CreatedDate = oldHPPAuth.CreatedDate;
                newHPPAuth.ClientId = oldHPPAuth.ClientId;
            }
        }

        public void UpdateLogoutDate(ResponseBase response, int UserId, string callerId, string tokenMd5)
        {
            DatabaseWrapper.databaseOperation(response,
                (ctx, query) =>
                {
                    UserAuthentication hppAuth = query.GetHPPToken(ctx, UserId, tokenMd5, callerId);
                    if (hppAuth != null)
                    {
                        hppAuth.LogoutDate = DateTime.UtcNow;
                        ctx.SubmitChanges();
                    }
                },
                readOnly: false
            );
        }

    }
}
