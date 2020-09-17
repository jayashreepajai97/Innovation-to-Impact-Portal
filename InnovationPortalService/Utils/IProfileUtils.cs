using IdeaDatabase.Utils;
using IdeaDatabase.Interchange;
using InnovationPortalService.Responses;

namespace InnovationPortalService.Utils
{
    public interface IProfileUtils
    {
        void GetHPProfile(ref GetProfileResponse profileResponse, int customerId, string Token, string CallerId, string LanguageCode, string CountryCode, string SessionToken, TokenScopeType tokenScopeType);
        EasCustomerProfile GetHPProfile(int UserID, string Token, string CallerId, string LanguageCode, string CountryCode, string SessionToken);
    }
}
