using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;
//using InnovationPortalService.HPPService;
using InnovationPortalService.Responses;
using Responses;
using System.Collections.Generic;

namespace InnovationPortalService.Utils
{
    public interface ICustomerUtils
    { 
        //GetProfileResponse GetCustomerProfile(UserAuthenticationInterchange hppAuthInterchange, bool RetainOldValues);
         
        
        void InsertOrUpdateHPPToken(GetProfileResponse response, UserAuthentication UserAuthentication, bool RetainOldValues);
       // GetProfileResponse RefreshToken(string UserName, string Password, string CallerId, int UserID, bool RetainOldValues);

        void UpdateLogoutDate(ResponseBase response, int customerId, string callerId, string tokenMd5);


    }
}