using Hpcs.DependencyInjector;
using SettingsRepository;
using IdeaDatabase.Utils;
using IdeaDatabase.Interchange;
using InnovationPortalService.Responses;

namespace InnovationPortalService.Utils
{
    public class ProfileUtils : IProfileUtils
    {
        private ICustomerHPIDUtils hpidUtils = DependencyInjector.Get<ICustomerHPIDUtils, CustomerHPIDUtils>();
        private ICustomerUtils customerUtils = DependencyInjector.Get<ICustomerUtils, CustomerUtils>();

        public void GetHPProfile(ref GetProfileResponse profileResponse, int customerId, string Token, string CallerId, string LanguageCode, string CountryCode, string SessionToken, TokenScopeType tokenScopeType)
        {
            UserAuthenticationInterchange hppAuthInterchange = new UserAuthenticationInterchange()
            {               
                UserId = customerId,
                CallerId = CallerId,
                LanguageCode = LanguageCode,
                CountryCode = CountryCode
            };

            if (SettingRepository.Get<bool>("HPIDEnabled", true))
            {
                var token = new TokenDetails() { AccessToken = Token, tokenScopeType = tokenScopeType };
                profileResponse = hpidUtils.GetCustomerProfileforHPID(hppAuthInterchange, token, true, IdeaDatabase.Enums.APIMethods.None);
            }
            //else
            //{
            //    hppAuthInterchange.Token = SessionToken;
            //    profileResponse = customerUtils.GetCustomerProfile(hppAuthInterchange, true);
            //}
        }

        public EasCustomerProfile GetHPProfile(int UserID, string Token, string CallerId, string LanguageCode, string CountryCode, string SessionToken)
        {
            EasCustomerProfile profile = new EasCustomerProfile(UserID);

            GetProfileResponse profileResponse = new GetProfileResponse();
            GetHPProfile(ref profileResponse, UserID, Token, CallerId, LanguageCode, CountryCode, SessionToken, TokenScopeType.apiProfileGetCall);

            if (profileResponse.ErrorList.Count == 0 && profileResponse.CustomerProfileObject != null)
            {
                profile.CallerId = CallerId;
                profile.TokenMD5 = SessionToken;
                profile.FirstName = !string.IsNullOrEmpty(profileResponse?.CustomerProfileObject?.FirstName) ? profileResponse.CustomerProfileObject.FirstName : string.Empty;
                profile.LastName = !string.IsNullOrEmpty(profileResponse?.CustomerProfileObject?.LastName) ? profileResponse.CustomerProfileObject.LastName : string.Empty;
                profile.EmailAddress = !string.IsNullOrEmpty(profileResponse?.CustomerProfileObject?.EmailAddress) ? profileResponse.CustomerProfileObject.EmailAddress : string.Empty;
                profile.CountryCode = !string.IsNullOrEmpty(profileResponse?.CustomerProfileObject?.Country) ? profileResponse.CustomerProfileObject.Country : SettingRepository.Get<string>("EASDeafaultPurchaseCountryCode");
                profile.LanguageCode = !string.IsNullOrEmpty(profileResponse?.CustomerProfileObject?.Language) ? profileResponse.CustomerProfileObject.Language : "en";
                profile.Locale = TranslationUtils.Locale(LanguageCode, CountryCode);
            }

            return profile;
        }
    }
}