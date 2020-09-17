using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class IdeaSupportings :ResponseBase
    {
        public string Ideatags { get; set; }
        public string GitRepo { get; set; }
        public bool IsAttachment { get; set; } = false;
        public int AttachmentCount { get; set; } = 0;
        public List<IdeaAttachmentRequest> ideaAttachments { get; set; }

    }
}