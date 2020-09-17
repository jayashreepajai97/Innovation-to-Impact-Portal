using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace InnovationEmailLamda
{
    public class Function
    {

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(Details input, ILambdaContext context)
        {
          

            // Replace smtp_username with your Amazon SES SMTP user name.
            String SMTP_USERNAME = "AKIATTET35X37S4BY6M2";

            // Replace smtp_password with your Amazon SES SMTP user name.
            String SMTP_PASSWORD = "BHDHsosezYmONe+ra5YZp/Wx6Yapkl6J312ACXCWujBB";

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            // String CONFIGSET = "ConfigSet";

            String HOST = "email-smtp.us-east-1.amazonaws.com";

            int PORT = 587;

            // The subject line of the email
            //String EmailSubject =
            //    "Amazon SES test (SMTP interface accessed using C#)";

            string res = "";

            // Create and build a new MailMessage object

            List<string> emails = input.EmailTo.Split(',').ToList();

            MailMessage message = new MailMessage();

            message.IsBodyHtml = true;
            message.From = new MailAddress(input.EmailFrom, input.FromName);

            foreach (var email in emails)
            {
                message.To.Add(new MailAddress(email));
            }
            message.Subject = input.EmailSubject;
            message.Body = input.EmailBody;

            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Enable SSL encryption
                client.EnableSsl = true;

                try
                {
                    Console.WriteLine("Attempting to send email...");
                    client.Send(message);
                    res = "Successful";
                }
                catch (Exception)
                {
                    res = "Failed";
                }
            }

            IDictionary<string, string> headers = new Dictionary<string, string>()
                                            {
                                                {"Content-Type", "application/json"}
                                            };

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = headers,
                Body = res,
                IsBase64Encoded = false
            };
        }
    }
}
