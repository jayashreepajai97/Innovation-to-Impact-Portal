using System;
using System.Net;
using System.Net.Http;

namespace IdeaDatabase.Utils
{
    public class HttpWebRequestFactory : IHttpWebRequestFactory
    {
        private HttpClient client;
        public bool IsValidImageResponse(string requestUrl, string requestMethod, int requestTimeout)
        {
            try
            {
                HttpWebRequest webRequest = HttpWebRequest.Create(requestUrl) as HttpWebRequest;
                webRequest.Timeout = requestTimeout;
                webRequest.Method = requestMethod;

                using (HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse)
                {
                    if (webResponse.StatusCode == HttpStatusCode.OK && webResponse.ContentType.StartsWith("image/"))
                    {
                        return true;
                    }
                }
            }
            catch (Exception) { }

            return false;
        }

        public HttpResponseMessage Get(Uri url)
        {
            using (client = new HttpClient())
            {
                return client.GetAsync(url).Result;
            }
        }
    }
}