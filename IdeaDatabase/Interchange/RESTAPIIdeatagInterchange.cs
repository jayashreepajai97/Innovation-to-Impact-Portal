using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeatagInterchange
    {

        public string Tags { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<int> AddedByUserId { get; set; }
    }
}