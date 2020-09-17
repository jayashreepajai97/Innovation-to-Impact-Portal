using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using IdeaDatabase.Utils;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using Hpcs.DependencyInjector;
using Newtonsoft.Json;
using NLog;
using Responses;
using SettingsRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace InnovationPortalService.Utils
{

    public class EmailUtil : IEmailUtil
    {
        private static string URL = SettingRepository.Get<string>("APIGatewaySendEmailURL");
        private static string EmailFromSES = SettingRepository.Get<string>("EmailFromSES");
        private static string EmailFromNameSES = SettingRepository.Get<string>("EmailFromNameSES");
        
        public EmailTemplate GetEmailTemplate(string emailTemplate)
        {
            ResponseBase response = new ResponseBase();
            EmailTemplate emailTemplateObj = new EmailTemplate();
            DatabaseWrapper.databaseOperation(response,
                (context, query) =>
                {
                    emailTemplateObj = query.GetEmailTemplate(context, emailTemplate);
                }
                , readOnly: true
            );
            return emailTemplateObj;
        }

        public HttpResponseMessage SendEmail(string EmailTo, string EmailBody, string EmailSubject)
        {
            var client = new HttpClient();

            EmailData data = new EmailData();
            data.EmailBody = EmailBody;
            data.EmailSubject = EmailSubject;
            data.EmailTo = EmailTo;
            data.FromName =EmailFromNameSES ;
            data.EmailFrom = EmailFromSES;

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var res= client.PostAsync(URL, content).Result;

            return res;
        }
    }


}