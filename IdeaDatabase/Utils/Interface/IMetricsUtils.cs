using IdeaDatabase.Responses;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
    public interface IMetricsUtils
    {
        void GetIdeasMetrics(RestAPIGetIdeaMetricsResponse response, int IdeaId);
    }
}
