namespace IdeaDatabase.Utils
{
    public enum TokenScopeType
    {
        userLogin,          // user login with userName & password
        userAuthenticate,   // user authenticates by access token & redirect URL 
        userCreate,        // create new user profile 
        userRefreshToken,
        apiCall,// RESTAPIProfile call  
        apiProfileGetCall,
        apiProfileGetByTokenCall

    }

    public enum RefreshTokenLoginType
    {
        Credentials,      // user login with userName & password
        Authentication    // user authenticates by access token & redirect URL 
    }

    public class TokenDetails
    {
        public string AccessToken { get; set; }
        public TokenScopeType tokenScopeType { get; set; }
        public string RefreshToken { get; set; }

        /// <summary>
        /// info how refresh token was created: 
        /// 0 - by standard login with credentials
        /// 1 - bu AUTH with access code and redirect URL
        /// </summary>
        public int RefreshTokenType;
    }
}