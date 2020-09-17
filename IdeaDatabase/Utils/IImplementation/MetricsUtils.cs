using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class MetricsUtils : IMetricsUtils
    {
        string Failure = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
        string Success = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);

        public void GetIdeasMetrics(RestAPIGetIdeaMetricsResponse response, int IdeaId)
        {
            List<RESTAPIIdeaMetricsInterchange> ideaMetricsInterchangeList = null;
            List<IdeaLog> ideaMetricList = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             ideaMetricsInterchangeList = new List<RESTAPIIdeaMetricsInterchange>();
                             ideaMetricList = new List<IdeaLog>();

                             ideaMetricList = query.GetIdeaMetrics(context, IdeaId);

                             if (ideaMetricList.Count > 0)
                             {
                                 foreach (var ideaMetric in ideaMetricList)
                                 {
                                     RESTAPIIdeaMetricsInterchange ideaInterchange = new RESTAPIIdeaMetricsInterchange(ideaMetric);
                                     ideaMetricsInterchangeList.Add(ideaInterchange);
                                 }
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );

            if (ideaMetricsInterchangeList != null && ideaMetricsInterchangeList.Count > 0)
                response.IdeaMetricsList.AddRange(ideaMetricsInterchangeList);
        }

       
    }
}