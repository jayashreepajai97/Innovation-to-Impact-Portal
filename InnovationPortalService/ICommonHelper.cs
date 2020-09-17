//using InnovationPortalService.HPPService;
using Responses;
using System;

namespace InnovationPortalService
{
    public interface ICommonHelper
    {
       // hppwsHeaderElement GetHPPContext(string version);
       // void SetAuthenticationHeaders(ICustProdRegistrationsserviceagent svc);
       // void SetHPPAuthenticationHeaders(DocLiteral hppService);
       // loginRequest GetLoginRequest(string UserName, string Password);
       // getUserResponse GetUserProfile(DocLiteral hppService, string SessionToken, ResponseBase response);
        bool HandleHPPException(int retryCounter, Exception ex, Fault f, ResponseBase response);
        string HPPAppId { get; }
    }
}