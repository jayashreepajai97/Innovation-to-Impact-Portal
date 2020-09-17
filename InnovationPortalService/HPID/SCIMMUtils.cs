using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace InnovationPortalService.HPID
{
    public class ScimUpdateHelper : ISCIMMUtils
    {

        private class ScimOperation
        {
            public string op { get; set; }
            public string path { get; set; }
            public object value { get; set; }
        }

        private class ScimRemoval
        {
            public string op { get; set; }
            public string path { get; set; }
        }

        private class ScimOperations
        {
            public string[] schemas = { "urn:ietf:params:scim:api:messages:2.0:PatchOp" };
            public List<object> Operations;

            public ScimOperations()
            {
                Operations = new List<object>();
            }
        }

        //allowed operations' types
        private enum opTypes
        {
            add,
            replace,
            remove
        }

        private Dictionary<string, Tuple<opTypes, object>> operations;
        private Dictionary<string, string> removals;

        public int AddOperationsCount
        {
            get
            {
                return operations.Values.Where(x => x.Item1 == opTypes.add).Count();
            }
        }

        public int ReplaceOperationsCount
        {
            get
            {
                return operations.Values.Where(x => x.Item1 == opTypes.replace).Count();
            }
        }

        public int RemoveOperationsCount
        {
            get
            {
                return removals.Count;
            }
        }

        public ScimUpdateHelper()
        {
            operations = new Dictionary<string, Tuple<opTypes, object>>();
            removals = new Dictionary<string, string>();
        }

        public void AddAttribute<T>(string path, T value)
        {
            operations[path] = new Tuple<opTypes, object>(opTypes.add, (object)value);
        }

        public void ReplaceAttribute<T>(string path, T value)
        {
            operations[path] = new Tuple<opTypes, object>(opTypes.replace, (object)value);
        }

        public void RemoveAttribute(string path)
        {
            removals[path] = opTypes.remove.ToString();
        }

        public void RemoveAttribute<T>(string path, T value)
        {
            operations[path] = new Tuple<opTypes, object>(opTypes.remove, (object)value);
        }

        public bool IsHPIDCustomerProfileUpdateRequired(CustomerData custData, HPIDCustomerProfile source, bool SupportUpdateToProfileDetails)
        {
            if (source == null || custData == null)
            {
                return false;
            }

            //Check for changes in all HPID fields
            {
                SelectOperationForName(source.name, custData.FirstName, custData.LastName);
                SelectOperationForEmail(source.emails, custData.EmailAddress);
                SelectOperationForLocalization(source.locale, custData);
                SelectOperationForCountryResidence(source.countryResidence, custData.Country);
                SelectOperationForCompanyName(source.hpp_organizationName, custData.CompanyName);

                //Remove possibility of updating/ removing addresses(Addresses node)
                //Remove possibility of updating/ removing phones(PhoneNumbers node)
                if (SupportUpdateToProfileDetails)
                {
                    SelectOperationForDisplayName(source.displayName, custData.DisplayName);
                    SelectOperationForGender(source.gender, custData.Gender);
                }

                //Support old profile update and new Profile update.
                SelectOperationForAddress(source.addresses, custData);
            }

            return (operations.Count > 0 || removals.Count > 0);
        }

        private void SelectOperationForName(Name origName, string newFirstName, string newLastName)
        {
            if (origName == null)
            {
                if (string.IsNullOrEmpty(newFirstName) && string.IsNullOrEmpty(newLastName))
                    return;

                this.AddAttribute("name", new Name() { familyName = newLastName, givenName = newFirstName });
                return;
            }


            // NOTICE: upper/lower case letters change not affects change in profile 
            var _newFName = GetToUpperText(newFirstName);
            var _newLName = GetToUpperText(newLastName);
            var _oldFName = GetToUpperText(origName.givenName);
            var _oldLName = GetToUpperText(origName.familyName);

            if (!_newFName.Equals(_oldFName))
            {
                this.ReplaceAttribute("name.givenName", newFirstName);
            }

            if (!_newLName.Equals(_oldLName))
            {
                this.ReplaceAttribute("name.familyName", newLastName);
            }
        }

        private void SelectOperationForAddress(List<Address> origAddresses, CustomerData newAddress)
        {
            if (!newAddress.UpdateCity)
                return;

            Address primaryAddress = null;
            if (origAddresses != null)
                primaryAddress = origAddresses.Where(x => x.Primary == true).FirstOrDefault();

            if (primaryAddress == null)
            {
                if (string.IsNullOrEmpty(newAddress.City))
                    return;

                List<Address> adress = new List<Address>();
                adress.Add(new Address(newAddress) { Locality = newAddress.City, Country = newAddress.Country });
                this.AddAttribute("addresses", adress);
                return;
            }

            if (!newAddress.City.Equals(primaryAddress.Locality))
            {
                this.ReplaceAttribute("addresses[primary eq true].locality", newAddress.City);
            }

            if (!newAddress.Country.Equals(primaryAddress.Country))
            {
                this.ReplaceAttribute("addresses[primary eq true].country", newAddress.Country);
            }
        }

        private void SelectOperationForAddress(List<Address> origAddresses, List<Address> newAddress, CustomerData custData)
        {
            List<Address> addressList = new List<Address>();
            List<string> existIds = new List<string>();

            if (newAddress != null && newAddress.Count > 0)
            {
                //2. Loop all new address.
                foreach (Address address in newAddress)
                {
                    //3. Check if already saved address exist for addressId.
                    Address origAddress = origAddresses?.Where(x => x.Id == address.Id).FirstOrDefault();

                    //4. If there is no address with addressId then add new.
                    if (origAddress == null)
                    {
                        Address a = new Address()
                        {
                            Locality = address.Locality,
                            Country = address.Country,
                            AddressBookAlias = address.AddressBookAlias,
                            District = address.District,
                            FullName = address.FullName,
                            //Id = address.Id,
                            Line1 = address.Line1,
                            Line2 = address.Line2,
                            Line3 = address.Line3,
                            PostalCode = address.PostalCode,
                            Primary = address.Primary,
                            Region = address.Region,
                            Role = address.Role,
                            Type = address.Type
                        };

                        addressList.Add(a);
                        this.AddAttribute("addresses", addressList);
                    }
                    else
                    {
                        existIds.Add(origAddress.Id);
                        //5. If org address exist then update the address.
                        foreach (var property in typeof(Address).GetProperties())
                        {
                            //yield return property;
                            var origPropertyValue = property.GetValue(origAddress);
                            var newPropertyValue = property.GetValue(address);

                            string propertyName = property.Name.First().ToString().ToLower() + property.Name.ToString().Substring(1);

                            if ((origPropertyValue == null && newPropertyValue == null) ||
                                (origPropertyValue != null && newPropertyValue != null && origPropertyValue.ToString() == newPropertyValue.ToString()))
                                continue;

                            if (property.Name.ToString().ToLower() != "id")
                                SelectOperationForField(origPropertyValue, $"addresses[id eq \"{origAddress.Id}\"].{propertyName}", newPropertyValue);
                        }
                    }
                }
            }


            //Delete all and re-add it.
            if (origAddresses != null && origAddresses.Count > 0)
            {
                foreach (Address item in origAddresses.Where(x => (existIds.Count == 0 || !existIds.Contains(x.Id))))
                {
                    this.RemoveAttribute($"addresses[id eq \"{item.Id}\"]");
                }
            }
        }

        private void SelectOperationForPhoneNumber(List<PhoneNumber> origPhoneNumbers, List<PhoneNumber> newPhoneNumber, CustomerData custData)
        {
            List<PhoneNumber> phoneNumberList = new List<PhoneNumber>();
            List<string> existIds = new List<string>();

            if (newPhoneNumber != null && newPhoneNumber.Count > 0)
            {
                //2. Loop all new phonenumber.
                foreach (PhoneNumber phoneNumber in newPhoneNumber)
                {
                    //3. Check if already saved phonenumber exist for given Id.
                    PhoneNumber origPhoneNumber = origPhoneNumbers?.Where(x => x.Id == phoneNumber.Id).FirstOrDefault();

                    //4. If there is no phonenumber with Id then add new.
                    if (origPhoneNumber == null)
                    {
                        PhoneNumber phNo = new PhoneNumber()
                        {
                            Primary = phoneNumber.Primary,
                            AreaCode = phoneNumber.AreaCode,
                            CountryCode = phoneNumber.CountryCode,
                            Type = phoneNumber.Type,
                            Number = phoneNumber.Number,
                            //Below property is use for ignore Serialization for AccountRecovery and Verified.
                            //We can't use AccountRecovery, Verified fields as this fields are private in HPAPI.
                            //Id field is auto generated by HPAPI.
                            SerializeForPriviteFields = false
                        };

                        phoneNumberList.Add(phNo);
                        this.AddAttribute("phoneNumbers", phoneNumberList);
                    }
                    else
                    {
                        existIds.Add(origPhoneNumber.Id);
                        //5. If org phonenumber exist then update it.
                        foreach (var property in typeof(PhoneNumber).GetProperties())
                        {
                            var origPropertyValue = property.GetValue(origPhoneNumber);
                            var newPropertyValue = property.GetValue(phoneNumber);

                            string propertyName = property.Name.First().ToString().ToLower() + property.Name.ToString().Substring(1);

                            if ((origPropertyValue == null && newPropertyValue == null) || 
                                (origPropertyValue != null && newPropertyValue != null && origPropertyValue.ToString() == newPropertyValue.ToString()))
                                continue;
                            
                            //Ignore below fields as this fields cant be used in request.
                            string[] IgnoreProperty = new[] { "id", "accountrecovery", "verified" };

                            if (!IgnoreProperty.Contains(propertyName.ToLower()))
                                SelectOperationForField(origPropertyValue, $"phoneNumbers[id eq \"{phoneNumber.Id}\"].{propertyName}", newPropertyValue);
                        }
                    }
                }
            }

            //Delete all and re-add it.
            if (origPhoneNumbers != null && origPhoneNumbers.Count > 0)
            {
                foreach (PhoneNumber item in origPhoneNumbers.Where(x => (existIds.Count == 0 || !existIds.Contains(x.Id))))
                {
                    this.RemoveAttribute($"phoneNumbers[id eq \"{item.Id}\"]");
                }
            }
        }

        private void SelectOperationForEmail(List<Email> origEmails, string newEmailAddress)
        {
            if (origEmails == null)
                return;

            if (string.IsNullOrEmpty(newEmailAddress))
                return;

            Email primaryEmail = origEmails.Where(x => x.primary == true).FirstOrDefault();
            if (primaryEmail == null)
            {
                Email eml = new Email() { value = newEmailAddress };
                this.AddAttribute("emails", eml);
                return;
            }

            if (!newEmailAddress.Equals(primaryEmail.value))
            {
                this.ReplaceAttribute("emails[primary eq true].value", newEmailAddress);
            }
        }

        private void SelectOperationForLocalization(string origLocale, CustomerData newdata)
        {
            if (string.IsNullOrEmpty(newdata.Language) || string.IsNullOrEmpty(newdata.Country))
                return;


            string locale = $"{newdata.Language.ToLower()}_{newdata.Country.ToUpper()}";

            if (string.IsNullOrEmpty(origLocale))
            {
                this.AddAttribute("locale", locale);
                return;
            }


            if (!origLocale.Equals(locale))
            {
                this.ReplaceAttribute("locale", locale);
            }
        }

        private void SelectOperationForCountryResidence(string origCountryResidence, string newCountry)
        {
            if (string.IsNullOrEmpty(newCountry))
                return;


            string country = newCountry.ToUpper();
            if (string.IsNullOrEmpty(origCountryResidence))
            {
                this.AddAttribute("countryResidence", country);
                return;
            }


            if (!origCountryResidence.Equals(country))
            {
                this.ReplaceAttribute("countryResidence", country);
            }
        }

        private void SelectOperationForGender(string origGender, string newGender)
        {
            if (string.IsNullOrEmpty(newGender))
                return;
            
            string genderUpperCase = newGender.ToUpper();
            if (string.IsNullOrEmpty(origGender))
            {
                this.AddAttribute("gender", genderUpperCase);
                return;
            }
            
            if (!origGender.Equals(genderUpperCase))
            {
                this.ReplaceAttribute("gender", genderUpperCase);
            }
        }

        private void SelectOperationForDisplayName(string origDisplayName, string newDisplayName)
        {
            if (string.IsNullOrEmpty(origDisplayName))
            {
                if (string.IsNullOrEmpty(newDisplayName))
                    return;

                this.AddAttribute("displayName", newDisplayName);
                return;
            }

            if (string.IsNullOrEmpty(newDisplayName))
            {
                // If Display Name not provided in the input payload 
                // or set there as null, retain previous values
                return;
            }

            if (!origDisplayName.Equals(newDisplayName))
            {
                this.ReplaceAttribute("displayName", newDisplayName);
            }
        }

        private void SelectOperationForCompanyName(string origOrganizationName, string newCompanyName)
        {
            if (string.IsNullOrEmpty(origOrganizationName))
            {
                if (string.IsNullOrEmpty(newCompanyName))
                    return;

                this.AddAttribute("hpp_organizationName", newCompanyName);
                return;
            }


            if (string.IsNullOrEmpty(newCompanyName))
            {
                this.RemoveAttribute("hpp_organizationName");
                return;
            }


            if (!origOrganizationName.Equals(newCompanyName))
            {
                this.ReplaceAttribute("hpp_organizationName", newCompanyName);
            }
        }

        private void SelectOperationForField<T>(T origField, string fieldName, T newField)
        {
            //1. If field doesn't exist in request, then it will be null and remove this field.
            if (newField == null)
            {
                this.RemoveAttribute(fieldName);
                return;
            }

            this.ReplaceAttribute(fieldName, newField);
        }

        private string GetToUpperText(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";

            return source.ToUpper();
        }

        public string GetJson()
        {
            ScimOperations scimOps = new ScimOperations();
            foreach (KeyValuePair<string, Tuple<opTypes, object>> pair in operations)
            {
                scimOps.Operations.Add(new ScimOperation() { op = pair.Value.Item1.ToString(), path = pair.Key, value = pair.Value.Item2 });
            }
            foreach (KeyValuePair<string, string> pair in removals)
            {
                scimOps.Operations.Add(new ScimRemoval() { op = pair.Value, path = pair.Key });
            }
            return JsonConvert.SerializeObject(scimOps, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() }).Replace("\"operations\"", "\"Operations\"");
        }
    }
}