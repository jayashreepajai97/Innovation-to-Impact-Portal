using IdeaDatabase.Utils;
using InnovationPortalService.Responses;
using Responses;
using static InnovationPortalService.HPID.HPIDUtils;

namespace InnovationPortalService.HPID
{
    public interface IAUTHUtils
    {
        TokenDetails GetHPIDSessionToken(int tokenScope, string KeyValue1, string KeyValue2, ResponseBase response, string clientId,int refreshTokenType = 0);
        bool GetIdsAndProfile(CustomerIds ids, string token, GetProfileResponse response);
        string CreateHPIDNewCustomerProfile(string sessionToken, string requestJson, ResponseBase response);
        void UpdateHPIDCustomerProfile(string sessionToken, string requestJson, ResponseBase response);
        HPIDCustomerProfile GetProfile(string token);
    }
}