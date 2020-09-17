using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class EmailData
    {
        public string EmailTo { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string FromName { get; set; }
        public string EmailFrom { get; set; }
    }
}