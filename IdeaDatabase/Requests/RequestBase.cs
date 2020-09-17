using Credentials;
using IdeaDatabase.Credentials;
using IdeaDatabase.Validation;

namespace IdeaDatabase.Requests
{
    public class RequestBase<T> : ValidableObject where T : ValidableObject
    {
        [RequiredValidation]
        public T Credentials { get; set; }
    }

    public class AccessRequest : RequestBase<AccessCredentials>
    { }

    //class created to replace original usage of RequestBase<LoginCredentials> which caused improper swagger file
    public class LoginRequest : RequestBase<LoginCredentials>
    {
        public bool KeepMeSignedIn { get; set; }
    } 
    
    public class TokenRequest : RequestBase<TokenCredentials>
    {
        [StringValidation(MinimumLength: 1, Required: false)]
        public string RefreshToken { get; set; }

        [StringValidation(Required: false, MinimumLength: 1)]
        public string ClientId { get; set; }
    }
}