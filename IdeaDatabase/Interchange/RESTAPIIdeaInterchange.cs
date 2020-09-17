using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using System;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace IdeaDatabase.Interchange
{
    public enum folderNames {
        DefaultImage,
        VideoAttachment,
        Documents
    }
    public class RESTAPIIdeaInterchange
    {
        private IIdeaUtils IdeaUtils = DependencyInjector.Get<IIdeaUtils, IdeaUtils>();
        public int IdeaId { get; set; }
        public string Username { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool IsAttachment { get; set; }
        public Nullable<int> AttachmentCount { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> TotalFollowers { get; set; }
        public Nullable<bool> IsBookmarked { get; set; }
        public string Image { get; set; }
        public string BusinessImpact { get; set; }
        public Nullable<int> CommentsCount { get; set; }
        public int Rating { get; set; }
        public Nullable<int> ChallengeId { get; set; }
        public string Solution { get; set; }
        public string GitRepo { get; set; }
        public string ChallengeName { get; set; }
        public string Tag { get; set; }
        public Nullable<bool> IsDraft { get; set; }
        public Nullable<bool> IsActive { get; set; }


        public RESTAPIIdeaInterchange(Idea idea)
        {
            IdeaUtils ideaUtils = new IdeaUtils();

            if (idea != null)
            {
                IdeaId = idea.IdeaId;
                Username = string.Concat(idea.User.FirstName, idea.User.LastName);
                Title = idea.Title;
                Description = idea.Description;
                Status = ideaUtils.getStatus(idea);
                IsAttachment = idea.IsAttachment;
                AttachmentCount = idea.AttachmentCount == null ? 0 : idea.AttachmentCount;
                CreatedDate = idea.CreatedDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                ModifiedDate = idea.ModifiedDate?.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                CategoryName = idea.IdeaCategory.CategoriesName;
                BusinessImpact = idea.BusinessImpact;
                ChallengeId = idea.ChallengeId;
                Solution = idea.Solution;
                GitRepo = idea.GitRepo;
                Tag = null;
                IsDraft = idea.IsDraft;
                IsActive = idea.IsActive;

                if (idea.IdeaChallenge != null)
                    ChallengeName = idea.IdeaChallenge.ChallengeName;

                var tags = idea.Ideatags.Where(x => x.IdeaId == idea.IdeaId).Select(x => x.Tags).DefaultIfEmpty().First();

                if(tags != null)
                {
                    Tag = tags;
                }
                 

                TotalFollowers = Convert.ToInt32(idea.IdeaSubscribers.FirstOrDefault(x => x.IdeaId == idea.IdeaId)?.TotalFollowers);
                IsBookmarked = idea.IdeaSubscribers.FirstOrDefault(x => x.IdeaId == idea.IdeaId)?.IsBookmarked == true ? true : false;
                CommentsCount = idea.IdeaComments.Where(x => x.IdeaId == idea.IdeaId).Count();
                Rating = Convert.ToInt32(idea.IdeaSubscribers.FirstOrDefault(x => x.IdeaId == idea.IdeaId)?.TotalRating);

                if (idea.IsAttachment == true)
                {
                    var ret = idea.IdeaAttachments;
                
                    if (ret != null)
                    {
                        bool IsValue = ret.Any(a => a.FolderName==(folderNames.DefaultImage.ToString()));
                        if (IsValue)
                            Image = IdeaUtils.getImagePath(idea);
                        else {
                            Image = IdeaUtils.getDefaultImagePath();
                        }
                    }
                   
                }
                else
                {
                    Image = IdeaUtils.getDefaultImagePath();
                }
            }
        }


        //string getStatus(Idea idea)
        //{
        //    IdeaUtils ideaUtils = new IdeaUtils();
        //    string enumValue = null;
        //    var IdeaState = ideaUtils.GetIdeaState(idea);

        //    if (Enum.IsDefined(typeof(IdeaStatusTypes), IdeaState) == true)
        //    {
        //        enumValue = EnumDescriptor.GetEnumDescription((IdeaStatusTypes)IdeaState);  //Enum.GetName(typeof(IdeaStatusTypes), IdeaState);
        //    }
        //    return enumValue;
        //}

    }
}