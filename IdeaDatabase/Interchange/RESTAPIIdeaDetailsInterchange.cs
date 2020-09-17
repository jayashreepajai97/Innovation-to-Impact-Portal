using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeaDetailsInterchange
    {

        private IIdeaUtils IdeaUtils = DependencyInjector.Get<IIdeaUtils, IdeaUtils>();
        public int IdeaId { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool IsAttachment { get; set; }
        public int AttachmentCount { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CategoryName { get; set; }
        public int? TotalFollowers { get; set; }
        public bool? IsBookmarked { get; set; }
        public string Image { get; set; }
        public string BusinessImpact { get; set; }
        public Nullable<int> CommentsCount { get; set; }
        public int Rating { get; set; }
        public int ApprovalStatus { get; set; }
        public Nullable<bool> IsSensitive { get; set; }
        public Nullable<int> ChallengeId { get; set; }
        public string Solution { get; set; }
        public string GitRepo { get; set; }
        public string ChallengeName { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<bool> IsDraft { get; set; }


        public List<string> TagList { get; set; }

        public List<RESTAPIIdeaReviewerInterchange> ReviewersList { get; set; }

        public List<RESTAPIIdeaReviewerInterchange> SponsorsList { get; set; }

        public List<RESTAPIIdeaAttachmentInterchange> AttachmentsList { get; set; }

        public List<RESTAPIIntellectualInterchange> IntellectualList { get; set; }

        public List<RESTAPIIdeaContributorInterchange> ContributorList { get; set; }

        IdeaUtils ideaUtils = new IdeaUtils();

        public RESTAPIIdeaDetailsInterchange(Idea idea)
        {

            if (idea != null)
            {
                IdeaId = idea.IdeaId;

                Username = string.Concat(idea.User.FirstName, " ", idea.User.LastName);
                Title = idea.Title;
                Description = idea.Description;
                Status = ideaUtils.getStatus(idea);
                IsAttachment = idea.IsAttachment;
                CreatedDate = idea.CreatedDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                ModifiedDate = idea.ModifiedDate?.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                CategoryName = idea.IdeaCategory.CategoriesName;
                BusinessImpact = idea.BusinessImpact;
                IsSensitive = idea.IsSensitive;
                ChallengeId = idea.ChallengeId;
                Solution = idea.Solution;
                GitRepo = idea.GitRepo;
                EmailAddress = idea.User.EmailAddress;
                CategoryId = idea.CategoryId;
                IsDraft = idea.IsDraft;

                TotalFollowers = Convert.ToInt32(idea.IdeaSubscribers.FirstOrDefault(x => x.IdeaId == idea.IdeaId)?.TotalFollowers);
                IsBookmarked = idea.IdeaSubscribers.FirstOrDefault(x => x.IdeaId == idea.IdeaId)?.IsBookmarked == true ? true : false; 
                CommentsCount = idea.IdeaComments.Where(x => x.IdeaId == idea.IdeaId).Count();
                Rating = Convert.ToInt32(idea.IdeaSubscribers.FirstOrDefault(x => x.IdeaId == idea.IdeaId)?.TotalRating);
                ApprovalStatus = ideaUtils.GetIdeaState(idea);

                int count = idea.IdeaAttachments.Where(x => x.FolderName != Enum.GetName(typeof(folderNames), folderNames.DefaultImage)).Count();
                AttachmentCount = count == 0 ? 0 : count;

                if (idea.IdeaChallenge != null)
                    ChallengeName = idea.IdeaChallenge.ChallengeName;

                TagList = new List<string>();
                ReviewersList = new List<RESTAPIIdeaReviewerInterchange>();
                SponsorsList = new List<RESTAPIIdeaReviewerInterchange>();
                AttachmentsList = new List<RESTAPIIdeaAttachmentInterchange>();
                IntellectualList = new List<RESTAPIIntellectualInterchange>();
                ContributorList = new List<RESTAPIIdeaContributorInterchange>();

                if (idea.IsAttachment == true)
                {
                    var ret = idea.IdeaAttachments;

                    if (ret != null)
                    {
                        if (ret != null)
                        {
                            bool IsValue = ret.Any(a => a.FolderName == (folderNames.DefaultImage.ToString()));
                            if (IsValue)
                                Image = IdeaUtils.getImagePath(idea);
                            else
                            {
                                Image = IdeaUtils.getDefaultImagePath();
                            }
                        }
                    }
                }
                else
                {
                    Image = IdeaUtils.getDefaultImagePath();
                }
            }
        }
    }
}