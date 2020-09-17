using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeaAttachmentInterchange
    {
        public int IdeaAttachmentID { get; set; }
        public string AttachedFileName { get; set; }
        public string FileExtention { get; set; }
        public Nullable<long> FileSizeInByte { get; set; }
        public string CreatedDate { get; set; }
        public string FolderName { get; set; }
        public string FilePath { get; set; }

    }
}