using Responses;

namespace HPSAProfileService.Responses
{
    public class SetSessionCacheResponse : ResponseBase
    {
        public string CacheToken { get; set; }
    }
}