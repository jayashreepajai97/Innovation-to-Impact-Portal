using IdeaDatabase.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIUserInterchange
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public RESTAPIUserInterchange(User user)
        {
            if (user != null)
            {
                UserId = user.UserId;
                FirstName = user.FirstName;
                LastName = user.LastName;
                EmailAddress = user.EmailAddress;
            }

        }
    }
}