using InnovationPortalService.Contract.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace InnovationPortalService.Utils
{
    public class AwsWebRequest : IAwsWebRequest
    {
        private readonly string contentType;
        private readonly string host;
        private readonly string accessKey;
        private readonly string secretKey;
        private readonly string regionName;
        private readonly string serviceName;
        private readonly string algorithm;
        private readonly string signedHeaders;

        public AwsWebRequest(string Host, string AccessKey, string SecretKey, string RegionName, string ServiceName, string ContentType, string Algorithm, string SignedHeaders)
        {
            contentType = ContentType;
            host = Host;
            accessKey = AccessKey;
            secretKey = SecretKey;
            regionName = RegionName;
            serviceName = ServiceName;
            algorithm = Algorithm;
            signedHeaders = SignedHeaders;
        }
        public WebRequest RequestPost(string canonicalUri, string canonicalQueriString, string jsonString)
        {
            return WebRequestPost(canonicalUri, canonicalQueriString, jsonString);
        }

        private WebRequest WebRequestPost(string canonicalUri, string canonicalQueriString, string jsonString)
        {
            string hashedRequestPayload = CreateRequestPayload(jsonString);

            string authorization = Sign(hashedRequestPayload, "POST", canonicalUri, canonicalQueriString);
            string requestDate = DateTime.UtcNow.ToString("yyyyMMddTHHmmss") + "Z";

            WebRequest webRequest = WebRequest.Create("https://" + host + canonicalUri);

            webRequest.Timeout = 20000;
            webRequest.Method = "POST";
            webRequest.ContentType = contentType;
            webRequest.Headers.Add("X-Amz-date", requestDate);
            webRequest.Headers.Add("Authorization", authorization);
            webRequest.Headers.Add("x-amz-content-sha256", hashedRequestPayload);
            webRequest.ContentLength = jsonString.Length;

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(jsonString);

            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);


            return webRequest;
        }

        private string CreateRequestPayload(string jsonString)
        {
            //Here should be JSON object of the model we are sending with POST request
            //var jsonToSerialize = new { Data = String.Empty };

            //We parse empty string to the serializer if we are makeing GET request
            //string requestPayload = new JavaScriptSerializer().Serialize(jsonToSerialize);
            string hashedRequestPayload = HexEncode(Hash(ToBytes(jsonString)));

            return hashedRequestPayload;
        }

        private string Sign(string hashedRequestPayload, string requestMethod, string canonicalUri, string canonicalQueryString)
        {
            var currentDateTime = DateTime.UtcNow;

            var dateStamp = currentDateTime.ToString("yyyyMMdd");
            var requestDate = currentDateTime.ToString("yyyyMMddTHHmmss") + "Z";
            var credentialScope = string.Format("{0}/{1}/{2}/aws4_request", dateStamp, regionName, serviceName);

            var headers = new SortedDictionary<string, string> {
            { "content-type",contentType },
            { "host", host  },
            { "x-amz-date", requestDate }
        };

            string canonicalHeaders = string.Join("\n", headers.Select(x => x.Key.ToLowerInvariant() + ":" + x.Value.Trim())) + "\n";

            // Task 1: Create a Canonical Request For Signature Version 4
            string canonicalRequest = requestMethod + "\n" + canonicalUri + "\n" + canonicalQueryString + "\n" + canonicalHeaders + "\n" + signedHeaders + "\n" + hashedRequestPayload;
            string hashedCanonicalRequest = HexEncode(Hash(ToBytes(canonicalRequest)));

            // Task 2: Create a String to Sign for Signature Version 4
            string stringToSign = algorithm + "\n" + requestDate + "\n" + credentialScope + "\n" + hashedCanonicalRequest;

            // Task 3: Calculate the AWS Signature Version 4
            byte[] signingKey = GetSignatureKey(secretKey, dateStamp, regionName, serviceName);
            string signature = HexEncode(HmacSha256(stringToSign, signingKey));

            // Task 4: Prepare a signed request
            // Authorization: algorithm Credential=access key ID/credential scope, SignedHeadaers=SignedHeaders, Signature=signature

            string authorization = string.Format("{0} Credential={1}/{2}/{3}/{4}/aws4_request, SignedHeaders={5}, Signature={6}",
            algorithm, accessKey, dateStamp, regionName, serviceName, signedHeaders, signature);

            return authorization;
        }

        private byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
        {
            byte[] kDate = HmacSha256(dateStamp, ToBytes("AWS4" + key));
            byte[] kRegion = HmacSha256(regionName, kDate);
            byte[] kService = HmacSha256(serviceName, kRegion);
            return HmacSha256("aws4_request", kService);
        }

        private byte[] ToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str.ToCharArray());
        }

        private string HexEncode(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }

        private byte[] Hash(byte[] bytes)
        {
            return SHA256.Create().ComputeHash(bytes);
        }

        private byte[] HmacSha256(string data, byte[] key)
        {
            return new HMACSHA256(key).ComputeHash(ToBytes(data));
        }

        public string WebResponse(WebRequest request)
        {
            using (WebResponse response = request.GetResponse())
            {
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                return responseReader.ReadToEnd();
            }
        }
    }
}