using Credentials;
using IdeaDatabase.Enums;
using InnovationPortalService.HPID;
using Newtonsoft.Json;
using Responses;
using System.Collections.Generic;

namespace InnovationPortalService.Responses
{
    [JsonObject]
    public class GetProfileByTokenResponse : ResponseBase
    {

        [JsonProperty]
        public GetProfileByTokenCredentials Credentials;

        [JsonProperty]
        public ProfileObject CustomerProfileObject;

        public GetProfileByTokenResponse()
        {

        }

        public GetProfileByTokenResponse(GetProfileResponse profileResponse)
        {
            if (profileResponse.Credentials != null)
                Credentials = new GetProfileByTokenCredentials(profileResponse.Credentials);
            if (profileResponse.CustomerProfileObject != null)
                CustomerProfileObject = new ProfileObject(profileResponse.CustomerProfileObject);
        }
    }

    [JsonObject]
    public class GetProfileByTokenCredentials
    {
        private AccessCredentials accessCredentials;

        public GetProfileByTokenCredentials(AccessCredentials accessCredentials)
        {
            this.accessCredentials = accessCredentials;
        }

        [JsonProperty]
        public int UserID { get { return accessCredentials.UserID; } }

        [JsonProperty]
        public string SessionToken { get { return accessCredentials.SessionToken; } }
    }

    [JsonObject]
    public class ProfileObject : ResponseBase
    {
        private CustomerProfile customerProfile;
        public ProfileObject(CustomerProfile customerProfile)
        {
            if (customerProfile != null)
                this.customerProfile = customerProfile;
        }
        [JsonProperty]
        public string Id { get { return customerProfile?.Id; } }
        [JsonProperty]
        public string FirstName { get { return customerProfile?.FirstName; } }
        [JsonProperty]
        public string LastName { get { return customerProfile?.LastName; } }
        [JsonProperty]
        public string Email { get { return customerProfile?.EmailAddress; } }
        [JsonProperty]
        public string City { get { return customerProfile?.City; } }
        [JsonProperty]
        public bool EmailOffers
        {
            get
            {
                if (customerProfile == null)
                    return false;

                if (customerProfile.EmailConsent.Equals(EmailConsentType.Y.ToString()))
                    return true;
                else
                    return false;
            }
        }
        [JsonProperty]
        public string PrimaryUse { get { return customerProfile?.PrimaryUse; } }
        [JsonProperty]
        public string Country { get { return customerProfile?.Country; } }
        [JsonProperty]
        public string Language { get { return customerProfile?.Language; } }
        [JsonProperty]
        public string Company { get { return customerProfile?.CompanyName; } }

        [JsonProperty]
        public bool? ActiveHealth { get { return customerProfile?.ActiveHealth; } }
        [JsonProperty]
        public string UserName { get { return customerProfile?.UserName; } }
        [JsonProperty]
        public string DisplayName { get { return customerProfile?.DisplayName; } }
        [JsonProperty]
        public string Gender { get { return customerProfile?.Gender; } }

        public List<Address> Addresses { get { return customerProfile?.Addresses; } }
        public List<PhoneNumber> PhoneNumbers { get { return customerProfile?.PhoneNumbers; } }

        [JsonProperty]
        public bool SmartFriend { get { return customerProfile?.SmartFriend ?? false; } }

        [JsonProperty]
        public string TimeOut { get { return customerProfile?.TimeOut; } }
    }
}