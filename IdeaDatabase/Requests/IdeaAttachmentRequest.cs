using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class IdeaAttachmentRequest : ValidableObject
    {
        [StringValidation(Required: true)]
        public string AttachedFileName { get; set; }
        [StringValidation(Required: true)]
        public string FileExtention { get; set; }
        [NumberValidation(min: 1, required: true)]
        public long FileSizeInByte { get; set; }
        [DateTimeValidation(required: true)]
        public DateTime ModifiedDate { get; set; }

        public string DocumentTypeFolderName { get; set; }
        public string DefaultFolder { get; set; }

        [NumberValidation(min: 1, required: true)]
        public int IdeaId { get; set; }
        public Stream stream { get; set; }
    }
}