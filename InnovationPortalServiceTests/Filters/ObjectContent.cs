using Responses;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace InnovationPortalServiceTests.Filters
{
    internal class ObjectContent : HttpContent
    {
        private Type type;
        private ResponseBase response;
        private JsonMediaTypeFormatter jsonMediaTypeFormatter;

        public ObjectContent(Type type, ResponseBase response, JsonMediaTypeFormatter jsonMediaTypeFormatter)
        {
            this.type = type;
            this.response = response;
            this.jsonMediaTypeFormatter = jsonMediaTypeFormatter;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            throw new NotImplementedException();
        }

        protected override bool TryComputeLength(out long length)
        {
            throw new NotImplementedException();
        }
    }
}