using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIArchiveIdeaRequest : ValidableObject
    {
        public bool IsArchive { get; set; } = false;
        [RequiredValidation]
        public int IdeaId { get; set; }
        [StringValidation(Required:true)]
        public string Description { get; set; }
    }
}