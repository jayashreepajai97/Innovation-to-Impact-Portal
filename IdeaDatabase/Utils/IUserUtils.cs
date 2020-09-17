using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using Responses;

namespace IdeaDatabase.Utils
{

    public class RequestFindOrInsertHPIDProfile {
        public string Locale { get; set; }
        public string CompanyName { get; set; }

        public bool? ActiveHealth { get; set; }
        public string HPIDprofileId { get; set; }
        public string HPPprofileId { get; set; }
        public string clientId { get; set; }
        public string EmailAddrees { get; set; }
        public APIMethods apiRetainOldValues  { get; set; }
        public TokenDetails tokenDetails { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public interface IUserUtils
    {
        User FindOrInsertProfile(ResponseBase response, string ProfileId);
        void SaveProfile(ResponseBase response, User profile);
        User FindOrInsertHPIDProfile(ResponseBase response, string HPIDprofileId, string HPPprofileId);
        User FindOrInsertHPIDProfile(ResponseBase response, RequestFindOrInsertHPIDProfile requestFindOrInsertHPIDProfile, out bool IsNewCustomer);
        void SaveHPIDProfile(ResponseBase response, User profile);
        User GetRefreshToken(string callerId, TokenDetails accessToken);
        string GetUserEmail(int UserId);
    }
}
