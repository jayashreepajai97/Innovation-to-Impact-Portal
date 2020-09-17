using NLog;
using System;
using Responses;
using System.Net;
using System.Text;
using System.Linq;
using System.Web.Mvc;
using System.Net.Http;
using SettingsRepository;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using IdeaDatabase.Enums;
using IdeaDatabase.Utils;
using System.Net.Http.Headers;
using System.Collections.Generic;
using InnovationPortalService.Responses;

namespace InnovationPortalService.HPID
{
    public class HPIDUtils : IAUTHUtils
    {
        private const int HPIDGETTOKENTIMEOUT = 14;
        private const int HPIDCREATEPROFILETIMEOUT = 14;

        private static string profileURL = SettingRepository.Get<string>("HPIDProfileUrl");
        private static string newuserURL = SettingRepository.Get<string>("HPIDNewUserUrl");
        private static string oauthUri = SettingRepository.Get<string>("HPIDOAuthUrl");
        private static string hpidredirectURI = SettingRepository.Get<string>("HPIDRedirectURI");
        private static string hpidApiKey = SettingRepository.Get<string>("HPIDApiKey");
        private static string hpidSecretKey = SettingRepository.Get<string>("HPIDApiSecret");

        private static int hpidGetTokenTimeout = HPIDGETTOKENTIMEOUT;
        private static int hpidCreateProfileTimeout = HPIDCREATEPROFILETIMEOUT;

        private static Logger logger = LogManager.GetLogger("HPIDTimeoutsLog");


        #region Private Methods
        /// <summary>
        /// Verifies language code: it is possible that language code saved in HPP profile is defined as number '12' or '13'
        /// It was caused bay mapping of chine languages zf = 13, zh = 12
        /// </summary>
        /// <param name="hpidLanguageCode">language code saved in HPID profile</param>
        /// <returns>Proper text of language code</returns>
        private string GetProperLanguageCode(string hpidLanguageCode)
        {
            if (string.IsNullOrEmpty(hpidLanguageCode))
                return null;

            return hpidLanguageCode.MapFromHPPLangCode();
        }

        private TokenDetails GetToken(List<KeyValuePair<string, string>> keyValues, ResponseBase response, int tokenScope, int refreshTokenType, string clientId)
        {
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, hpidGetTokenTimeout);
            client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/x-www-form-urlencoded");
            TokenDetails _tokenDetails = new TokenDetails();

            _tokenDetails.tokenScopeType = (TokenScopeType)tokenScope;

            string hpidApiKeyAuth = null;
            string hpidSecretKeyAuth = null;

            if (! String.IsNullOrEmpty(clientId))
            {
                hpidApiKeyAuth = SettingRepository.Get<string>("HPIDApiKeyAUTH_" + clientId);
                hpidSecretKeyAuth = SettingRepository.Get<string>("HPIDApiSecretAUTH_" + clientId);
            }
            else
            {
                hpidApiKeyAuth = SettingRepository.Get<string>("HPIDApiKeyAUTH");
                hpidSecretKeyAuth = SettingRepository.Get<string>("HPIDApiSecretAUTH");
            }


            if (tokenScope == (int)TokenScopeType.userAuthenticate ||
                refreshTokenType == (int)RefreshTokenLoginType.Authentication)
            {
                _tokenDetails.RefreshTokenType = (int)RefreshTokenLoginType.Authentication;
                client.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue("Basic",
                           Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:", hpidApiKeyAuth) + string.Format("{0}", hpidSecretKeyAuth))));

                log.Debug($"getToken: AUTH: tokenScope='{tokenScope}' reftokentype='{refreshTokenType}' api='{hpidApiKeyAuth}' secret='{hpidSecretKeyAuth}' uri='{oauthUri}' ");
            }
            else
            {
                _tokenDetails.RefreshTokenType = (int)RefreshTokenLoginType.Credentials;
                client.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue("Basic",
                           Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:", hpidApiKey) + string.Format("{0}", hpidSecretKey))));

                log.Debug($"getToken: CRED: tokenScope='{tokenScope}' reftokentype='{refreshTokenType}'  api='{hpidApiKey}' secret='{hpidSecretKey}' uri='{oauthUri}' ");
            }

            try
            {
                var postData = new FormUrlEncodedContent(keyValues);
                var resp = client.PostAsync(oauthUri, postData);
                var jResponse = resp.Result.Content.ReadAsStringAsync().Result;
                JObject respObj = JObject.Parse(jResponse);

                try
                {
                    string error_cause = (string)respObj["error_cause"];
                    if (error_cause.Equals("accountLocked"))
                    {
                        log.Debug($"getToken: accountLocked");
                        response.ErrorList.Add(Faults.HPIDAccountLocked);
                        return null;
                    }
                    log.Debug($"getToken: error_cause={error_cause}");
                }
                catch (Exception) { };

                _tokenDetails.AccessToken = (string)respObj["access_token"];
                _tokenDetails.RefreshToken = (string)respObj["refresh_token"];

                log.Debug($"getToken response: RefreshToken={_tokenDetails.RefreshToken} AccessToken={_tokenDetails.AccessToken}");

                if (string.IsNullOrEmpty(_tokenDetails.RefreshToken))
                {
                    log.Debug($"getToken response: ALL={respObj}");
                }


                return _tokenDetails;
            }
            catch (TimeoutException ex)
            {
                log.Debug($"getToken: Errortype=TimeoutException,Exception={ex.Message}");
                response.ErrorList.Add(new Fault(Faults.HPIDTimeoutError, ex));
                logger.Error($"GetToken: timeout exception ({ex.Message})");
            }
            catch (Exception ex)
            {
                log.Debug($"getToken: Errortype=Exception,Exception={ex.Message}");
                response.ErrorList.Add(new Fault(Faults.HPIDInternalError, ex));
                logger.Error($"GetToken: other exception ({ex.InnerException?.Message})");
            }

            return null;
        }

        private bool CheckServiceResponse(JObject respObj, ResponseBase response)
        {
            // try to get more info from response 
            string scimType = (string)respObj["scimType"];
            string detail = (string)respObj["detail"];
            string[] schemas = null;
            try { var schema = respObj["schemas"]; schemas = new string[] { schema.ToString() }; } catch (Exception) { };


            // PROPER RESPONSE
            if (schemas == null || schemas.Length == 0)
                return true;


            var textSccess = "urn:hp:hpid:scim:schemas:1.0:User";
            if (schemas[0].Contains(textSccess))
                return true;


            var textError = "urn:ietf:params:scim:api:messages:2.0:Error";
            if (!schemas[0].Contains(textError))
                return true;



            // FAILURES
            if (!string.IsNullOrEmpty(scimType))
            {
                // profile already exists
                if (scimType.Equals("uniqueness"))
                {
                    response.ErrorList.Add(Faults.HPIDUserExists);
                    log.Debug($"checkresponse: Errortype=user already exists, exception=uniqueness");
                    return false;
                }

                // invalid or expired token - in create new profile, in get existing profile
                if (scimType.Equals("invalid_token"))
                {
                    response.ErrorList.Add(Faults.HPIDInvalidToken);
                    log.Debug($"checkresponse: Errortype=invalid token");
                    return false;
                }

                // invalid request content
                if (scimType.Equals("invalidSyntax") ||
                    scimType.Equals("invalidPath"))
                {
                    if (!string.IsNullOrEmpty(detail))
                        response.ErrorList.Add(new Fault("HPID", "InvalidSyntax", $"Request has invalid syntax \n{detail}"));
                    else
                        response.ErrorList.Add(Faults.HPIDInvalidSyntax);

                    log.Debug($"checkresponse: Errortype=invalidSyntax or invalidPath");
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(detail))
            {
                // invalid or expired token - in create new profile, in get existing profile
                if (detail.Equals("Access Denied"))
                {
                    response.ErrorList.Add(Faults.HPIDInvalidToken);
                    log.Debug($"checkresponse: Errortype=Access Denied - invalid token");
                    return false;
                }
            }

            response.ErrorList.Add(Faults.HPIDInternalError);
            log.Debug($"checkresponse: Errortype=HPID internal error");
            return false;
        }
        #endregion Private Methods

        public static ILogger log = LogManager.GetLogger($"HPIDAutheticationLog");

        public class CustomerIds
        {
            public string HPPid { get; set; }
            public string HPIDid { get; set; }
        }

        static HPIDUtils()
        {
            int w;
            if (SettingRepository.TryGet<int>("HPIDGetTokenTimeout", out w))
            {
                hpidGetTokenTimeout = w;
            }

            if (SettingRepository.TryGet<int>("HPIDCreateProfileTimeout", out w))
            {
                hpidCreateProfileTimeout = w;
            }
        }

        public HPIDUtils()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                     | SecurityProtocolType.Tls11
                                     | SecurityProtocolType.Tls12;
        }

        public TokenDetails GetHPIDSessionToken(int tokenScope, string KeyValue1, string KeyValue2, ResponseBase response, string clientId, int refreshTokenType = 0)
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            switch (tokenScope)
            {
                case (int)TokenScopeType.userAuthenticate:

                    if (!string.IsNullOrEmpty(KeyValue1) && !string.IsNullOrEmpty(KeyValue2))
                    {
                        keyValues.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
                        keyValues.Add(new KeyValuePair<string, string>("redirect_uri", KeyValue2));
                        keyValues.Add(new KeyValuePair<string, string>("code", KeyValue1));
                    }
                    else
                    {
                        response.ErrorList.Add(Faults.InvalidCredentials);
                        return null;
                    }
                    break;


                case (int)TokenScopeType.userLogin:

                    if (!string.IsNullOrEmpty(KeyValue1) && !string.IsNullOrEmpty(KeyValue2))
                    {
                        keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
                        keyValues.Add(new KeyValuePair<string, string>("username", KeyValue1 + "@hpid"));
                        keyValues.Add(new KeyValuePair<string, string>("password", KeyValue2));
                    }
                    else
                    {
                        response.ErrorList.Add(Faults.InvalidCredentials);
                        return null;
                    }

                    break;
                case (int)TokenScopeType.userCreate:

                    keyValues.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                    keyValues.Add(new KeyValuePair<string, string>("redirect_uri", hpidredirectURI));
                    break;
                case (int)TokenScopeType.userRefreshToken:

                    keyValues.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
                    keyValues.Add(new KeyValuePair<string, string>("scope", "user.profile.read"));
                    keyValues.Add(new KeyValuePair<string, string>("refresh_token", KeyValue2));
                    break;
            }

            return GetToken(keyValues, response, tokenScope, refreshTokenType,clientId);
        }

        public bool GetIdsAndProfile(CustomerIds ids, string token, GetProfileResponse response)
        {
            JObject respObj = null;
            log.Debug($"GetIdsAndProfile: START token={token}");
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Stopwatch timer = new Stopwatch();
            string respResult = null;
            try
            {

                timer.Start();
                var resp = client.GetAsync(profileURL).Result;
                timer.Stop();

                respResult = resp.Content.ReadAsStringAsync().Result;
                respObj = JObject.Parse(respResult);

                if (!CheckServiceResponse(respObj, response))
                {
                    log.Debug($"GetIdsAndProfile: get profile failed");
                    return false;
                }
            }
            catch (TimeoutException ex)
            {
                log.Debug($"GetIdsAndProfile: Errortype=TimeoutException,Exception={ex.Message}");
                response.ErrorList.Add(new Fault(Faults.HPIDTimeoutError, ex));
                return false;
            }
            catch (Exception ex)
            {
                log.Debug($"GetIdsAndProfile: Errortype=Exception,Exception={ex.Message}");
                response.ErrorList.Add(new Fault(Faults.HPIDInternalError, ex));
                return false;
            }
            finally
            {
                timer.Stop();
                // Log the outbound service call details in ExtServiceLogs table
                ExtServiceLogs.LogExtServiceCallDetails(client?.DefaultRequestHeaders?.Authorization?.ToString(),
                    HttpVerbs.Get, "HPID", profileURL?.ToString(), null, respObj?.ToString(), (int)timer.ElapsedMilliseconds, false);
            }

            // get HPIDid profile ID if missing, it means that profile is broken
            ids.HPIDid = (string)respObj["id"];
            if (string.IsNullOrEmpty(ids.HPIDid))
            {
                response.ErrorList.Add(Faults.HPIDInvalidResponse);
                log.Debug($"GetIdsAndProfile: Errortype=HPIDid profile ID if missing");
                return false;
            };


            // get HPPid profile ID - if missing, it means that user is just logged
            ids.HPPid = (string)respObj["hpp_id"];


            // get profile data
            // ===========================================================================
            CustomerProfile profile = new CustomerProfile();
            profile.EmailConsent = EmailConsentType.N.ToString();
            profile.PrimaryUse = PrimaryUseType.Item002.ToString();


            try { profile.FirstName = (string)respObj["name"]["givenName"]; } catch (Exception ex) { profile.FirstName = null; log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-FirstName, exception={ex.Message}"); }
            try { profile.LastName = (string)respObj["name"]["familyName"]; } catch (Exception ex) { profile.LastName = null; log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-LastName, exception={ex.Message}"); }
            profile.UserName = (string)respObj["userName"];
            profile.Country = (string)respObj["countryResidence"];
            profile.CompanyName = (string)respObj["hpp_organizationName"];
            try { profile.Language = GetProperLanguageCode(((string)respObj["locale"]).Substring(0, 2)); } catch (Exception ex) { profile.Language = null; log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-Language, exception={ex.Message}"); }
            try { profile.Locale  = (string)respObj["locale"]; } catch (Exception ex) { profile.Locale = null; log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-Locale, exception={ex.Message}"); }
            try { profile.DisplayName = (string)respObj["displayName"]; } catch (Exception ex) { profile.DisplayName = null; log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-DisplayName, exception={ex.Message}"); }
            try { profile.Gender = (string)respObj["gender"]; } catch (Exception ex) { profile.Gender = null; log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-Gender, exception={ex.Message}"); }
            profile.Id = (string)respObj["id"];
            //Get Address details
            try
            {
                JToken jStatus = respObj.SelectToken("$.addresses");
                if (jStatus != null && jStatus.Count() > 0)
                {
                    profile.Addresses = new List<Address>();
                    foreach (var i in jStatus)
                    {
                        Address add = i.ToObject<Address>();
                        profile.Addresses.Add(add);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-addresses, exception={ex.Message}");
            }

            //Get phone numbers
            try
            {
                JToken jStatus = respObj.SelectToken("$.phoneNumbers");
                if (jStatus != null && jStatus.Count() > 0)
                {
                    profile.PhoneNumbers = new List<PhoneNumber>();
                    foreach (var i in jStatus)
                    {
                        PhoneNumber ph = i.ToObject<PhoneNumber>();
                        profile.PhoneNumbers.Add(ph);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-phoneNumbers, exception={ex.Message}");
            }

            // use primary email
            try
            {
                JToken jStatus = respObj.SelectToken("$.emails");
                if (jStatus != null && jStatus.Count() > 0)
                {
                    foreach (var i in jStatus)
                    {
                        Email eml = i.ToObject<Email>();
                        if (eml.primary)
                        {
                            profile.EmailAddress = eml.value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-email, exception={ex.Message}");
            };

            //use primary address   
            try
            {
                JToken jStatus = respObj.SelectToken("$.addresses");
                if (jStatus != null && jStatus.Count() > 0)
                {
                    foreach (var i in jStatus)
                    {
                        Address adr = i.ToObject<Address>();
                        if (Convert.ToBoolean(adr.Primary))
                        {
                            profile.City = adr.Locality;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-address, exception={ex.Message}");
            };


            response.CustomerProfileObject = profile;

            return true;
        }

        public string CreateHPIDNewCustomerProfile(string sessionToken, string requestJson, ResponseBase response)
        {
            string newuserID = null;
            JObject respObj = null;

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, hpidCreateProfileTimeout);
            client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/scim+json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionToken);

            Stopwatch timer = new Stopwatch();
            string postResponse = null;
            try
            {
                HttpContent cont = new StringContent(requestJson, Encoding.UTF8, "application/json");

                timer.Start();
                var resp = client.PostAsync(newuserURL, cont).Result;
                timer.Stop();

                postResponse = resp.Content.ReadAsStringAsync().Result;
                respObj = JObject.Parse(postResponse);

                newuserID = (string)respObj["id"];

                if (!CheckServiceResponse(respObj, response))
                    return null;
            }
            catch (TimeoutException ex)
            {
                response.ErrorList.Add(new Fault(Faults.HPIDTimeoutError, ex));
                logger.Error($"CreateProfile: timeout exception ({ex.Message})");
            }
            catch (Exception ex)
            {
                response.ErrorList.Add(new Fault(Faults.HPIDInternalError, ex));
                logger.Error($"CreateProfile: other exception ({ex.InnerException?.Message})");
            }
            finally
            {
                timer.Stop();
                // Log the outbound service call details in ExtServiceLogs table
                ExtServiceLogs.LogExtServiceCallDetails(client?.DefaultRequestHeaders?.Authorization?.ToString(),
                    HttpVerbs.Post, "HPID", profileURL?.ToString(), requestJson, respObj?.ToString(), (int)timer.ElapsedMilliseconds, true);
            }

            return newuserID;
        }

        public void UpdateHPIDCustomerProfile(string sessionToken, string requestJson, ResponseBase response)
        {
            JObject respObj = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionToken);

            Stopwatch timer = new Stopwatch();
            string respResponse = null;
            try
            {
                StringContent cont = new StringContent(requestJson, Encoding.UTF8, "application/json");

                timer.Start();
                var resp = HttpClientExtensions.PatchAsync(client, profileURL, cont).Result;
                timer.Stop();

                respResponse = resp.Content.ReadAsStringAsync().Result;
                respObj = JObject.Parse(respResponse);

                CheckServiceResponse(respObj, response);
            }
            catch (TimeoutException ex)
            {
                response.ErrorList.Add(new Fault(Faults.HPIDTimeoutError, ex));
                return;
            }
            catch (Exception ex)
            {
                response.ErrorList.Add(new Fault(Faults.HPIDInternalError, ex));
                return;
            }
            finally
            {
                timer.Stop();
                // Log the outbound service call details in ExtServiceLogs table
                ExtServiceLogs.LogExtServiceCallDetails(client?.DefaultRequestHeaders?.Authorization?.ToString(),
                    HttpVerbs.Patch, "HPID", profileURL?.ToString(), requestJson, respObj?.ToString(), (int)timer.ElapsedMilliseconds, false);
            }
        }

        public HPIDCustomerProfile GetProfile(string token)
        {
            JObject respObj = null;
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Stopwatch timer = new Stopwatch();
            string respResult = null;
            ResponseBase response = new ResponseBase();
            try
            {
                timer.Start();
                var resp = client.GetAsync(profileURL).Result;
                timer.Stop();

                respResult = resp.Content.ReadAsStringAsync().Result;
                respObj = JObject.Parse(respResult);

                if (!CheckServiceResponse(respObj, response))
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                timer.Stop();
                // Log the outbound service call details in ExtServiceLogs table
                ExtServiceLogs.LogExtServiceCallDetails(client?.DefaultRequestHeaders?.Authorization?.ToString(),
                    HttpVerbs.Get, "HPID", profileURL?.ToString(), null, respObj?.ToString(), (int)timer.ElapsedMilliseconds, false);

            }

            if (string.IsNullOrEmpty((string)respObj["id"]))
            {
                return null;
            }

            HPIDCustomerProfile profile = new HPIDCustomerProfile();
            try
            {
                var fName = (string)respObj["name"]["familyName"];
                var gName = (string)respObj["name"]["givenName"];

                profile.name = new Name() { familyName = fName, givenName = gName };
            }
            catch (Exception) { };

            profile.countryResidence = (string)respObj["countryResidence"];
            profile.locale = (string)respObj["locale"];
            profile.hpp_organizationName = (string)respObj["hpp_organizationName"];

            //Add DisplayName
            try
            {
                profile.displayName = (string)respObj["displayName"];
            }
            catch (Exception ex)
            {
                profile.displayName = null;
                log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-DisplayName, exception={ex.Message}");
            }

            //Add gender
            try
            {
                profile.gender = (string)respObj["gender"];
            }
            catch (Exception ex)
            {
                profile.gender = null;
                log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-Gender, exception={ex.Message}");
            }

            try
            {
                JToken jStatus = respObj.SelectToken("$.emails");
                if (jStatus != null && jStatus.Count() > 0)
                {
                    if (profile.emails == null)
                        profile.emails = new List<Email>();

                    foreach (var i in jStatus)
                    {
                        Email eml = i.ToObject<Email>();

                        if (eml != null)
                            profile.emails.Add(eml);
                    }
                }
            }
            catch (Exception) { };

            //Get all addresses
            try
            {
                JToken jStatus = respObj.SelectToken("$.addresses");
                if (jStatus != null && jStatus.Count() > 0)
                {
                    if (profile.addresses == null)
                        profile.addresses = new List<Address>();

                    foreach (var i in jStatus)
                    {
                        Address adr = i.ToObject<Address>();
                        if (adr != null)
                            profile.addresses.Add(adr);
                    }
                }
            }
            catch (Exception) { };

            //Get all phonenumbers
            try
            {
                JToken jStatus = respObj.SelectToken("$.phoneNumbers");
                if (jStatus != null && jStatus.Count() > 0)
                {
                    profile.phoneNumbers = new List<PhoneNumber>();
                    foreach (var i in jStatus)
                    {
                        PhoneNumber ph = i.ToObject<PhoneNumber>();
                        profile.phoneNumbers.Add(ph);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug($"GetIdsAndProfile: Errortype=HPID response parse-phoneNumbers, exception={ex.Message}");
            }

            return profile;
        }
    }
}