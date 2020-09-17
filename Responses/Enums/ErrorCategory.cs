using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responses.Enums
{
    public enum ErrorCategory
    {
        General,
        SessionTimeout,       
        InvalidSNFormat,
        SNNotFound,
        IdeaAlreadyRegistered         
    }
}
