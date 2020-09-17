using System;

namespace InnovationPortalService.Filters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SkipLoggingAttribute : Attribute
    {
        public string ErrorCode;
        
        public SkipLoggingAttribute()
        {

        }

        public SkipLoggingAttribute(string errorCode)
        {
            ErrorCode = errorCode;
        }
    }

}