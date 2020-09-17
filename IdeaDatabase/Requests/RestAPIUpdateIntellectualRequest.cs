using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIUpdateIntellectualRequest : UpdateIntellectualRequest
    {
        public IList<HttpContent> files { get; }

        public RestAPIUpdateIntellectualRequest()
        {
            files = new List<HttpContent>();
            ideaAttachments = new List<IdeaAttachmentRequest>();
        }
    }
}