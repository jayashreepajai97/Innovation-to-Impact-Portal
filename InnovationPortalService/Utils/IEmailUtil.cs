using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InnovationPortalService.Utils
{
    interface IEmailUtil
    {
        HttpResponseMessage SendEmail(string EmailTo, string EmailBody, string EmailSubject);
        EmailTemplate GetEmailTemplate(string emailTemplate);
    }
}