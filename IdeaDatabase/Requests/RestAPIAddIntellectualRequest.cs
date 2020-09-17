using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIAddIntellectualRequest : ValidableObject
    {
        public int IdeaId { get; set; }
        public string RecordId { get; set; }
        public string Status { get; set; }
        public DateTime FiledDate { get; set; }
        public int ApplicationNumber { get; set; }
        public int PatentId { get; set; }
    }
}