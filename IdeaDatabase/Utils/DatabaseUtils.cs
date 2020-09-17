using System;
using System.Text;
using SettingsRepository;
using System.Threading.Tasks;
using IdeaDatabase.DataContext;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using NLog;

namespace IdeaDatabase.Utils
{
    internal static class LogUtils
    {
        private const int MaxParseLength = 1024; //1KB
        private const string PasswordPattern = "([\\s\\S]*)(\"[Pp]assword\" *: *)(\")([^\"]*)(\")([\\s\\S]*)";

       

        internal static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return (value.Length <= maxLength) ? value : value.Substring(0, maxLength);
        }

        internal static string MaskPassword(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            string parsePart = null;
            string remainingPart = null;

            if (input.Length > MaxParseLength)
            {
                parsePart = input.Substring(0, MaxParseLength);
                remainingPart = input.Substring(MaxParseLength, (input.Length - MaxParseLength));
            }
            else
            {
                parsePart = input;
            }

            Match match = Regex.Match(parsePart, PasswordPattern, RegexOptions.Compiled);

            if (!match.Success)
            {
                return input;
            }

            StringBuilder output = new StringBuilder();

            output.Append(match.Groups[1]);
            output.Append(match.Groups[2]);
            output.Append(match.Groups[3]);
            output.Append(new string('*', match.Groups[4].Length));
            output.Append(match.Groups[5]);
            output.Append(match.Groups[6]);
            if (remainingPart != null)
            {
                output.Append(remainingPart);
            }
            return output.ToString();
        }

    }

    public class AdmLog
    {
        private const int MaxHeaderLength = 1024; //1KB
        private const int MaxLength = 4096;       //4KB

        private string request;
        private string response;
        private string header;
        private string requestType;
        private string methodName;
        private int? elapsedTime;

        private AdmLog()
        {
        }

        private AdmLog(string header, string requestType, string methodName, string request, string response, int? elapsedTime)
        {
            this.header = LogUtils.Truncate(header, MaxHeaderLength);
            this.requestType = requestType;
            this.methodName = methodName;
            this.request = LogUtils.Truncate(LogUtils.MaskPassword(request), MaxLength);
            this.response = LogUtils.Truncate(LogUtils.MaskPassword(response), MaxLength);
            this.elapsedTime = elapsedTime;
        }

        public static void LogApiCallDetails(string header, string requestType, string methodName, string request, string response, int? elapsedTime)
        {
            try
            {
                Task.Factory.StartNew(
                    () =>
                    {
                        try
                        {
                            AdmLog logData = new AdmLog(header, requestType, methodName, request, response, elapsedTime);
                            logData.Save();
                        }
                        catch
                        {
                            // Ignore any exception encountered during logging
                        }
                    }
                );
            }
            catch
            {
                // Ignore any exception encountered during logging
            }
        }

        private void Save()
        {
            // This stored procedure is used for storing the log data
            /*
            CREATE PROCEDURE SaveAdmLog(IN input_RequestTime INT, 
                IN input_RequestType VARCHAR(8), 
                IN input_MethodName VARCHAR(48), 
                IN input_Header VARCHAR(1024), 
                IN input_Request VARCHAR(4096), 
                IN input_Response VARCHAR(4096))
            BEGIN
                INSERT INTO AdmLogs(RequestTime, RequestType, MethodName, 
                    Header, Request, Response)
                VALUES(input_RequestTime, input_RequestType, input_MethodName, 
                input_Header, input_Request, input_Response);
            END
            */

            // The table schema for storing the log data is as follows
            /*
            CREATE TABLE AdmLogs (
                Id int(11) NOT NULL AUTO_INCREMENT,
                EventDate timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                RequestTime int(11) DEFAULT NULL,
                RequestType varchar(8) DEFAULT NULL,
                MethodName varchar(256) DEFAULT NULL,
                Header varchar(1024) DEFAULT NULL,
                Request varchar(4096) DEFAULT NULL,
                Response varchar(4096) DEFAULT NULL,
                PRIMARY KEY (Id)
            );
            */

            //using (IIdeaDatabaseDataContext context = new IdeaDatabaseReadWrite())
            //{
            //    context.SaveAdmLog(elapsedTime, requestType, methodName, header, request, response);
            //}
        }
    }

    public class ExtServiceLogs
    {
        private static Logger logger = LogManager.GetLogger("DatabaseSubmitChanges");
        private static bool IsEnableExtServiceLog = SettingRepository.Get<bool>("EnableExtServiceLog");
        private const int MaxLength = 4096; //4KB

        private string request;
        private string response;
        private string requestType;
        private string serviceName;
        private string methodName;
        private string header;
        private int requestTime;

        private ExtServiceLogs()
        {
           
        }
         

        public override string ToString()
        {
            string logmessage = $"RequestType={requestType},ServiceName={serviceName},MethodName={methodName},Header={header}," +
                $"request={request},response={response},requestTime={requestTime}";
            return logmessage.ToString();
        }

        public static void LogExtServiceCallDetails(string clientHeader, HttpVerbs? requestType, string serviceName, 
            string methodName, string requestObj, string responseObj, int elapsedTime, bool hasPassword = false)
        {
            try
            {
                if (IsEnableExtServiceLog)
                {
                    Task.Factory.StartNew(
                        () =>
                        {
                            try
                            {
                                ExtServiceLogs logData = new ExtServiceLogs()
                                {
                                    requestType = requestType?.ToString().ToUpper(),
                                    serviceName = serviceName,
                                    methodName = methodName,
                                    header = clientHeader,
                                    request = hasPassword
                                        ? LogUtils.Truncate(LogUtils.MaskPassword(requestObj), MaxLength)
                                        : LogUtils.Truncate(requestObj, MaxLength),
                                    response = hasPassword
                                        ? LogUtils.Truncate(LogUtils.MaskPassword(responseObj), MaxLength)
                                        : LogUtils.Truncate(responseObj, MaxLength),
                                    requestTime = elapsedTime
                                };

                                logger.Info(typeof(ExtServiceLogs).ToString(), logData.ToString());
                            }
                            catch
                            {
                                // Ignore any exception encountered during logging
                            }
                        }
                    );
                }
            }
            catch
            {
                // Ignore any exception encountered during logging
            }
        }

        private void Save()
        {
            

            // This stored procedure is used for storing the log data
            /*
            CREATE PROCEDURE `SaveExtServiceLogs`(IN input_RequestTime INT, 
                IN input_RequestType VARCHAR(8),
                IN input_ServiceName VARCHAR(256),  
                IN input_MethodName VARCHAR(48), 
                IN input_Header VARCHAR(1024), 
                IN input_Request VARCHAR(4096), 
                IN input_Response VARCHAR(4096))
            BEGIN
                INSERT INTO ExtServiceLogs(RequestTime, RequestType, ServiceName, 
                    MethodName, Header, Request, Response) 
                VALUES (input_RequestTime, input_RequestType, input_ServiceName, 
                    input_MethodName, input_Header, input_Request, input_Response);
            END
            */

            // The table schema for storing the log data is as follows
            /*
            CREATE TABLE IF NOT EXISTS ExtServiceLogs 
            (
                Id int(11) NOT NULL AUTO_INCREMENT,
                EventDate timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                RequestTime int(11) DEFAULT NULL,
                RequestType varchar(8) DEFAULT NULL,
                ServiceName varchar(256) DEFAULT NULL,  
                MethodName varchar(16) DEFAULT NULL,
                Header varchar(1024) DEFAULT NULL,
                Request varchar(4096) DEFAULT NULL,
                Response varchar(4096) DEFAULT NULL,
                PRIMARY KEY (Id)
            );
            */

            //using (IdeaDatabaseDataContext context = new DeviceDatabaseReadWrite())
            //{
            //    context.SaveExtServiceLogs(requestTime, requestType, serviceName, methodName, header, request, response);
            //}
        }
    }
}