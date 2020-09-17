using IdeaDatabase.DataContext;
using IdeaDatabase.Utils.Interface;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class LogUtils : ILogUtils
    {
        public void InsertIdeaLog(ResponseBase response, int IdeaId, string LogMessage, int Type, string OriginMethod, string OriginAPI, int UserId)
        {
            DatabaseWrapper.databaseOperation(response,
                      (context, query) =>
                      {
                          IdeaLog ideaLog = new IdeaLog()
                          {
                              IdeaId = IdeaId,
                              LogMessage = LogMessage,
                              Type = Type,
                              OriginMethod = OriginMethod,
                              OriginAPI = OriginAPI,
                              CreatedDate = DateTime.UtcNow,
                              UserId = UserId
                          };

                          query.AddIdeaLog(context, ideaLog);
                          context.SubmitChanges();
                      }
                      , readOnly: false
                  );

        }
    }
}