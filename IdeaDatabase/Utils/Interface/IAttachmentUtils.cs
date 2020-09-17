using IdeaDatabase.Requests;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
    public interface IAttachmentUtils
    {
        void SubmitIdeaAttachment(ResponseBase response, RestAPIIdeaSupportingRequest restAPIIdeaSupportingRequest, int UserID);
    }
}
