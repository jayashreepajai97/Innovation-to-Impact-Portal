using IdeaDatabase.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
    public interface IChallengeUtils
    {
        void InsertChallenge(RestAPIAddIdeaChallengeResponse response, string ChallengeName, int AddedByUserId);
        RestAPIGetIdeaChallengeResponse GetChallenges(RestAPIGetIdeaChallengeResponse response);

    }
}
