using IdeaDatabase.Utils.IImplementation;
using Hpcs.DependencyInjector;
using InnovationPortalService.Requests;
using System;
using System.Threading.Tasks;
using IdeaDatabase.Utils.Interface;
using IdeaDatabase.Responses;
using System.Collections.Generic;
using System.Linq;
using IdeaDatabase.DataContext;
using System.Runtime.CompilerServices;
using IdeaDatabase.Requests;

namespace InnovationPortalService.Utils
{
    public interface IIdeaUtil
    {
        SubmitIdeaAttachmentResponse SubmitIdeaAttachment(InMemoryMultipartFormDataStreamProvider provider, int UserID);
        RestAPIAddIntellectualResponse SubmitIntellectual(InMemoryMultipartFormDataStreamProvider provider, int UserID);
        RestAPISubmitIdeaResponse SubmitIdeas(RestAPISubmitIdeaResponse response, RestAPISubmitIdeaRequest req, int UserID);
        void UpdateIdeaDraft(RestAPIUpdateDraftResponse response, RestAPIUpdateIdeaDraftRequest request, int UserID);
        RestAPIUpdateIntellectResponse UpdateIntellectual(InMemoryMultipartFormDataStreamProvider provider, int UserID);
        void GetIdeaAttachments(RestAPIGetAttachmentsResponse response, int IdeaId);


    }
}