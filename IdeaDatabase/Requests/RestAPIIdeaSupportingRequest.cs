using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIIdeaSupportingRequest : IdeaSupportings
    {
        public int UserID { get; set; }
        public int IdeaId { get; set; }
        public RestAPIIdeaSupportingRequest()
        {
            ideaAttachments = new List<IdeaAttachmentRequest>();
        }
    }
}