using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace IdeaDatabase.Utils.IImplementation
{
    public class IdeaUtils : IIdeaUtils
    {
        bool IsS3Enabled = Convert.ToBoolean(WebConfigurationManager.AppSettings["IsS3Enabled"]);

        public string getImagePath(Idea idea)
        {
            string  folderName, path, fileName, hostName, AWSIdeaAttachmentFolder, portNumber;
            string Awsip, ITGIP;
           
            string domain = HttpContext.Current.Request.Url.Scheme;
            hostName = Environment.MachineName;
         
            portNumber = WebConfigurationManager.AppSettings["AWSHostPort"];

            AWSIdeaAttachmentFolder = WebConfigurationManager.AppSettings["AWSIdeaAttachmentFolder"];

            folderName = idea.IdeaAttachments.FirstOrDefault(x => x.IdeaId == idea.IdeaId && x.FolderName == (folderNames.DefaultImage.ToString())).FolderName;
            fileName = idea.IdeaAttachments.FirstOrDefault(x => x.IdeaId == idea.IdeaId && x.FolderName == (folderNames.DefaultImage.ToString())).AttachedFileName;

            if (IsS3Enabled)
            {
                Awsip = WebConfigurationManager.AppSettings["AWSHost"];
                return path= new Uri(string.Format(@"{0}/{1}/{2}/{3}/{4}", Awsip, AWSIdeaAttachmentFolder, idea.IdeaId, folderName, fileName)).ToString();
            }
            ITGIP = WebConfigurationManager.AppSettings["ITGIP"];
            return path = string.Format(@"{0}://{1}:{2}/{3}/{4}/{5}/{6}", domain, ITGIP, portNumber, AWSIdeaAttachmentFolder, idea.IdeaId, folderName, fileName);
        }

        public string getDefaultImagePath()
        {
            string Awsip, ITGIP, defaultPath, IdeaDefaultImage, portNumber;
            string domain = HttpContext.Current.Request.Url.Scheme;
             
            IdeaDefaultImage = WebConfigurationManager.AppSettings["IdeaDefaultImage"];
            portNumber = WebConfigurationManager.AppSettings["AWSHostPort"];

            if (IsS3Enabled) {
                Awsip = WebConfigurationManager.AppSettings["AWSHost"];
                return defaultPath = new Uri(string.Format(@"{0}/{1}", Awsip, IdeaDefaultImage)).ToString();
            }
            ITGIP = WebConfigurationManager.AppSettings["ITGIP"];
            return defaultPath = string.Format(@"{0}://{1}:{2}/{3}", domain, ITGIP, portNumber, IdeaDefaultImage);

        }

        public int GetIdeaState(Idea idea)
        {
            return idea.IdeaStatusLogs.Where(x => x.IdeaId == idea.IdeaId).OrderByDescending(x => x.CreatedDate).FirstOrDefault().IdeaState;
        }

      

        public string getStatus(Idea idea)
        {
            string enumValue = null;
            var IdeaState = GetIdeaState(idea);

            if (Enum.IsDefined(typeof(IdeaStatusTypes), IdeaState) == true)
            {
                enumValue = EnumDescriptor.GetEnumDescription((IdeaStatusTypes)IdeaState);  
            }
            return enumValue;
        }

        public RESTAPIIdeaDetailsInterchange GetIdeasDetails(IIdeaDatabaseDataContext dc, int IdeaId)
        {

            string Awsip, ITGIP, folderName, path, fileName, hostName, portNumber, AWSIdeaIPFolder, AWSIdeaAttachmentFolder;
            int ideaId;
            ITGIP = WebConfigurationManager.AppSettings["ITGIP"];
            string domain = HttpContext.Current.Request.Url.Scheme;
            hostName = Environment.MachineName;
            Awsip = WebConfigurationManager.AppSettings["AWSHost"];
            portNumber = WebConfigurationManager.AppSettings["AWSHostPort"];
            AWSIdeaIPFolder = WebConfigurationManager.AppSettings["AWSIdeaIPFolder"];
            AWSIdeaAttachmentFolder = WebConfigurationManager.AppSettings["AWSIdeaAttachmentFolder"];
            bool IsS3Enabled = Convert.ToBoolean(WebConfigurationManager.AppSettings["IsS3Enabled"]);



            RESTAPIIdeaReviewerInterchange restAPIIdeaReviewerInterchange = null;
            RESTAPIIntellectualInterchange rESTAPIIntellectualInterchange = null;
            //RESTAPIIdeaContributorInterchange rESTAPIIdeaContributorInterchange = null;

            var ideaDetails = (from idea in dc.Ideas
                               where idea.IdeaId == IdeaId
                               select idea
                       ).FirstOrDefault();

            if (ideaDetails == null)
            {
                return null;
            }

            RESTAPIIdeaDetailsInterchange ideaDetailsInterchange = new RESTAPIIdeaDetailsInterchange(ideaDetails);

            //Idea
            var Idea = dc.Ideas.Where(x => x.IdeaId == IdeaId).FirstOrDefault();

            //  IdeaTagId
            var tags = dc.Ideatags.Where(x => x.IdeaId == IdeaId).ToList();
            foreach (var tag in tags)
            {
                //ideaDetailsInterchange.TagList.Add(new RESTAPIIdeatagInterchange { Tags = tag.Tags, CreatedDate = tag.CreatedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"), AddedByUserId = tag.AddedByUserId });
                ideaDetailsInterchange.TagList.Add(tag.Tags);
            }

            //////  IdeaAssignments

            //var assign = dc.IdeaAssignments.Where(x => x.IdeaId == IdeaId).ToList();
            //foreach (var ass in assign)
            //{
            //    ideaDetailsInterchange.AssignmentList.Add(new IdeaAssignment { IsActive = ass.IsActive, CreatedDate = ass.CreatedDate, ReviewByUserId = ass.ReviewByUserId });
            //}

            ////IdeaComments
            //var comments = dc.IdeaComments.Where(x => x.IdeaId == IdeaId).ToList();
            //foreach (var comm in comments)
            //{
            //    ideaDetailsInterchange.IdeaCommentList.Add(new IdeaComment { CommentDescription = comm.CommentDescription, CreatedDate = comm.CreatedDate, CommentByUserid = comm.CommentByUserid });
            //}

            var ideastatus = dc.IdeaStatusLogs.Where(x => x.IdeaId == IdeaId && x.IsActive == true).OrderByDescending(x => x.CreatedDate).FirstOrDefault();  /* && x.IdeaState == (int)IdeaStatusTypes.ReviewPending)*/
            var ideaAssignment = dc.IdeaAssignments.Where(x => x.IdeaId == IdeaId).FirstOrDefault();


            if (ideastatus.IdeaState == 1)
            {
                var reviewerUser = dc.Users.Where(x => x.UserId == ideaAssignment.ReviewByUserId).FirstOrDefault();
                restAPIIdeaReviewerInterchange = new RESTAPIIdeaReviewerInterchange()
                {
                    Username = reviewerUser.FirstName + " " + reviewerUser.LastName,
                    EmailAddress = reviewerUser.EmailAddress,
                    CreatedDate = ideaAssignment.CreatedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                };
                if (reviewerUser != null)
                {
                    ideaDetailsInterchange.ReviewersList.Add(restAPIIdeaReviewerInterchange);
                }
                ideaDetailsInterchange.SponsorsList = new List<RESTAPIIdeaReviewerInterchange>();
            }
            else if (ideastatus.IdeaState == 2)
            {
                // reviewer user data
                var ideastatuslog = dc.IdeaStatusLogs.Where(x => x.IdeaId == IdeaId && x.IdeaState == (int)IdeaStatusTypes.SponsorPending).FirstOrDefault();

                var reviewerUser = dc.Users.Where(x => x.UserId == ideastatuslog.ModifiedByUserId).FirstOrDefault();

                restAPIIdeaReviewerInterchange = new RESTAPIIdeaReviewerInterchange()
                {
                    Username = reviewerUser.FirstName + " " + reviewerUser.LastName,
                    EmailAddress = reviewerUser.EmailAddress,
                    CreatedDate = ideastatuslog.CreatedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                };
                if (reviewerUser != null)
                {
                    ideaDetailsInterchange.ReviewersList.Add(restAPIIdeaReviewerInterchange);
                }

                // sponsor user data

                var sponsorUser = dc.Users.Where(x => x.UserId == ideaAssignment.ReviewByUserId).FirstOrDefault();
                restAPIIdeaReviewerInterchange = new RESTAPIIdeaReviewerInterchange()
                {
                    Username = sponsorUser.FirstName + " " + sponsorUser.LastName,
                    EmailAddress = sponsorUser.EmailAddress,
                    CreatedDate = ideaAssignment.CreatedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                };
                if (sponsorUser != null)
                {
                    ideaDetailsInterchange.SponsorsList.Add(restAPIIdeaReviewerInterchange);
                }
            }
            else if (ideastatus.IdeaState == 3)
            {
                // reviewer user data
                var ideastatuslog = dc.IdeaStatusLogs.Where(x => x.IdeaId == IdeaId && x.IdeaState == (int)IdeaStatusTypes.SponsorPending).FirstOrDefault();

                var reviewerUser = dc.Users.Where(x => x.UserId == ideastatuslog.ModifiedByUserId).FirstOrDefault();

                restAPIIdeaReviewerInterchange = new RESTAPIIdeaReviewerInterchange()
                {
                    Username = reviewerUser.FirstName + " " + reviewerUser.LastName,
                    EmailAddress = reviewerUser.EmailAddress,
                    CreatedDate = ideastatuslog.CreatedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                };
                if (reviewerUser != null)
                {
                    ideaDetailsInterchange.ReviewersList.Add(restAPIIdeaReviewerInterchange);
                }

                // sponsor user data
                var approvedIdea = dc.IdeaStatusLogs.Where(x => x.IdeaId == IdeaId && x.IdeaState == (int)IdeaStatusTypes.Sponsored).FirstOrDefault();
                var sponsorUser = dc.Users.Where(x => x.UserId == approvedIdea.ModifiedByUserId).FirstOrDefault();
                restAPIIdeaReviewerInterchange = new RESTAPIIdeaReviewerInterchange()
                {
                    Username = sponsorUser.FirstName + " " + sponsorUser.LastName,
                    EmailAddress = sponsorUser.EmailAddress,
                    CreatedDate = approvedIdea.CreatedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                };
                if (sponsorUser != null)
                {
                    ideaDetailsInterchange.SponsorsList.Add(restAPIIdeaReviewerInterchange);
                }
            }

            //IdeaAttachment
            //var attach = dc.IdeaAttachments.Where(x => x.IdeaId == IdeaId).ToList();
            //foreach (var at in attach)
            //{
            //    ideaId = at.IdeaId;
            //    folderName = at.FolderName;
            //    fileName = at.AttachedFileName;

            //    if (IsS3Enabled)
            //    {
            //        path = new Uri(string.Format(@"{0}/{1}/{2}/{3}/{4}", Awsip, AWSIdeaAttachmentFolder, ideaId, folderName, fileName)).ToString();
            //    }
            //    else
            //    {
            //        ITGIP = WebConfigurationManager.AppSettings["ITGIP"];
            //        path = string.Format(@"{0}://{1}:{2}/{3}/{4}/{5}/{6}", domain, ITGIP, portNumber, AWSIdeaAttachmentFolder, ideaId, folderName, fileName);
            //    }

            //    ideaDetailsInterchange.AttachmentsList.Add(new RESTAPIIdeaAttachmentInterchange { IdeaAttachmentID = at.IdeaAttachmentId, AttachedFileName = at.AttachedFileName, FileExtention = at.FileExtention.Trim(), FileSizeInByte = at.FileSizeInByte, FolderName = at.FolderName, CreatedDate = at.CreatedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"), FilePath = path });
            //}

            //IdeaIntellectualProperty
            var intellectual = dc.IdeaIntellectualProperties.Where(x => x.IdeaId == IdeaId).ToList();
            path = string.Empty;
            foreach (var intellect in intellectual)
            {
                rESTAPIIntellectualInterchange = new RESTAPIIntellectualInterchange();
                foreach (var attachment in intellect.IdeaAttachments)
                {
                    ideaId = attachment.IdeaId;
                    folderName = attachment.FolderName;
                    fileName = attachment.AttachedFileName;

                    if (attachment.IntellectualPropertyId != null)
                    {
                        ideaId = attachment.IdeaId;
                        folderName = attachment.FolderName;
                        fileName = attachment.AttachedFileName;

                        if (IsS3Enabled)
                        {
                            path = new Uri(string.Format(@"{0}/{1}/{2}/{3}/{4}", Awsip, AWSIdeaAttachmentFolder, ideaId, folderName, fileName)).ToString();
                        }
                        else
                        {
                            path = string.Format(@"{0}://{1}:{2}/{3}/{4}/{5}/{6}", domain, ITGIP, portNumber, AWSIdeaAttachmentFolder, ideaId, AWSIdeaIPFolder, fileName);
                        }

                        rESTAPIIntellectualInterchange.AttachmentsList.Add(
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

                rESTAPIIntellectualInterchange.IntellectualId = intellect.IntellectualId;
                rESTAPIIntellectualInterchange.RecordId = intellect.RecordId;
                rESTAPIIntellectualInterchange.Status = intellect.Status;
                rESTAPIIntellectualInterchange.FiledDate = intellect.FiledDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                rESTAPIIntellectualInterchange.ApplicationNumber = intellect.ApplicationNumber;
                rESTAPIIntellectualInterchange.PatentId = intellect.PatentId;
                rESTAPIIntellectualInterchange.CreatedDate = intellect.CreatedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                rESTAPIIntellectualInterchange.ModifiedDate = intellect.ModifiedDate?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                rESTAPIIntellectualInterchange.InventionReference = intellect.InventionReference;

                ideaDetailsInterchange.IntellectualList.Add(rESTAPIIntellectualInterchange);
            }

            // IdeaContributors
            var ideaContributors = dc.IdeaContributors.Where(x => x.IdeaId == IdeaId).ToList();
            foreach (var contributor in ideaContributors)
            {
                User user = dc.Users.Where(x => x.UserId == contributor.UserId).FirstOrDefault();
                if (user != null)
                {
                    ideaDetailsInterchange.ContributorList.Add(new RESTAPIIdeaContributorInterchange { EmailAddress = user.EmailAddress, Username = string.Concat(user.FirstName, ' ', user.LastName) });
                }
            }
            return ideaDetailsInterchange;
        }
    }
}