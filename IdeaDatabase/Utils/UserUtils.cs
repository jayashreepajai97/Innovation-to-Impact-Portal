using System;
using Responses;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using IdeaDatabase.DataContext;
using IdeaDatabase.Utils.Interface;
using IdeaDatabase.Utils.IImplementation;
using Hpcs.DependencyInjector;

namespace IdeaDatabase.Utils
{
    public class UserUtils : IUserUtils
    {
      
        public User FindOrInsertHPIDProfile(ResponseBase response, string HPIDprofileId, string HPPprofileId)
        {
            User profile = null;

            DatabaseWrapper.databaseOperation(response,
                            (context, query) =>
                            {
                                User HPIDprofile = null;
                                User HPPprofile = null;

                                if (!string.IsNullOrEmpty(HPIDprofileId))
                                    HPIDprofile = query.GetHPIDProfile(context, HPIDprofileId);

                                if (!string.IsNullOrEmpty(HPPprofileId))
                                    HPPprofile = query.GetProfile(context, HPPprofileId);

                                // no data in db - new HPID user
                                if (HPIDprofile == null && HPPprofile == null)
                                {
                                    profile = new User
                                    {
                                        IsacId = 0,
                                        HPIDprofileId = HPIDprofileId,
                                        ProfileId = "",
                                        IsSynchronized = false,
                                        EmailConsent = false,
                                        PrimaryUse = PrimaryUseType.Item002.ToString(),
                                    };

                                    query.AddUser(context, profile);

                                    // in case if new profile was not saved in Database
                                    if (profile.UserId == 0)
                                        profile = null;
                                }

                                // existing HPP user migrated to HPID - first login to our service (HPID  ID not saved yet)
                                if (HPIDprofile == null && HPPprofile != null)
                                {
                                    HPPprofile.HPIDprofileId = HPIDprofileId;

                                    if (string.IsNullOrEmpty(HPPprofile.PrimaryUse))
                                        HPPprofile.PrimaryUse = PrimaryUseType.Item002.ToString();

                                    context.SubmitChanges();
                                    profile = HPPprofile;
                                }

                                // HPID user having already HPID ID in our database - no need to check if HPP profile is available
                                if (HPIDprofile != null)
                                {
                                    if (string.IsNullOrEmpty(HPIDprofile.PrimaryUse))
                                    {
                                        HPIDprofile.PrimaryUse = PrimaryUseType.Item002.ToString();
                                        context.SubmitChanges();
                                    }

                                    profile = HPIDprofile;
                                }
                            }
                            , readOnly: false
                        );


            if (profile == null && response.ErrorList.Count == 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }

            return profile;
        }
        public User FindOrInsertHPIDProfile(ResponseBase response, RequestFindOrInsertHPIDProfile requestFindOrInsertHPIDProfile, out bool IsNewCustomer)
        {
            // settings required for refresh token
            bool storeRefreshToken = (requestFindOrInsertHPIDProfile.tokenDetails.tokenScopeType == TokenScopeType.userAuthenticate ||
                                      requestFindOrInsertHPIDProfile.tokenDetails.tokenScopeType == TokenScopeType.userLogin ||
                                      requestFindOrInsertHPIDProfile.tokenDetails.tokenScopeType == TokenScopeType.apiProfileGetByTokenCall
                                      ) ? true : false;

            DateTime expireIn = DateTime.Now.AddMinutes(15);

            //default is false
            bool IsNew = false;

            User profile = null;
            DatabaseWrapper.databaseOperation(response,
                            (context, query) =>
                            {
                                User HPIDprofile = query.GetHPIDProfile(context, requestFindOrInsertHPIDProfile.HPIDprofileId);
                                User HPPprofile = query.GetProfile(context, requestFindOrInsertHPIDProfile.HPPprofileId);

                                // no data in db - new HPID user
                                if (HPIDprofile == null && HPPprofile == null)
                                {
                                    profile = new User
                                    {
                                        EmailAddress = requestFindOrInsertHPIDProfile.EmailAddrees,
                                        IsacId = 0,
                                        HPIDprofileId = requestFindOrInsertHPIDProfile.HPIDprofileId,
                                        ProfileId = "",
                                        PrimaryUse = PrimaryUseType.Item002.ToString(),
                                        RefreshToken = requestFindOrInsertHPIDProfile.tokenDetails.RefreshToken,
                                        RefreshTokenExpireIn = expireIn,
                                        RefreshTokenType = requestFindOrInsertHPIDProfile.tokenDetails.RefreshTokenType,
                                        OrigClientId = requestFindOrInsertHPIDProfile.clientId,
                                        CreatedDate = DateTime.UtcNow,
                                        ModifiedDate = DateTime.UtcNow,
                                        IsSynchronized = false,
                                        EmailConsent = false,
                                        IsActive = true,
                                        Locale = requestFindOrInsertHPIDProfile.Locale,
                                        FirstName = requestFindOrInsertHPIDProfile.FirstName,
                                        LastName=requestFindOrInsertHPIDProfile.LastName
                                    };

                                    query.AddUser(context, profile);
                                    profile = query.GetHPIDProfile(context, requestFindOrInsertHPIDProfile.HPIDprofileId);
                                    IsNew = true;
                                }
                                
                                // existing HPP user migrated to HPID - first login to our service (HPID  ID not saved yet)
                                if (HPIDprofile == null && HPPprofile != null)
                                {
                                    if (storeRefreshToken)
                                    {
                                        HPPprofile.RefreshToken = requestFindOrInsertHPIDProfile.tokenDetails.RefreshToken;
                                        HPPprofile.RefreshTokenExpireIn = expireIn;
                                        HPPprofile.RefreshTokenType = requestFindOrInsertHPIDProfile.tokenDetails.RefreshTokenType;
                                    }

                                    HPPprofile.HPIDprofileId = requestFindOrInsertHPIDProfile.HPIDprofileId;
                                    profile = HPPprofile;
                                }
                                
                                // HPID user having already HPID ID in our database - no need to check if HPP profile is available
                                if (HPIDprofile != null)
                                {
                                    if (storeRefreshToken)
                                    {
                                        HPIDprofile.RefreshToken = requestFindOrInsertHPIDProfile.tokenDetails.RefreshToken;
                                        HPIDprofile.RefreshTokenExpireIn = expireIn;
                                        HPIDprofile.RefreshTokenType = requestFindOrInsertHPIDProfile.tokenDetails.RefreshTokenType;
                                    }
                                    profile = HPIDprofile;
                                }

                                if (profile != null && profile.Locale != requestFindOrInsertHPIDProfile.Locale && APIMethodsUtils.APIMethodsAllowedForLocale.HasFlag(requestFindOrInsertHPIDProfile.apiRetainOldValues))
                                {
                                    profile.Locale = requestFindOrInsertHPIDProfile.Locale;
                                }

                                context.SubmitChanges();
                            }
                            , readOnly: false
                        );

            IsNewCustomer = IsNew;

            if (profile == null && response.ErrorList.Count == 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }

            return profile;
        }
        public User FindOrInsertProfile(ResponseBase response, string ProfileId)
        {
            User profile = null;

            DatabaseWrapper.databaseOperation(response,
                (context, query) =>
                {
                    profile = query.GetProfile(context, ProfileId);

                    if (profile == null)
                    {
                        profile = new User();
                        profile.IsacId = 0;
                        profile.ProfileId = ProfileId;
                        profile.IsSynchronized = false;
                        profile.EmailConsent = false;
                        profile.PrimaryUse = PrimaryUseType.Item002.ToString();

                        context.Users.Add(profile);

                        context.SubmitChanges();

                        profile = query.GetProfile(context, ProfileId);
                    }
                }
                , readOnly: false
            );

            if (profile == null && response.ErrorList.Count == 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }

            return profile;
        }
        public void SaveProfile(ResponseBase response, User profile)
        {
            if (profile == null)
                return;

            DatabaseWrapper.databaseOperation(response,
                (context, query) =>
                {
                    User dbProfile = query.GetProfile(context, profile.ProfileId);

                    if (dbProfile == null)
                        return;

                    dbProfile.IsSynchronized = profile.IsSynchronized;
                    dbProfile.IsacId = profile.IsacId;
                    dbProfile.EmailConsent = profile.EmailConsent;
                    dbProfile.CompanyName = profile.CompanyName;

                    dbProfile.PrimaryUse = profile.PrimaryUse;
                    if (string.IsNullOrEmpty(dbProfile.PrimaryUse))
                        dbProfile.PrimaryUse = PrimaryUseType.Item002.ToString();

                    context.SubmitChanges();
                }
                , readOnly: false
            );
        }
        public void SaveHPIDProfile(ResponseBase response, User profile)
        {
            if (profile == null)
                return;

            DatabaseWrapper.databaseOperation(response,
                (context, query) =>
                {
                    User dbProfile = query.GetHPIDProfile(context, profile.HPIDprofileId);

                    if (dbProfile == null)
                        return;

                    dbProfile.IsSynchronized = profile.IsSynchronized;
                    dbProfile.IsacId = profile.IsacId;
                    dbProfile.EmailConsent = profile.EmailConsent;
                    dbProfile.CompanyName = profile.CompanyName;

                    dbProfile.PrimaryUse = profile.PrimaryUse;
                    if (string.IsNullOrEmpty(dbProfile.PrimaryUse))
                        dbProfile.PrimaryUse = PrimaryUseType.Item002.ToString();


                    context.SubmitChanges();
                }
                , readOnly: false
            );
        }
        public User GetRefreshToken(string callerId, TokenDetails accessToken)
        {
            User profile = null;

            ResponseBase response = new ResponseBase();
            DatabaseWrapper.databaseOperation(response,
                (context, query) =>
                {
                    // try to get customer ID
                    UserAuthentication appAuth = query.GetHPPToken(context, accessToken.AccessToken, callerId);
                    if (appAuth == null)
                    {
                        return;
                    }

                    profile = query.GetUser(context, appAuth.UserId);
                }
                , readOnly: true
            );

            return profile;
        }

        public string GetUserEmail(int UserId)
        {

            string email = "";
            ResponseBase response = new ResponseBase();
            DatabaseWrapper.databaseOperation(response,
                (context, query) =>
                {
                    email = query.GetUserEmail(context, UserId);
                }
                , readOnly: true
            );

            return email;
        }
    }
}