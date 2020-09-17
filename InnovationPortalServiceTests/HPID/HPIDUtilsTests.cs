using Responses;
using System.Linq;
using IdeaDatabase.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InnovationPortalService.HPID;

namespace InnovationPortalServiceTests.HPID.Tests
{

    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HPIDUtilsTests
    {
        TokenDetails sessionToken;

        [TestInitialize]
        public void Init()
        {
            sessionToken = new TokenDetails();
        }
        [TestMethod()]
        public void GetHPIDSessionTokenTest_InvalidAuthCredentials()
        {
            HPIDUtils hpu = new HPIDUtils();

            ResponseBase response = new ResponseBase();
            sessionToken = hpu.GetHPIDSessionToken(1, null, null, response, null);

            Assert.IsNull(sessionToken);
            Assert.IsTrue(response.ErrorList.Where(x => x.ReturnCode.Equals("InvalidCredentials")).Count() == 1);
        }

        [TestMethod()]
        public void GetHPIDSessionTokenTest_InvalidLoginCredentials()
        {
            HPIDUtils hpu = new HPIDUtils();

            ResponseBase response = new ResponseBase();
            sessionToken = hpu.GetHPIDSessionToken(0, null, null, response,null);

            Assert.IsNull(sessionToken);
            Assert.IsTrue(response.ErrorList.Where(x => x.ReturnCode.Equals("InvalidCredentials")).Count() == 1);
        }

        //[TestMethod()]
        //public void getTokenTest_HPIDTimeoutError()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (HttpClient h, String s, HttpContent c) => { throw new TimeoutException("exception"); };

        //        ResponseBase response = new ResponseBase();
        //        sessionToken = hpidUtils.GetHPIDSessionToken(2, null, null, response, null);

        //        Assert.IsNull(sessionToken);
        //        Assert.IsTrue(response.ErrorList.Where(x => x.DebugStatusText.Equals("The service operation timed out")).Count() == 1);
        //    }
        //}

        //[TestMethod()]
        //public void getTokenTest_HPIDInternalError()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (HttpClient h, String s, HttpContent c) => { throw new Exception("exception"); };

        //        ResponseBase response = new ResponseBase();
        //        sessionToken = hpidUtils.GetHPIDSessionToken(2, null, null, response, null);

        //        Assert.IsNull(sessionToken);
        //        Assert.IsTrue(response.ErrorList.Where(x => x.DebugStatusText.Equals("Remote service internal error")).Count() == 1);
        //    }
        //}

        //[TestMethod()]
        //public void GetHPIDSessionTokenTest_AuthTokenSuccess()
        //{
        //    string jsonContent = "{\"access_token\": \"123456\"}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (HttpClient h, String s, HttpContent c) => { return Task.FromResult(mess); };

        //        ResponseBase response = new ResponseBase();
        //        sessionToken = hpidUtils.GetHPIDSessionToken(1, "accessCode", "redirectURL", response, null);

        //        Assert.IsTrue(response.ErrorList.Count == 0);
        //        Assert.IsTrue(sessionToken.AccessToken.Equals("123456"));
        //    }
        //}

        //[TestMethod()]
        //public void GetHPIDSessionTokenTest_AccountLocked()
        //{
        //    string jsonContent = "{\"error_cause\": \"accountLocked\"}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (HttpClient h, String s, HttpContent c) => { return Task.FromResult(mess); };

        //        ResponseBase response = new ResponseBase();
        //        sessionToken = hpidUtils.GetHPIDSessionToken(1, "accessCode", "redirectURL", response, null);

        //        Assert.IsTrue(response.ErrorList.Contains(Faults.HPIDAccountLocked));
        //    }
        //}

        //[TestMethod()]
        //public void GetHPIDSessionTokenTest_LoginTokenSuccess()
        //{
        //    string jsonContent = "{\"access_token\": \"123456\"}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (HttpClient h, String s, HttpContent c) => { return Task.FromResult(mess); };

        //        ResponseBase response = new ResponseBase();
        //        sessionToken = hpidUtils.GetHPIDSessionToken(0, "userName", "password", response, null);

        //        Assert.IsTrue(response.ErrorList.Count == 0);
        //        Assert.IsTrue(sessionToken.AccessToken.Equals("123456"));
        //    }
        //}

        //[TestMethod()]
        //public void GetIdsAndProfileTest_HPIDTimeoutError()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { throw new TimeoutException(""); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        Assert.IsFalse(hpidUtils.GetIdsAndProfile(ids, null, response));
        //        Assert.IsTrue(response.ErrorList.Where(x => x.DebugStatusText.Equals("The service operation timed out")).Count() == 1);
        //    }
        //}

        //[TestMethod()]
        //public void GetIdsAndProfileTest_HPIDInternalError()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { throw new Exception(""); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        Assert.IsFalse(hpidUtils.GetIdsAndProfile(ids, null, response));
        //        Assert.IsTrue(response.ErrorList.Where(x => x.DebugStatusText.Equals("Remote service internal error")).Count() == 1);
        //    }
        //}

        //[TestMethod()]
        //public void GetIdsAndProfileTest_SuccessWithData()
        //{
        //    string jsonContent = "{\"id\": \"123456\", \"hpp_id\": \"789012\", \"locale\": \"locale\", \"name\": {\"givenName\" : \"tekst1\", \"familyName\" : \"tekst2\"}, \"emails\": [{\"value\" : \"tekst3\"}],\"addresses\": [{\"primary\" : \"true\",\"country\" : \"tekst5\"}]}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        Assert.IsTrue(hpidUtils.GetIdsAndProfile(ids, "sessionToken", response));
        //        Assert.IsTrue(response.ErrorList.Count == 0);
        //        Assert.IsTrue(ids.HPIDid.Equals("123456"));
        //    }
        //}

        //[TestMethod()]
        //public void GetIdsAndProfileTest_Success_MappingZHLanguageCode()
        //{
        //    string jsonContent = "{\"id\": \"123456\", \"hpp_id\": \"789012\", \"locale\": \"12_XX\", \"name\": {\"givenName\" : \"tekst1\", \"familyName\" : \"tekst2\"}, \"emails\": [{\"value\" : \"tekst3\"}],\"addresses\": [{\"primary\" : \"true\",\"country\" : \"tekst5\"}]}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        Assert.IsTrue(hpidUtils.GetIdsAndProfile(ids, "sessionToken", response));
        //        Assert.IsTrue(response.ErrorList.Count == 0);
        //        Assert.IsTrue(ids.HPIDid.Equals("123456"));
        //        Assert.IsTrue(response.CustomerProfileObject.Language.Equals("zh"));
        //    }
        //}

        //[TestMethod()]
        //public void GetIdsAndProfileTest_Success_MappingZFLanguageCode()
        //{
        //    string jsonContent = "{\"id\": \"123456\", \"hpp_id\": \"789012\", \"locale\": \"13_XX\", \"name\": {\"givenName\" : \"tekst1\", \"familyName\" : \"tekst2\"}, \"emails\": [{\"value\" : \"tekst3\"}],\"addresses\": [{\"primary\" : \"true\",\"country\" : \"tekst5\"}]}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        Assert.IsTrue(hpidUtils.GetIdsAndProfile(ids, "sessionToken", response));
        //        Assert.IsTrue(response.ErrorList.Count == 0);
        //        Assert.IsTrue(ids.HPIDid.Equals("123456"));
        //        Assert.IsTrue(response.CustomerProfileObject.Language.Equals("zf"));
        //    }
        //}

        //[TestMethod()]
        //public void GetIdsAndProfileTest_Success()
        //{
        //    string jsonContent = "{\"id\": \"123456\", \"hpp_id\": \"789012\"}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        Assert.IsTrue(hpidUtils.GetIdsAndProfile(ids, "sessionToken", response));
        //        Assert.IsTrue(response.ErrorList.Count == 0);
        //        Assert.IsTrue(ids.HPIDid.Equals("123456"));
        //    }
        //}

        //[TestMethod()]
        //public void GetIdsAndProfileTest_IDNotFoundInResponse()
        //{
        //    string jsonContent = "{\"hpp_id\": \"789012\"}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        Assert.IsFalse(hpidUtils.GetIdsAndProfile(ids, "sessionToken", response));
        //        Assert.IsTrue(response.ErrorList.Where(x => x.DebugStatusText.Equals("Remote service returned invalid response")).Count() == 1);
        //    }
        //}

        //[TestMethod()]
        //public void GetIdsAndProfileTest_InvalidToken()
        //{
        //    string jsonContent = "{\"scimType\": \"invalid_token\",\"schemas\": [\"urn:ietf:params:scim:api:messages:2.0:Error\"]}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        Assert.IsFalse(hpidUtils.GetIdsAndProfile(ids, "sessionToken", response));
        //        Assert.IsTrue(response.ErrorList.Where(x => x.DebugStatusText.Equals("Access token is expired or otherwise invalid")).Count() == 1);
        //    }
        //}

        //[TestMethod()]
        //public void CreateHPIDNewCustomerProfile_HPIDTimeoutError()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (HttpClient h, String s, HttpContent c) => { throw new TimeoutException("exception"); };

        //        ResponseBase response = new ResponseBase();
        //        Assert.IsNull(hpidUtils.CreateHPIDNewCustomerProfile("sessionToken", "requestJson", response));
        //        Assert.IsTrue(response.ErrorList.Where(x => x.DebugStatusText.Equals("The service operation timed out")).Count() == 1);
        //    }
        //}
 
         

        //[TestMethod()]
        //public void GetProfileTest_GetAddressAndPhoneNumber()
        //{
        //    string jsonContent = "{\"id\":\"5wfigzaezzr5csw9qzyiuay318hh19bb\",\"meta\":{\"resourceType\":\"User\",\"created\":\"2019-08-14T07:05:03.428Z\",\"lastModified\":\"2019-08-14T09:34:02.887Z\",\"version\":\"\\\"44890120771\\\"\",\"location\":\"https://directory.stg.cd.id.hp.com/directory/v1/scim/v2/Users/5wfigzaezzr5csw9qzyiuay318hh19bb\"},\"schemas\":[\"urn:hp:hpid:scim:schemas:1.0:User\"],\"addresses\":[{\"locality\":\"Nagpur\",\"country\":\"CN\",\"primary\":true,\"role\":\"shipping\",\"type\":\"home\",\"line1\":\"yyy\",\"line2\":\"rrr\",\"district\":\"yy\",\"postalCode\":\"492001\",\"region\":\"rrrs\",\"addressBookAlias\":\"yyy\",\"line3\":\"4752156358\",\"fullName\":\"HEabcdefd EC\",\"id\":\"is5n9bogwyxajrnktszdcsdhgnnge66i\"}],\"countryResidence\":\"CN\",\"displayName\":\"jarphoto55\",\"emails\":[{\"value\":\"abcdefd.xyzxsdv@hp.com\",\"accountRecovery\":true,\"primary\":true,\"type\":\"other\",\"verified\":false}],\"enabled\":true,\"extendedMeta\":{\"createdByClient\":\"UyN6Xmls0lo89sEyNTQCGC6KcmI7lA4W\",\"lastModifiedByClient\":\"UyN6Xmls0lo89sEyNTQCGC6KcmI7lA4W\",\"lastModifiedByUser\":\"5wfigzaezzr5csw9qzyiuay318hh19bb\"},\"gender\":\"F\",\"hpp_organizationName\":\"HP Inc\",\"legalZone\":\"GLOBAL\",\"locale\":\"en_CN\",\"name\":{\"familyName\":\"xyzxsdv\",\"givenName\":\"abcdefd\"},\"phoneNumbers\":[{\"primary\":true,\"countryCode\":\"IN\",\"areaCode\":\"492550\",\"number\":\"7411471020\",\"type\":\"other\",\"id\":\"rfqkkywr3e4s819f7c53anh7yx49if7e\",\"accountRecovery\":false,\"verified\":false},{\"primary\":false,\"countryCode\":\"CN\",\"areaCode\":\"333333\",\"number\":\"555555\",\"type\":\"other\",\"id\":\"7pu6e6hgae496yu1st78bkbfe6s3uk9a\",\"accountRecovery\":false,\"verified\":false}],\"type\":\"HUMAN\",\"userName\":\"jar.xyzxsdv55@hp.com\"}";

        //    HPIDCustomerProfile expResult = JsonConvert.DeserializeObject<HPIDCustomerProfile>(jsonContent);
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        HPIDCustomerProfile profile = hpidUtils.GetProfile("sessionToken");
        //        Assert.IsNotNull(profile);
        //        Assert.AreEqual(expResult.gender, profile.gender);
        //        Assert.AreEqual(expResult.displayName, profile.displayName);
        //        Assert.IsTrue(profile.addresses.Count > 0);
        //        Assert.AreEqual(expResult.addresses.Count, profile.addresses.Count);
        //        Assert.AreEqual(expResult.addresses[0].Id, profile.addresses[0].Id);
        //        Assert.IsTrue(profile.phoneNumbers.Count > 0);
        //        Assert.AreEqual(profile.phoneNumbers.Count, 2);
        //        Assert.AreEqual(profile.phoneNumbers[0].Id, "rfqkkywr3e4s819f7c53anh7yx49if7e");
        //    }
        //}

        //[TestMethod()]
        //public void GetProfileTest_GetAddressAndPhoneNumber_NUllGenderDisplayName()
        //{
        //    string jsonContent = "{\"id\":\"5wfigzaezzr5csw9qzyiuay318hh19bb\",\"meta\":{\"resourceType\":\"User\",\"created\":\"2019-08-14T07:05:03.428Z\",\"lastModified\":\"2019-08-14T09:34:02.887Z\",\"version\":\"\\\"44890120771\\\"\",\"location\":\"https://directory.stg.cd.id.hp.com/directory/v1/scim/v2/Users/5wfigzaezzr5csw9qzyiuay318hh19bb\"},\"schemas\":[\"urn:hp:hpid:scim:schemas:1.0:User\"],\"addresses\":[{\"locality\":\"Nagpur\",\"country\":\"CN\",\"primary\":true,\"role\":\"shipping\",\"type\":\"home\",\"line1\":\"yyy\",\"line2\":\"rrr\",\"district\":\"yy\",\"postalCode\":\"492001\",\"region\":\"rrrs\",\"addressBookAlias\":\"yyy\",\"line3\":\"4752156358\",\"fullName\":\"HEabcdefd EC\",\"id\":\"is5n9bogwyxajrnktszdcsdhgnnge66i\"}],\"countryResidence\":\"CN\",\"emails\":[{\"value\":\"abcdefd.xyzxsdv@hp.com\",\"accountRecovery\":true,\"primary\":true,\"type\":\"other\",\"verified\":false}],\"enabled\":true,\"extendedMeta\":{\"createdByClient\":\"UyN6Xmls0lo89sEyNTQCGC6KcmI7lA4W\",\"lastModifiedByClient\":\"UyN6Xmls0lo89sEyNTQCGC6KcmI7lA4W\",\"lastModifiedByUser\":\"5wfigzaezzr5csw9qzyiuay318hh19bb\"},\"gender\": null,\"hpp_organizationName\":\"HP Inc\",\"legalZone\":\"GLOBAL\",\"locale\":\"en_CN\",\"name\":{\"familyName\":\"xyzxsdv\",\"givenName\":\"abcdefd\"},\"phoneNumbers\":[{\"primary\":true,\"countryCode\":\"IN\",\"areaCode\":\"492550\",\"number\":\"7411471020\",\"type\":\"other\",\"id\":\"rfqkkywr3e4s819f7c53anh7yx49if7e\",\"accountRecovery\":false,\"verified\":false},{\"primary\":false,\"countryCode\":\"CN\",\"areaCode\":\"333333\",\"number\":\"555555\",\"type\":\"other\",\"id\":\"7pu6e6hgae496yu1st78bkbfe6s3uk9a\",\"accountRecovery\":false,\"verified\":false}],\"type\":\"HUMAN\",\"userName\":\"jar.xyzxsdv55@hp.com\"}";

        //    HPIDCustomerProfile expResult = JsonConvert.DeserializeObject<HPIDCustomerProfile>(jsonContent);
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        HPIDCustomerProfile profile = hpidUtils.GetProfile("sessionToken");
        //        Assert.IsNotNull(profile);
        //        Assert.IsNull(profile.gender);
        //        Assert.IsNull(profile.displayName);
        //        Assert.IsTrue(profile.addresses.Count > 0);
        //        Assert.AreEqual(expResult.addresses.Count, profile.addresses.Count);
        //        Assert.AreEqual(expResult.addresses[0].Id, profile.addresses[0].Id);
        //        Assert.IsTrue(profile.phoneNumbers.Count > 0);
        //        Assert.AreEqual(profile.phoneNumbers.Count, 2);
        //        Assert.AreEqual(profile.phoneNumbers[0].Id, "rfqkkywr3e4s819f7c53anh7yx49if7e");
        //    }
        //}

        //[TestMethod()]
        //public void GetProfileTest_Success()
        //{
        //    string jsonContent = "{\"id\": \"123456\", \"hpp_id\": \"789012\", \"locale\": \"locale\", \"name\": {\"givenName\" : \"tekst1\", \"familyName\" : \"tekst2\"}, \"emails\": [{\"value\" : \"tekst3\"}],\"addresses\": [{\"primary\" : \"true\",\"country\" : \"tekst5\"}]}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        HPIDCustomerProfile profile = hpidUtils.GetProfile("sessionToken");
        //        Assert.IsNotNull(profile);
        //    }
        //}

        //[TestMethod()]
        //public void GetProfileTest_InvalidProfile()
        //{
        //    string jsonContent = "{\"hpp\": \"123456\", \"hpp_id\": \"789012\", \"locale\": \"locale\", \"name\": {\"givenName\" : \"tekst1\", \"familyName\" : \"tekst2\"}, \"emails\": [{\"value\" : \"tekst3\"}],\"addresses\": [{\"primary\" : \"true\",\"country\" : \"tekst5\"}]}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        HPIDCustomerProfile profile = hpidUtils.GetProfile("sessionToken");
        //        Assert.IsNull(profile);
        //    }
        //}

        //[TestMethod()]
        //public void GetProfileTest_BrokenProfile()
        //{
        //    string jsonContent = "{\"id\": \"123456\", \"hpp_id\": \"789012\", \"locale\": \"locale\",\"emails\": {\"value\" : \"tekst3\"},\"addresses\": {\"primary\" : \"true\",\"country\" : \"tekst5\"}}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        HPIDCustomerProfile profile = hpidUtils.GetProfile("sessionToken");
        //        Assert.IsNotNull(profile);
        //    }
        //}

        //[TestMethod()]
        //public void GetProfileTest_HPIDInternalError()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { throw new Exception(""); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        HPIDCustomerProfile profile = hpidUtils.GetProfile("sessionToken");
        //        Assert.IsNull(profile);
        //    }
        //}

        //[TestMethod()]
        //public void GetProfileTest_InvalidToken()
        //{
        //    string jsonContent = "{\"scimType\": \"invalid_token\",\"schemas\": [\"urn:ietf:params:scim:api:messages:2.0:Error\"]}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.GetAsyncString = (HttpClient h, String s) => { return Task.FromResult(mess); };

        //        CustomerIds ids = new CustomerIds();
        //        GetProfileResponse response = new GetProfileResponse();
        //        HPIDCustomerProfile profile = hpidUtils.GetProfile("sessionToken");
        //        Assert.IsNull(profile);
        //    }
        //}

        //[TestMethod()]
        //public void GetHPIDSessionTokenTest_RefreshToken()
        //{
        //    string jsonContent = "{\"access_token\": \"AccessTokenReturnedByHpid\", \"refresh_token\" : \"RefreshTokenReturnedByHpid\", \"error_cause\":\"\"}";
        //    using (ShimsContext.Create())
        //    {
        //        HPIDUtils hpidUtils = new HPIDUtils();

        //        HttpResponseMessage mess = new HttpResponseMessage();
        //        mess.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        //        System.Net.Http.Fakes.ShimHttpClient.AllInstances.PostAsyncStringHttpContent = (HttpClient h, String s, HttpContent c) => { return Task.FromResult(mess); };

        //        ResponseBase response = new ResponseBase();
        //        sessionToken = hpidUtils.GetHPIDSessionToken((int)TokenScopeType.userRefreshToken, "", "refreshToken", response, null, (int)RefreshTokenLoginType.Credentials);

        //        Assert.IsTrue(response.ErrorList.Count == 0);
        //        Assert.IsTrue(sessionToken.AccessToken.Equals("AccessTokenReturnedByHpid"));
        //        Assert.IsTrue(sessionToken.RefreshToken.Equals("RefreshTokenReturnedByHpid"));
        //        Assert.IsTrue(sessionToken.RefreshTokenType == (int)RefreshTokenLoginType.Credentials);
        //    }
        //}
    }
}