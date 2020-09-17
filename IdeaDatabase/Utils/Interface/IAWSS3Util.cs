using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
   public interface IAWSS3Util
    {
        void UploadObject(Stream stream,string keyName, string folderPath);
        string GenerateURL(string KeyName);
    }
}
