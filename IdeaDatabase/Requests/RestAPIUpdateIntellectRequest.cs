using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIUpdateIntellectRequest : ValidableObject
    {
        public string RecordId { get; set; }
        public string Status { get; set; }
        public int ApplicationNumber { get; set; }
        public int PatentId { get; set; }
    }
}