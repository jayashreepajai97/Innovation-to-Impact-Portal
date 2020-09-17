using IdeaDatabase.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeaCommentInterchange
    {
        public int IdeaCommentId { get; set; }
        public int IdeaID { get; set; }
        public string CommentDescription { get; set; }
        public string CreatedDate { get; set; }
        public int CommentByUserid { get; set; }
        public string CommentByUserName { get; set; }

        public List<RESTAPIIdeaDiscussionInterchange> CommentDiscussion { get; set; }

        public RESTAPIIdeaCommentInterchange(IdeaComment ideaComment)
        {
            List<RESTAPIIdeaDiscussionInterchange> DiscussionList = null;
            List<RESTAPIIdeaDiscussionInterchange> discussionInterchangeList = null;

            if (ideaComment != null)
            {
                discussionInterchangeList = new List<RESTAPIIdeaDiscussionInterchange>();

                IdeaCommentId = ideaComment.IdeaCommentId;
                IdeaID = ideaComment.IdeaId;
                CommentDescription = ideaComment.CommentDescription;
                CreatedDate = ideaComment.CreatedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"); ;
                CommentByUserid = ideaComment.CommentByUserId;
                CommentByUserName = ideaComment.User.FirstName;


                if (ideaComment.IdeaCommentDiscussions != null)
                {
                    DiscussionList = new List<RESTAPIIdeaDiscussionInterchange>();
                    foreach(IdeaCommentDiscussion ideaCommentDiscussion in ideaComment.IdeaCommentDiscussions)
                    {
                        discussionInterchangeList.Add(new RESTAPIIdeaDiscussionInterchange(ideaCommentDiscussion));
                    }
                }

                if (discussionInterchangeList != null && discussionInterchangeList.Count > 0)
                    DiscussionList.AddRange(discussionInterchangeList);

                CommentDiscussion = DiscussionList;
            }
        }
    }
}