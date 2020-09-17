using IdeaDatabase.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
    public interface IIdeaAssignmentUtils
    {
        void SubmitIdeaAssignments(RestAPIAddIdeaStateResponse response, int UserId, int IdeaId, int ideaState);
    }
}
