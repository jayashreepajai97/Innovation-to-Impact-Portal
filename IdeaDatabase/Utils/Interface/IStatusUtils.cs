using IdeaDatabase.DataContext;
using IdeaDatabase.Responses;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IdeaDatabase.Utils.Interface
{
    public interface IStatusUtils
    {
        void GetRoles(RESTAPIGetRolesResponse response);
        void InsertStatus(RestAPIAddIdeaStateResponse response, int IdeaID, int ideaState, int UserId);
    }
}