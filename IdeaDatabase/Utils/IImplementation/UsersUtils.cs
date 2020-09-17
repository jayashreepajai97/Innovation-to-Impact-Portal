using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class UsersUtils : IUsersUtils
    {
        public void SearchUser(RestAPISearchUserResponse response, string SearchName)
        {
            List<RESTAPIUserInterchange> userInterchangeList = null;
            List<User> userlist = null;


            DatabaseWrapper.databaseOperation(response,
                        (context, query) =>
                        {

                            userInterchangeList = new List<RESTAPIUserInterchange>();
                            userlist = new List<User>();

                            if (!String.IsNullOrEmpty(SearchName))
                            {
                                userlist = query.GetUsers(context, SearchName);
                                if (userlist.Count > 0)
                                {
                                    foreach (var user in userlist)
                                    {
                                        RESTAPIUserInterchange userInterchange = new RESTAPIUserInterchange(user);
                                        userInterchangeList.Add(userInterchange);
                                    }
                                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
                                }
                                else
                                {
                                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                                    response.ErrorList.Add(Faults.UserNotFound);
                                    return;
                                }
                            }
                            else
                            {
                                response.ErrorList.Add(Faults.InvalidSearch);
                                return;
                            }
                        }
                        , readOnly: true
                    );

            if (userInterchangeList != null && userInterchangeList.Count > 0)
                response.UserSearchList.AddRange(userInterchangeList);

        }
    }
}