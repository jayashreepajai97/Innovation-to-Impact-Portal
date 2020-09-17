using InnovationPortalService.Responses;
using Newtonsoft.Json;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Filters
{
    public class CaptchaAuthorizationAttribute : AuthorizeAttribute
    {
        private static readonly string PrivateKey = "6LfMrCATAAAAAFbxJAgcgsEnKN16YjHTU4ddxsMD";
        private static readonly string VerifyURL = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!ConfigurationUtils.CaptchaRequired)
                return;
            ResponseBase r = new ResponseBase();
            var tmp = actionContext.Request.Headers.Where(x => x.Key == "ReCaptchaToken").FirstOrDefault();
            var captcha = tmp.Value != null ? tmp.Value.First() : null;

            if (string.IsNullOrEmpty(captcha))
            {
                r.ErrorList.Add(Faults.MissingReCaptchaToken);
            }
            else
            {
                try
                {
                    var client = new WebClient();
                    var GoogleReply = client.DownloadString(string.Format(VerifyURL, PrivateKey, captcha));
                    var captchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(GoogleReply);
                    if (captchaResponse.Success == "false")
                        r.ErrorList.Add(Faults.ReCaptchaValidationFailed);
                }
                catch (Exception)
                {
                    r.ErrorList.Add(Faults.ReCaptchaValidationFailed);
                }
            }

            if(r.ErrorList.Count > 0)
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, r, GlobalConfiguration.Configuration);

        }

        class ReCaptchaResponse
        {
            [JsonProperty("success")]
            public string Success
            {
                get { return m_Success; }
                set { m_Success = value; }
            }

            private string m_Success;
            [JsonProperty("error-codes")]
            public List<string> ErrorCodes
            {
                get { return m_ErrorCodes; }
                set { m_ErrorCodes = value; }
            }


            private List<string> m_ErrorCodes;
        }
        
    }
}