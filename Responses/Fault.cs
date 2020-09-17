using System;
using Responses.Enums;

namespace Responses
{
    public class Fault
    {
        public string Origin { get; set; }

        public string ReturnCode { get; set; }

        public ErrorCategory ErrorCategory { get; set; }

        public string DebugStatusText { get; set; }

        public string StatusText { get; set; }

        public string DebugMessage { get; set; }

        public string DebugStackTrace { get; set; }

        public Fault(string o, string e, string m, ErrorCategory c = ErrorCategory.General)
        {
            Origin = o;
            ReturnCode = e;
            DebugStatusText = m;
            ErrorCategory = c;
        }

        public Fault(string e, string m, ErrorCategory c = ErrorCategory.General) : this("InnovationPortalService", e, m, c)
        {
        }

        public Fault(Fault f, Exception e)
        {
            Origin = f.Origin;
            ReturnCode = f.ReturnCode;
            DebugStatusText = f.DebugStatusText;
            ErrorCategory = f.ErrorCategory;
            if (e != null && ConfigurationUtils.ShowStackTrace)
            {
                DebugMessage = e.Message;
                DebugStackTrace = e.StackTrace;
            }
        }
    }
}