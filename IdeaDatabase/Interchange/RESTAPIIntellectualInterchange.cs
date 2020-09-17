using IdeaDatabase.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIntellectualInterchange
    {
        public int IntellectualId { get; set; }
        public string RecordId { get; set; }
        public Nullable<int> Status { get; set; }
        public string FiledDate { get; set; }
        public string ApplicationNumber { get; set; }
        public string PatentId { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string InventionReference { get; set; }
        public List<RESTAPIIdeaAttachmentInterchange> AttachmentsList { get; set; }

        public RESTAPIIntellectualInterchange()
        {
            AttachmentsList = new List<RESTAPIIdeaAttachmentInterchange>();
        }

        public RESTAPIIntellectualInterchange(IdeaIntellectualProperty ideaIntellectualProperty)
        {
            AttachmentsList = new List<RESTAPIIdeaAttachmentInterchange>();

            string Awsip, ITGIP, folderName, path, fileName, portNumber, AWSIdeaIPFolder, AWSIdeaAttachmentFolder;

            ITGIP = WebConfigurationManager.AppSettings["ITGIP"];
            string domain = HttpContext.Current.Request.Url.Scheme;
            Awsip = WebConfigurationManager.AppSettings["AWSHost"];
            portNumber = WebConfigurationManager.AppSettings["AWSHostPort"];
            AWSIdeaIPFolder = WebConfigurationManager.AppSettings["AWSIdeaIPFolder"];
            AWSIdeaAttachmentFolder = WebConfigurationManager.AppSettings["AWSIdeaAttachmentFolder"];
            bool IsS3Enabled = Convert.ToBoolean(WebConfigurationManager.AppSettings["IsS3Enabled"]);

            if (ideaIntellectualProperty != null)
            {
                IntellectualId = ideaIntellectualProperty.IntellectualId;
                RecordId = ideaIntellectualProperty.RecordId;
                Status = ideaIntellectualProperty.Status;
                FiledDate = ideaIntellectualProperty.FiledDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                ApplicationNumber = ideaIntellectualProperty.ApplicationNumber;
                PatentId = ideaIntellectualProperty.PatentId;
                CreatedDate = ideaIntellectualProperty.CreatedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                ModifiedDate = ideaIntellectualProperty.ModifiedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                InventionReference = ideaIntellectualProperty.InventionReference;


                foreach (var attachment in ideaIntellectualProperty.IdeaAttachments)
                {
                    if (attachment.IntellectualPropertyId != null)
                    {
                        folderName = attachment.FolderName;
                        fileName = attachment.AttachedFileName;

                        if (IsS3Enabled)
                        {
                            path = new Uri(string.Format(@"{0}/{1}/{2}/{3}/{4}", Awsip, AWSIdeaAttachmentFolder, attachment.IdeaId, folderName, fileName)).ToString();
                        }
                        else
                        {
                            path = string.Format(@"{0}://{1}:{2}/{3}/{4}/{5}/{6}", domain, ITGIP, portNumber, AWSIdeaAttachmentFolder, attachment.IdeaId, AWSIdeaIPFolder, fileName);
                        }


                        AttachmentsList.Add(
                            new RESTAPIIdeaAttachmentInterchange
                            {
                                IdeaAttachmentID = attachment.IdeaAttachmentId,
                                AttachedFileName = attachment.AttachedFileName,
                                FileExtention = attachment.FileExtention.Trim(),
                                FileSizeInByte = attachment.FileSizeInByte,
                                FolderName = attachment.FolderName,
                                CreatedDate = attachment.CreatedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                                FilePath = path
                            });
                    }


                }
            }

        }
    }
}