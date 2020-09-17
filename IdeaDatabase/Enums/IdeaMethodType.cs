using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Enums
{
    public enum IdeaMethodTypes
    {
        [Description("/ideas")]
        InsertIdea = 0,

        [Description("/ideas")]
        AddIdeaAttachments = 1,

        [Description("/ideas/private")]
        UpdateSensitive = 2,

        [Description("/ideas/status")]
        UpdateIdeaStatus = 3,

        [Description("/ideas/comments")]
        AddIdeaComment = 4,

        [Description("/ideas/comments/{IdeaId}")]
        GetIdeaComments = 5,

        [Description("/ideas/comments/{IdeaCommentId}")]
        DeleteIdeaComment = 6,

        [Description("/ideas")]
        GetIdeaList = 7,

        [Description("/ideas/details/{IdeaId}")]
        GetDetails = 8,

        [Description("/reviewers/ideas")]
        GetIdeaReviewsList = 9,

        [Description("/sponsors/ideas")]
        GetIdeaSponsorsList = 10,

        [Description("/ideas/public")]
        GetPublicList = 11,

        [Description("/ideas/comments/reply")]
        AddIdeaCommentReply = 12,

        [Description("/ideas/search")]
        SearchIdea = 13,

        [Description("/ideas/details/{IdeaId}")]
        UpdateDetails = 14,

        [Description("/email/{EmailAddress}")]
        SendEmail = 15,

        [Description("/ideas/intellectual")]
        AddIntellectualProperty = 16,

        [Description("/ideas/intellectual")]
        DeleteIntellectualProperty = 17,

        [Description("/ideas/intellectual")]
        UpdateIntellectualProperty = 18,

        [Description("/ideas/intellectual/{IntellectualId}")]
        GetIntellectualProperty = 19,

        [Description("/ideas")]
        UpdateIdeaDraft = 20,

        [Description("/ideas/attachments/{IdeaAttachmentId}")]
        DeleteIdeaAttachment = 21
    }

}