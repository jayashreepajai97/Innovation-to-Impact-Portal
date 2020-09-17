using IdeaDatabase.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
    public interface IUsersUtils
    {
        void SearchUser(RestAPISearchUserResponse response, string SearchName);
    }
}
