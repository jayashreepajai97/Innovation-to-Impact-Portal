using Newtonsoft.Json;
using IdeaDatabase.Utils;
using IdeaDatabase.Enums;
using IdeaDatabase.Validation;
using System.Collections.Generic;

namespace InnovationPortalService.HPID
{
    public class HPIDCustomerProfile
    {
        public string domain { get; set; }
        public bool enabled { get; set; }
        public string identityProvider { get; set; }
        public string legalZone { get; set; }

        public string[] schemas { get; set; }
        public string countryResidence { get; set; }
        public string locale { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string displayName { get; set; }
        public string gender { get; set; }
        public List<Email> emails { get; set; }
        public Name name { get; set; }
        public List<Address> addresses { get; set; }

        [JsonIgnore]
        public List<PhoneNumber> phoneNumbers { get; set; }

        [JsonIgnore]
        public string hpp_organizationName { get; set; }

        public HPIDCustomerProfile()
        {
        }

        public HPIDCustomerProfile(CustomerProfile customer)
        {
            domain = "hpid";
            enabled = true;
            identityProvider = "hpid";
            legalZone = "GLOBAL";

            // only to create new user profile at HPID !!!!!!!!!
            schemas = new string[] { "urn:hp:hpid:scim:schemas:1.0:User" };

            countryResidence = customer.Country;
            locale = $"{customer.Language.ToLower()}_{customer.Country}";
            userName = customer.UserName;
            password = customer.Password;
            gender = customer.Gender;
            displayName = customer.DisplayName;
            hpp_organizationName = customer.CompanyName;

            emails = new List<Email>() { new Email() { value = customer.EmailAddress } };
            name = new Name() { familyName = customer.LastName, givenName = customer.FirstName };

            addresses = new List<Address>();
            if (!string.IsNullOrEmpty(customer.City))
                addresses.Add(new Address(customer) { Locality = customer.City, Country = customer.Country });

            phoneNumbers = new List<PhoneNumber>();
            if (!string.IsNullOrEmpty(customer.City))
                phoneNumbers.Add(new PhoneNumber(customer) { CountryCode = customer.Country });
        }
    }

    public class Email
    {
        public string value { get; set; }
        public bool accountRecovery { get; set; }
        public bool primary { get; set; }
        public string type { get; set; }
        public bool verified { get; set; }

        public Email()
        {
            accountRecovery = true;
            primary = true;
            type = "other";
            verified = true;
        }
    }

    public class Name
    {
        public string familyName { get; set; }
        public string givenName { get; set; }
    }

    public class Address : ValidableObject
    {
        public string Locality { get; set; }
        public string Country { get; set; }
        public bool? Primary { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public string AddressBookAlias { get; set; }
        public string Line3 { get; set; }
        public string FullName { get; set; }

        public Address()
        {
        }

        public Address(CustomerData customer)
        {
            Primary = true;
            Role = "other";
            Type = AddressType.other.ToString();

            if (string.IsNullOrEmpty(customer.PrimaryUse))
                return;

            if (customer.PrimaryUse.Equals(PrimaryUseType.Item002.ToString()))
                Type = AddressType.home.ToString();
            else if (EnumUtils.Validate(typeof(PrimaryUseType), customer.PrimaryUse))
                Type = AddressType.business.ToString();
        }
    }

    public class PhoneNumber
    {
        public bool? Primary { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public bool? AccountRecovery { get; set; }
        public bool? Verified { get; set; }
        [JsonIgnore]
        public bool SerializeForPriviteFields { get; set; } = true;

        public PhoneNumber()
        {
        }

        public PhoneNumber(CustomerData customer)
        {
            Primary = true;
            Type = AddressType.other.ToString();
            
            if (string.IsNullOrEmpty(customer.PrimaryUse))
                return;

            if (customer.PrimaryUse.Equals(PrimaryUseType.Item002.ToString()))
                Type = AddressType.home.ToString();
            else if (EnumUtils.Validate(typeof(PrimaryUseType), customer.PrimaryUse))
                Type = AddressType.business.ToString();
        }
        public bool ShouldSerializeAccountRecovery()
        {
            return SerializeForPriviteFields;
        }
        public bool ShouldSerializeVerified()
        {
            return SerializeForPriviteFields;
        }
    }

    public enum AddressType
    {
        other,
        home,
        business
    }
}