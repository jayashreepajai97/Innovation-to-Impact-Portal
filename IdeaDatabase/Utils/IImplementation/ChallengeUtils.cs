using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class ChallengeUtils : IChallengeUtils
    {
        public RestAPIGetIdeaChallengeResponse GetChallenges(RestAPIGetIdeaChallengeResponse response)
        {
            DatabaseWrapper.databaseOperation(response,
          (context, query) =>
          {
              List<IdeaChallenge> ideaChallenges = query.GetIdeaChallenges(context);

              if (ideaChallenges.Count != 0)
              {
                  response.ChallengeList = ideaChallenges.Select(s => new ChallengeResponse() { ChallengeID = s.IdeaChallengeId, ChallengeName = s.ChallengeName}).ToList();
                  response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
              }
              else
              {
                  response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure); ;
              }

          }, readOnly: true);

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
            return response;
        }

        public void InsertChallenge(RestAPIAddIdeaChallengeResponse response, string ChallengeName, int AddedByUserId)
        {
            DatabaseWrapper.databaseOperation(response,
            (context, query) =>
            {
                IdeaChallenge ideaChallenge = query.GetChallengeByName(context, ChallengeName);
                if (ideaChallenge == null)
                {
                    ideaChallenge = new IdeaChallenge() {ChallengeName = ChallengeName, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, AddedByUserId = AddedByUserId };
                    query.AddIdeaChallenge(context, ideaChallenge);
                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
                }
                else
                {
                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                    response.ErrorList.Add(Faults.IdeaChallengeNameExists);
                    return;
                }
                context.SubmitChanges();
            }
            , readOnly: false
            );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
                return;
            }
        }
    }
}