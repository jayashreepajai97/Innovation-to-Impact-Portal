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
using IdeaDatabase.Utils;
using IdeaDatabase.Interchange;
using IdeaDatabase.Enums;
using EmailTemplate = IdeaDatabase.DataContext.EmailTemplate;
using IdeaDatabase.Requests;
using System.Runtime.CompilerServices;
using Responses;

namespace InnovationPortalService.Utils
{
    public class IdeaUtil : IIdeaUtil
    {
        ISubmitIdeaUtil ideautils = DependencyInjector.Get<ISubmitIdeaUtil, SubmitIdeaUtil>();
        IFileUtil fileUtil = DependencyInjector.Get<IFileUtil, FileUtil>();
        IUserUtils userUtil = DependencyInjector.Get<IUserUtils, UserUtils>();
        IAttachmentUtils  attachmentUtils = DependencyInjector.Get<IAttachmentUtils, AttachmentUtils>();

        string Failure = EnumUtils.ConvertValue<string>(ResponseStatusType.Failure);
        string Success = EnumUtils.ConvertValue<string>(ResponseStatusType.Success);

        static string DefaultDocumentFolderName = DateTime.UtcNow.ToString("yyyyMMdd");

        public RestAPISubmitIdeaResponse SubmitIdeas(RestAPISubmitIdeaResponse response, RestAPISubmitIdeaRequest req, int UserID)
        {
            try
            {
                ideautils.SubmitIdeaRequest(response, req, UserID);

                if (response.ErrorList.Count != 0)
                {
                    return response;
                }

                if (!req.IsDraft)
                {
                    // Submit IdeaAssignment and Send email if Idea is not drafted
                    ideautils.SubmitIdeaAssignment(response, response.IdeaId);
                    SendEmail(response.IdeaId);
                }

                if (response.ErrorList.Count != 0)
                {
                    return response;
                }

                if (response.ErrorList.Count != 0)
                    response.Status = Failure;

            }
            catch (Exception ex)
            {
                response.Status = Failure;
                throw new Exception(ex.Message, ex);
            }

            return response;
        }

        public SubmitIdeaAttachmentResponse SubmitIdeaAttachment(InMemoryMultipartFormDataStreamProvider provider, int UserID)
        {
            SubmitIdeaAttachmentResponse restAPISubmitIdeaResponse = new SubmitIdeaAttachmentResponse() { Status = Success };
            RestAPIIdeaSupportingRequest IdeaSupportingRequest = provider.restAPIIdeaSupportingRequest;
            IdeaSupportingRequest.UserID = UserID;

            try
            {  
                attachmentUtils.SubmitIdeaAttachment(restAPISubmitIdeaResponse, IdeaSupportingRequest, UserID);
                if (restAPISubmitIdeaResponse.ErrorList.Count != 0)
                {
                    restAPISubmitIdeaResponse.Status = Failure;
                    return restAPISubmitIdeaResponse;
                }

                fileUtil.UploadAttachmentAsync(provider, restAPISubmitIdeaResponse, UserID, provider.DefaultPath);

                if (restAPISubmitIdeaResponse.ErrorList.Count != 0)
                    restAPISubmitIdeaResponse.Status = Failure;
            }
            catch (Exception ex)
            {
                restAPISubmitIdeaResponse.Status = Failure;
                throw new Exception(ex.Message, ex);
            }
            return restAPISubmitIdeaResponse;
        }

        private void SendEmail(int IdeaId)
        {
            SubmitIdeaUtil submitIdeaUtil = new SubmitIdeaUtil();
            List<IdeaEmailToDetails> emailTo = submitIdeaUtil.GetAllStakeholdersEmailAdd(IdeaId);

            RESTAPIIdeaBasicDetailsInterchange ideaDetails = new RESTAPIIdeaBasicDetailsInterchange();

            ideaDetails = ideautils.GetIdeaBasicDetails(IdeaId);
            EmailUtil emailObj = new EmailUtil();

            // If the highest status in Statuslog is 1 then the User from Idea Assignmemts should be Reviwer
            // If the highest status in Statuslog is 2 then the User from Idea Assignmemts should be Sponsor
            // If the highest status in Statuslog is 3 then no action is needed as of now

            foreach(var usr in emailTo)
            {
                if(usr.IdeaState==1 || usr.IdeaState==5)
                {
                    // Send Email to Submitter 
                    EmailTemplate emailTemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaCreatedSubmitter.ToString());
                    emailObj.SendEmail(usr.EmailAddress, emailTemp.Body, emailTemp.Subject);
                }
             
                if(usr.IdeaState==0)
                {
                    EmailTemplate emailTemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaAssignedReviewer.ToString());
                    emailObj.SendEmail(usr.EmailAddress, emailTemp.Body, emailTemp.Subject);
                }
            }
        }

        public RestAPIAddIntellectualResponse SubmitIntellectual( InMemoryMultipartFormDataStreamProvider provider, int UserID)
        {
            RestAPIAddIntellectualResponse intellectualResponse = new RestAPIAddIntellectualResponse() { Status=Success};
 
            SubmitIntellectualRequest submitIntellectual = provider.SubmitIntellectualrequest;
            submitIntellectual.UserID = UserID;

            try
            {
                if (provider.attachFileCount != 0)
                {
                    submitIntellectual.IsAttachment = true;
                    submitIntellectual.AttachmentCount = provider.attachFileCount;
                }

                ideautils.SubmitIdeaIntellectual(intellectualResponse, submitIntellectual);

                if (intellectualResponse.ErrorList.Count != 0)
                    intellectualResponse.Status = Failure;

                if (provider.SubmitIntellectualrequest.files.Count == 0)
                    return intellectualResponse;


                fileUtil.UploadIntellectualAttachmentAsync(intellectualResponse, provider);

                if (intellectualResponse.ErrorList.Count != 0)
                    intellectualResponse.Status = Failure;
            }
            catch (Exception ex)
            {
                intellectualResponse.Status = Failure;
                throw new Exception(ex.Message, ex);
            }
             
            return intellectualResponse;
        }

        public void UpdateIdeaDraft(RestAPIUpdateDraftResponse response, RestAPIUpdateIdeaDraftRequest request, int UserID)
        {
            try
            {
                ideautils.UpdateIdeaDraft(response, request, UserID);

                if (!request.IsDraft)
                {
                    ideautils.SubmitIdeaAssignment(response, request.IdeaId);
                    SendEmail(request.IdeaId);
                }
            }
            catch (Exception ex)
            {
                response.Status = Failure;
                throw new Exception(ex.Message, ex);
            }
        }

        public RestAPIUpdateIntellectResponse UpdateIntellectual(InMemoryMultipartFormDataStreamProvider provider, int UserID)
        {
            RestAPIUpdateIntellectResponse updateIntellectualResponse = new RestAPIUpdateIntellectResponse() { Status = Success };

            UpdateIntellectualRequest updateIntellectual = provider.UpdateIntellectualRequest;
            updateIntellectual.UserID = UserID;

            try
            {
                if (provider.attachFileCount != 0)
                {
                    updateIntellectual.IsAttachment = true;
                    updateIntellectual.AttachmentCount = provider.attachFileCount;
                }

                ideautils.UpdateIntellectProperty(updateIntellectualResponse, updateIntellectual);

                if (updateIntellectualResponse.ErrorList.Count != 0)
                    updateIntellectualResponse.Status = Failure;

                if (provider.UpdateIntellectualRequest.files.Count == 0)
                    return updateIntellectualResponse;


                fileUtil.UploadIntellectualAttachmentAsync(updateIntellectualResponse, provider);

                if (updateIntellectualResponse.ErrorList.Count != 0)
                    updateIntellectualResponse.Status = Failure;
            }
            catch (Exception ex)
            {
                updateIntellectualResponse.Status = Failure;
                throw new Exception(ex.Message, ex);
            }

            return updateIntellectualResponse;
        }

        public void GetIdeaAttachments(RestAPIGetAttachmentsResponse response, int IdeaId)
        {
            List<RESTAPIIdeaAttachmentsInterchange> attachmentInterchangeList = null;
            List<IdeaAttachment> attachmentlist = null;

            DatabaseWrapper.databaseOperation(response,
                         (context, query) =>
                         {
                             attachmentInterchangeList = new List<RESTAPIIdeaAttachmentsInterchange>();
                             attachmentlist = new List<IdeaAttachment>();

                             attachmentlist = query.GetIdeaAttachmentsByIdeaId(context, IdeaId);
                             if (attachmentlist.Count > 0)
                             {
                                 foreach (var attachment in attachmentlist)
                                 {
                                     if (attachment.FolderName != Enum.GetName(typeof(folderNames), folderNames.DefaultImage))
                                     {
                                         RESTAPIIdeaAttachmentsInterchange commentInterchange = new RESTAPIIdeaAttachmentsInterchange(attachment);
                                         attachmentInterchangeList.Add(commentInterchange);
                                     }
                                 }
                             }
                             response.Status = Success;
                         }
                         , readOnly: true
                     );

            if (attachmentInterchangeList != null && attachmentInterchangeList.Count > 0)
                response.IdeaAttachmentList.AddRange(attachmentInterchangeList);
        }
    }

}