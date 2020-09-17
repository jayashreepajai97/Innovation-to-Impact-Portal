using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
    public interface ILogUtils
    {
        void InsertIdeaLog(ResponseBase response, int IdeaId, string LogMessage, int Type, string OriginMethod, string OriginAPI, int UserId);
    }
}
