////using InnovationPortalService.HPPService;
using Hpcs.DependencyInjector;

namespace InnovationPortalService.Utils
{
    public class AuthenticationUtils : IAuthenticationUtils
    {
        private ICommonHelper commonHelper;
        // DocLiteral hppService;

        public AuthenticationUtils()
        {
            commonHelper = DependencyInjector.Get<ICommonHelper, CommonHelper>();
            //hppService = DependencyInjector.Get<DocLiteral, HPPClient>();
            //commonHelper.SetHPPAuthenticationHeaders(hppService);
        }

        public string GetProfileIdByEmail(string EmailAddress)
        {
            return null;
            //    checkUserExistsRequest verify = new checkUserExistsRequest();
            //    verify.hppwsHeaderElement = commonHelper.GetHPPContext("3");
            //    verify.checkUserExistsRequestElement = new checkUserExistsRequestType() { email = EmailAddress,userId = commonHelper.HPPAppId };
            //    checkUserExistsResponse vResponse = null;
            //    try
            //    {
            //        vResponse = hppService.checkUserExists(verify);
            //    }
            //    catch (FaultException<genericFault> ex)
            //    {
            //        if (ex.Detail.fault[0].ruleNumber == 268)                
            //            return null;                

            //        throw ex;                
            //    }
            //    return (vResponse == null) ? null : vResponse.checkUserExistsResponseElement.profileIdByEmail;
        }

        public string GetProfileIdByUserId(string UserId)
        {
            return null;
            //    checkUserExistsRequest verify = new checkUserExistsRequest();
            //    verify.hppwsHeaderElement = commonHelper.GetHPPContext("3");
            //    verify.checkUserExistsRequestElement = new checkUserExistsRequestType () { userId = UserId , applicationId = commonHelper.HPPAppId};
            //    checkUserExistsResponse vResponse = hppService.checkUserExists(verify);

            //    return (vResponse == null) ? null : vResponse.checkUserExistsResponseElement.profileIdByUserId;
        }
    }
}