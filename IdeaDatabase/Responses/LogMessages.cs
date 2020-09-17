using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Responses
{
    public static class LogMessages
    {
        public static string UpdateSensitive { get; } = "updated idea stealth mode.";
        public static string AddIdeaComment { get; } = "commented on your idea.";
        public static string UpdateIdeaStatus { get; } = "updated idea status.";
        public static string AddIdeaCommentReply { get; } = "replied on your comment.";
        public static string UpdateDetails { get; } = "Your idea has been updated by";
    }
}