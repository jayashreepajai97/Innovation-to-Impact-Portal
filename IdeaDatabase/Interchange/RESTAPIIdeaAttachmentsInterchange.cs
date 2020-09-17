using IdeaDatabase.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeaAttachmentsInterchange
    {
        public int IdeaAttachmentID { get; set; }
        public string AttachedFileName { get; set; }
        public string FileExtention { get; set; }
        public Nullable<long> FileSizeInByte { get; set; }
        public string CreatedDate { get; set; }
        public string FolderName { get; set; }
        public string FilePath { get; set; }

        public RESTAPIIdeaAttachmentsInterchange(IdeaAttachment ideaAttachment)
        {
            string Awsip, ITGIP, hostName, portNumber, AWSIdeaIPFolder, AWSIdeaAttachmentFolder;
            int ideaId;
            string domain = HttpContext.Current.Request.Url.Scheme;
            hostName = Environment.MachineName;
            Awsip = WebConfigurationManager.AppSettings["AWSHost"];
            portNumber = WebConfigurationManager.AppSettings["AWSHostPort"];
            AWSIdeaIPFolder = WebConfigurationManager.AppSettings["AWSIdeaIPFolder"];
            AWSIdeaAttachmentFolder = WebConfigurationManager.AppSettings["AWSIdeaAttachmentFolder"];
            bool IsS3Enabled = Convert.ToBoolean(WebConfigurationManager.AppSettings["IsS3Enabled"]);

            if (ideaAttachment != null)
            {
                IdeaAttachmentID = ideaAttachment.IdeaAttachmentId;
                AttachedFileName = ideaAttachment.AttachedFileName;
                FileExtention = ideaAttachment.FileExtention;
                FileSizeInByte = ideaAttachment.FileSizeInByte;
                CreatedDate = ideaAttachment.CreatedDate?.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                FolderName = ideaAttachment.FolderName;

                if (IsS3Enabled)
                {
                    FilePath = new Uri(string.Format(@"{0}/{1}/{2}/{3}/{4}", Awsip, AWSIdeaAttachmentFolder, ideaAttachment.IdeaId, ideaAttachment.FolderName, ideaAttachment.AttachedFileName)).ToString();
                }
                else
                {
                    ITGIP = WebConfigurationManager.AppSettings["ITGIP"];
                    FilePath = string.Format(@"{0}://{1}:{2}/{3}/{4}/{5}/{6}", domain, ITGIP, portNumber, AWSIdeaAttachmentFolder, ideaAttachment.IdeaId, ideaAttachment.FolderName, ideaAttachment.AttachedFileName);
                }
            }

        }
    }
}