using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class AttachmentUtils : IAttachmentUtils
    {
        private readonly ITagsUtils tagUtil = DependencyInjector.Get<ITagsUtils, TagsUtils>();

        public void SubmitIdeaAttachment(ResponseBase response, RestAPIIdeaSupportingRequest restAPIIdeaSupportingRequest, int UserID)
        {
            DatabaseWrapper.databaseOperation(response, (context, query) =>
            {
                Idea idea = query.GetIdeaById(context, restAPIIdeaSupportingRequest.IdeaId);
                if (idea == null)
                {
                    response.ErrorList.Add(Faults.IdeaNotFound);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(restAPIIdeaSupportingRequest.Ideatags))
                    tagUtil.InsertTags(response, idea, restAPIIdeaSupportingRequest.Ideatags, UserID);

                if (!string.IsNullOrEmpty(restAPIIdeaSupportingRequest.GitRepo))
                    idea.GitRepo = restAPIIdeaSupportingRequest.GitRepo;

                List<IdeaAttachment> ideaAttachments = new List<IdeaAttachment>();
                if (restAPIIdeaSupportingRequest.ideaAttachments.Count > 0)
                {
                    restAPIIdeaSupportingRequest.ideaAttachments.ForEach((Id) =>
                    {
                        IdeaAttachment newIdea = new IdeaAttachment();
                        newIdea.AttachedFileName = Id.AttachedFileName;
                        newIdea.FileExtention = Id.FileExtention;
                        newIdea.FileSizeInByte = Id.FileSizeInByte;
                        newIdea.IdeaId = Id.IdeaId;
                        newIdea.CreatedDate = DateTime.UtcNow;
                        newIdea.FolderName = Id.DocumentTypeFolderName;
                        ideaAttachments.Add(newIdea);

                        if (Id.DocumentTypeFolderName == Enum.GetName(typeof(folderNames), folderNames.DefaultImage))
                            query.DeleteDefaultImageAttachment(context, idea.IdeaId);

                    });
                    idea.IdeaAttachments = ideaAttachments;
                    query.AddIdeaAttachments(context, ideaAttachments);
                }

                idea.ModifiedDate = DateTime.UtcNow;
                context.SubmitChanges();

                idea.AttachmentCount = idea.IdeaAttachments.Count;
                idea.IsAttachment = idea.IdeaAttachments.Count == 0 ? false : true;

                context.SubmitChanges();

            }, readOnly: false
             );
            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
                return;
            }
        }
    }
}