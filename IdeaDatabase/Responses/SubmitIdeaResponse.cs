using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPISubmitIdeaResponse : ResponseBase
    {
        public int IdeaId { get; set; }
    }

    public class SubmitIdeaAttachmentResponse : ResponseBase
    {

    }
}