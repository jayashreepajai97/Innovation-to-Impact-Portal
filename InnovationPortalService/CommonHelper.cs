using IdeaDatabase.Utils;
using InnovationPortalService.Responses;
using Responses;
using SettingsRepository;
using System;
using System.Diagnostics.CodeAnalysis;

namespace InnovationPortalService
{
    public class CommonHelper : ICommonHelper
    {
        private static string _hppAppId = SettingRepository.Get<string>("HPPAppId");
        public string HPPAppId { get { return _hppAppId; } }
        

        [ExcludeFromCodeCoverage]
        private void TrustSSLServer()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
            ((sender, certificate, chain, sslPolicyErrors) => true);
        }

        public bool HandleHPPException(int retryCounter, Exception ex, Fault f, ResponseBase response)
        {
            if (retryCounter == RetryCounter.HPPRetryCounter)
            {
                if (ex.GetType() == typeof(TimeoutException))
                    response.ErrorList.Add(Faults.HPPTimeoutError);
                else
                    response.ErrorList.Add(f);

                return true;
            }
            return false;
        }
    }
}