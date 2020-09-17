using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using NLog;
using Responses;
using System;
using System.Collections.Generic;

namespace IdeaDatabase.Utils
{
    public class DatabaseWrapper
    {
        private static Logger logger = LogManager.GetLogger("DatabaseSubmitChanges");
        
        private static void basicDatabaseOperation(ResponseBase response, Action action, string methodName = null)
        {
            if (methodName != null) logger.Info($"basicDatabaseOperati | Start");

            int retryCount = 0;
            List<Fault> returnedFault = new List<Fault>();
            while (retryCount < RetryCounter.DbRetryCounter)
            {
                try
                {
                    action();
                    if (methodName != null) logger.Info($"basicDatabaseOperati | Success");
                    return;
                }
                catch (Exception ex)
                {
                    if (methodName != null)
                    {
                        string innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
                        logger.Info($"basicDatabaseOperati | Exception: retry='{retryCount}' exMsg='{ex.Message}' exInner='{innerMsg}'");
                    }

                    retryCount++;
                    if (ConfigurationUtils.ShowOnlyOneServerIsBusyError && retryCount > 1)
                    {
                        continue;
                    }
                    returnedFault.Add(new Fault(Faults.ServerIsBusy, ex));
                    if (ex.InnerException != null)
                    {
                        returnedFault.Add(new Fault(Faults.ServerIsBusy, ex.InnerException));
                    }
                }
            }

            if (retryCount == RetryCounter.DbRetryCounter)
            {
                response.ErrorList.UnionWith(returnedFault);
            }
        }

        public static void databaseOperation(ResponseBase response, Action<IIdeaDatabaseDataContext, IQueryUtils> t,
            bool readOnly)
        {
            string methodName = null;
         

            basicDatabaseOperation(response,
                () =>
                {
                    IIdeaDatabaseDataContext context;
                    if (readOnly == true)
                    {
                        context = DependencyInjector.Get<IIdeaDatabaseDataContext, IdeaDatabaseReadOnly>();
                    }
                    else
                    {
                        context = DependencyInjector.Get<IIdeaDatabaseDataContext, IdeaDatabaseReadWrite>();
                    }
                    IQueryUtils query = DependencyInjector.Get<IQueryUtils, QueryUtils>();
                    t(context, query);
                },
                methodName
            );
        }

        public static void databaseOperation(ResponseBase response, int UserID, int IdeaId,
            Relation r, Action<IIdeaDatabaseDataContext, IQueryUtils, Idea> t, bool readOnly)
        {
            databaseOperationWithIdea(response, UserID,
                (context, query) =>
                {
                    return query.GetIdea(context, IdeaId, UserID, r);
                },
                r, t, readOnly, Faults.InvalidSerialNumber
            );
        }

        

        public static void databaseOperation(ResponseBase response, IIdeaDatabaseDataContext context,
            Action<IIdeaDatabaseDataContext> t)
        {
            basicDatabaseOperation(response,
                () =>
                {
                    t(context);
                }
            );
        }

        private static void databaseOperationWithIdea(ResponseBase response, int UserID,
          Func<IIdeaDatabaseDataContext, IQueryUtils, Idea> GetIdea,
          Relation r, Action<IIdeaDatabaseDataContext, IQueryUtils, Idea> t, bool readOnly, Fault notFoundFault)
        {
            basicDatabaseOperation(response,
                () =>
                {
                    IIdeaDatabaseDataContext context;
                    if (readOnly == true)
                        context = DependencyInjector.Get<IIdeaDatabaseDataContext, IdeaDatabaseReadOnly>();
                    else
                        context = DependencyInjector.Get<IIdeaDatabaseDataContext, IdeaDatabaseReadWrite>();

                    IQueryUtils query = DependencyInjector.Get<IQueryUtils, QueryUtils>();

                    //check if idea is registered for the user
                    Idea idea = GetIdea(context, query);
                    if (idea == null)
                    {
                        response.ErrorList.Add(notFoundFault);
                        return;
                    }
                    t(context, query, idea);
                }
            );
        }

    }
}