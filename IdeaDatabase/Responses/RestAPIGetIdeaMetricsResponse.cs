using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPIGetIdeaMetricsResponse : ResponseBase
    {
        public List<RESTAPIIdeaMetricsInterchange> IdeaMetricsList;
        public RestAPIGetIdeaMetricsResponse()
        {
            IdeaMetricsList = new List<RESTAPIIdeaMetricsInterchange>();
        }
    }
}