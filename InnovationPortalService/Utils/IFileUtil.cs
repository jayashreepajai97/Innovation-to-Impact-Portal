using IdeaDatabase.Responses;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovationPortalService.Utils
{
    interface IFileUtil
    {
        void UploadAttachmentAsync(InMemoryMultipartFormDataStreamProvider dataStreamProvider, ResponseBase responseBase, int userID, string defaultPath);
        void UploadIntellectualAttachmentAsync(RestAPIAddIntellectualResponse IntellectualAttachmentResponse, InMemoryMultipartFormDataStreamProvider dataStreamProvider);
        void UploadIntellectualAttachmentAsync(RestAPIUpdateIntellectResponse IntellectualAttachmentResponse, InMemoryMultipartFormDataStreamProvider dataStreamProvider);


    }
}
