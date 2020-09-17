using IdeaDatabase.Enums;
using IdeaDatabase.Requests;
using InnovationPortalService.Requests;
using SettingsRepository;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace InnovationPortalService.Utils
{
    public class InMemoryMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private static string DefaultDirectoryName = string.Empty;
        private static bool IsS3Enable { get; } = SettingRepository.Get<bool>("IsS3Enabled");
        private MimeRequestType mimeRequestType;

        private static readonly string AWSIdeaAttachmentFolder = SettingRepository.Get<string>("AWSIdeaAttachmentFolder");
        private static readonly string AWSIdeaIPFolder = SettingRepository.Get<string>("AWSIdeaIPFolder");
        private static readonly string LocalIdeaFolder = SettingRepository.Get<string>("LocalIdeaFolder");
 

        private string DirectoryPath { get; set; }



        static InMemoryMultipartFormDataStreamProvider()
        {
            DefaultDirectoryName = AWSIdeaAttachmentFolder;
        }

        public InMemoryMultipartFormDataStreamProvider(MimeRequestType mimeRequest) : base(DefaultDirectoryName)
        {
            mimeRequestType = mimeRequest;
            switch (mimeRequestType)
            {
                case MimeRequestType.Idea:
                    DefaultDirectoryName = IsS3Enable ? AWSIdeaAttachmentFolder : LocalIdeaFolder;
                    break;
                case MimeRequestType.IntelectualProperty:
                    DefaultDirectoryName = IsS3Enable ? AWSIdeaAttachmentFolder : LocalIdeaFolder; 
                    break;
                case MimeRequestType.UpdateIntellectualProperty:
                    DefaultDirectoryName = IsS3Enable ? AWSIdeaAttachmentFolder : LocalIdeaFolder;
                    break;
                default:
                    break;
            }
        }


        // Set of indexes of which HttpContents we designate as form data  
        private Collection<bool> _isFormData = new Collection<bool>();

       
        public int attachFileCount { get; set; }

        /// <summary>
        /// bind the multipart/formdata request to object
        /// </summary>
        public RestAPIIdeaSupportingRequest restAPIIdeaSupportingRequest { get; } = new RestAPIIdeaSupportingRequest();

        public RestAPISubmitIntellectualRequest SubmitIntellectualrequest { get; } = new RestAPISubmitIntellectualRequest();

        public RestAPIUpdateIntellectualRequest UpdateIntellectualRequest { get; } = new RestAPIUpdateIntellectualRequest();

        public string DefaultPath { get; } = DefaultDirectoryName;



        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            // For form data, Content-Disposition header is a requirement  
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                // We will post process this as form data  
                _isFormData.Add(string.IsNullOrEmpty(contentDisposition.FileName));

                return new MemoryStream();
            }

            // If no Content-Disposition header was present.  
            throw new InvalidOperationException(string.Format("Did not find required '{0}' header field in MIME multipart body part..", "Content-Disposition"));
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
        public static string GetLocalFileIDName(HttpContentHeaders headers)
        {
            return UnquoteToken(headers.ContentDisposition.Name);
        }

        /// <summary>  
        /// Read the non-file contents as form data.  
        /// </summary>  
        /// <returns></returns>  
        public override async Task ExecutePostProcessingAsync()
        {

            // Find instances of non-file HttpContents and read them asynchronously  
            // to get the string content and then add that as form data  
            for (int index = 0; index < Contents.Count; index++)
            {
                if (_isFormData[index])
                {
                    HttpContent formContent = Contents[index];
                    // Extract name from Content-Disposition header. We know from earlier that the header is present.  
                    ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                    string formFieldName = UnquoteToken(contentDisposition.Name) ?? String.Empty;
                    // Read the contents as string data and add to form data  
                    string formFieldValue = await formContent.ReadAsStringAsync();
                    //FormData.Add(formFieldName, formFieldValue);

                    MapPropertyInfo(formFieldName, formFieldValue);

                }
                else
                {
                    await processFilesAsync(Contents[index]);
                }
            }

        }

        private void MapPropertyInfo(string formFieldName, string formFieldValue)
        {

            switch (mimeRequestType)
            {
                case MimeRequestType.Idea:
                    MapNameValueCollection(formFieldName, formFieldValue, restAPIIdeaSupportingRequest);
                    break;
                case MimeRequestType.IntelectualProperty:
                    MapNameValueCollection(formFieldName, formFieldValue, SubmitIntellectualrequest);
                    break;
                case MimeRequestType.UpdateIntellectualProperty:
                    MapNameValueCollection(formFieldName, formFieldValue, UpdateIntellectualRequest);
                    break;
                default:
                    break;
            }
        }

        private async Task processFilesAsync(HttpContent httpContent)
        {
            try
            {
                Stream inputs = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                if (inputs.Length != 0)
                {
                    attachFileCount++;
                    IdeaAttachmentRequest ideaAttachmentRequest = AttachmentRequest(httpContent, inputs);
                    if (mimeRequestType.CompareTo(MimeRequestType.Idea) == 0)
                    {
                        restAPIIdeaSupportingRequest.ideaAttachments.Add(ideaAttachmentRequest);
                    }
                    if (mimeRequestType.CompareTo(MimeRequestType.IntelectualProperty) == 0)
                    {
                        SubmitIntellectualrequest.ideaAttachments.Add(ideaAttachmentRequest);
                        SubmitIntellectualrequest.files.Add(httpContent);
                    }
                    if(mimeRequestType.CompareTo(MimeRequestType.UpdateIntellectualProperty) == 0)
                    { 
                        UpdateIntellectualRequest.ideaAttachments.Add(ideaAttachmentRequest);
                        UpdateIntellectualRequest.files.Add(httpContent);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private static IdeaAttachmentRequest AttachmentRequest(HttpContent httpContent, Stream inputs)
        {
            IdeaAttachmentRequest ideaAttachmentRequest = new IdeaAttachmentRequest();
            ideaAttachmentRequest.AttachedFileName = httpContent.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            ideaAttachmentRequest.FileSizeInByte = inputs.Length;
            ideaAttachmentRequest.FileExtention = Path.GetExtension(ideaAttachmentRequest.AttachedFileName);
            ideaAttachmentRequest.ModifiedDate = DateTime.UtcNow;
            ideaAttachmentRequest.DocumentTypeFolderName = UnquoteToken(httpContent.Headers.ContentDisposition.Name);
            ideaAttachmentRequest.DefaultFolder = DefaultDirectoryName;
            ideaAttachmentRequest.stream = inputs;
            return ideaAttachmentRequest;
        }

        private string GetFileName(HttpContent fileContent)
        {
            return fileContent.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }


        /// <summary>  
        /// Remove bounding quotes on a token if present  
        /// </summary>  
        /// <param name="token">Token to unquote.</param>  
        /// <returns>Unquoted token.</returns>  
        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }
            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }

        private void MapNameValueCollection(string key, object value, object objType)
        {
            try
            {
                Type t = value.GetType();

                if (string.IsNullOrEmpty(key))
                {
                    throw new HttpRequestException($"Did not find required '{key}' header field in MIME multipart body part..");
                }
                PropertyInfo pi = objType.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    pi.SetValue(objType, ChangeType(value, pi.PropertyType), null);
                }
                
            }
            catch (Exception e)
            {
                throw new HttpRequestException(string.Format("Did not find required '{0}' header field in MIME multipart body part..", "Content-Disposition"), e.InnerException);
            }

        }


        private object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
    }
}