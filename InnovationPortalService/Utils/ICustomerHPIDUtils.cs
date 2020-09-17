using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Utils;
using InnovationPortalService.Responses;
using Responses;

namespace InnovationPortalService.Utils
{
    public interface ICustomerHPIDUtils
    {
        GetProfileResponse GetCustomerProfileforHPID(UserAuthenticationInterchange HppAuthInterchange, TokenDetails SessionToken, bool RetainOldValues, APIMethods apiRetainOldValues);
        GetProfileResponse GetCustomerProfileByLogin(UserAuthenticationInterchange HppAuthInterchange, bool RetainOldValues, APIMethods apiRetainOldValues);
        GetProfileResponse GetCustomerProfileByAuthentication(UserAuthenticationInterchange HppAuthInterchange, bool RetainOldValues, string AccessCode, string RedirectUrl, APIMethods apiRetainOldValues, string ClientId);
        void ExecuteLogout(ResponseBase response, int customerId, string sessionToken, string callerId);
        GetProfileByTokenResponse GetCustomerProfileByTokenforHPID(UserAuthenticationInterchange HPPAuthInterchange, TokenDetails sessionTokenDetails, string languageCode ,string countryCode, APIMethods apiRetainOldValues);

        GetProfileResponse GetCustomerProfileByTestLogin(UserAuthenticationInterchange UserAuthInterchange, bool RetainOldValues, APIMethods apiRetainOldValues);
        GetProfileResponse GetCustomerProfileByDefaultUserLogin(GetProfileResponse response, UserAuthenticationInterchange UserAuthInterchange, bool RetainOldValues, APIMethods apiRetainOldValues);

    }
}
