using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeaMetricsInterchange
    {
        public Nullable<int> IdeaId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }

        public RESTAPIIdeaMetricsInterchange(IdeaLog ideaLog)
        {
            if(ideaLog != null)
            {
                IdeaId = ideaLog.IdeaId;
                UserName = string.Concat(ideaLog.User.FirstName, " ", ideaLog.User.LastName);
                CreatedDate = ideaLog.CreatedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

                if (ideaLog.OriginMethod == Enum.GetName(typeof(IdeaMethodTypes), IdeaMethodTypes.UpdateDetails))
                    Message = string.Format("{0} {1}", ideaLog.LogMessage, UserName);
                else
                    Message = string.Format("{0} {1}", UserName, ideaLog.LogMessage);
            }
        }
    }
}