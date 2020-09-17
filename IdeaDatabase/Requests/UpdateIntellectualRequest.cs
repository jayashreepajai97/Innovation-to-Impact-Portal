using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class UpdateIntellectualRequest
    {
        public int IntellectualId { get; set; }
        public int UserID { get; set; }
        public int IdeaId { get; set; }
        public string RecordId { get; set; }
        public Nullable<int> Status { get; set; }
        public DateTime? FiledDate { get; set; }
        public string ApplicationNumber { get; set; }
        public string PatentId { get; set; }
        public int AttachmentCount { get; set; } = 0;
        public bool IsAttachment { get; set; } = false;
        public string InventionReference { get; set; }
        public IList<IdeaAttachmentRequest> ideaAttachments { get; set; }
    }
}