using System;
using System.Net.Http;

namespace IdeaDatabase.Utils
{
    public interface IHttpWebRequestFactory
    {
        bool IsValidImageResponse(string url, string method, int timeout);

        HttpResponseMessage Get(Uri url);
    }
}
