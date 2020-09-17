using IdeaDatabase.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace InnovationPortalService.Requests
{
    public class RestAPISubmitIntellectualRequest : SubmitIntellectualRequest
    {
        public IList<HttpContent> files { get; }

        public RestAPISubmitIntellectualRequest()
        {
            files = new List<HttpContent>();
            ideaAttachments = new List<IdeaAttachmentRequest>();
        }
    }
}