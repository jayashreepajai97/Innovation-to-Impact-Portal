using IdeaDatabase.Interchange;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public class RestAPIGetAttachmentsResponse : ResponseBase
    {
        public List<RESTAPIIdeaAttachmentsInterchange> IdeaAttachmentList;

        public RestAPIGetAttachmentsResponse()
        {
            IdeaAttachmentList = new List<RESTAPIIdeaAttachmentsInterchange>();
        }
    }
}