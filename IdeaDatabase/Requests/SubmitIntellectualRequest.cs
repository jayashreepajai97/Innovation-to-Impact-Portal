using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class SubmitIntellectualRequest : ValidableObject
    {
        [RequiredValidation]
        public int UserID { get; set; }
        //[RequiredValidation]
        public int IdeaId { get; set; }
        [RequiredValidation]
        public string RecordId { get; set; }
        [StringValidation(Required: true)]
        public Nullable<int> Status { get; set; }
        public DateTime? FiledDate { get; set; }
        [RequiredValidation]
        public string ApplicationNumber { get; set; }
        [RequiredValidation]
        public string PatentId { get; set; }
        public int AttachmentCount { get; set; } = 0;
        public bool IsAttachment { get; set; } = false;
        [RequiredValidation]
        public string InventionReference { get; set; }
        public IList<IdeaAttachmentRequest> ideaAttachments { get; set; }
    }
}