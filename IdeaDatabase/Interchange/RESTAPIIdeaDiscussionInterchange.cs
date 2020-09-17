using IdeaDatabase.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeaDiscussionInterchange
    {
        public int IdeaCommentID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string DiscussionDescription { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }

        public RESTAPIIdeaDiscussionInterchange(IdeaCommentDiscussion ideaCommentDiscussion)
        {
            if(ideaCommentDiscussion != null)
            {
                IdeaCommentID = ideaCommentDiscussion.IdeaCommentId;
                UserID = ideaCommentDiscussion.UserId;
                DiscussionDescription = ideaCommentDiscussion.DiscussionDescription;
                CreatedDate = ideaCommentDiscussion.CreatedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"); ;
                ModifiedDate = ideaCommentDiscussion.ModifiedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"); ;
            }
        }
    }
}