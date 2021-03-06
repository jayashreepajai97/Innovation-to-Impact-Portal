﻿using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using IdeaDatabase.Requests;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using Hpcs.DependencyInjector;
using InnovationPortalService.Filters;
using InnovationPortalService.Utils;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using InnovationPortalService.Requests;

namespace InnovationPortalService.Controllers
{
    [HPIDEnable]
    public class RESTAPIIdeaController : RESTAPIControllerBase
    {

        private readonly IStatusUtils statusUtils = DependencyInjector.Get<IStatusUtils, StatusUtils>();
        private readonly ISubmitIdeaUtil submitIdeaUtil = DependencyInjector.Get<ISubmitIdeaUtil, SubmitIdeaUtil>();
        private readonly IIdeaUtil ideaUtil = DependencyInjector.Get<IIdeaUtil, IdeaUtil>();
        private readonly IMetricsUtils metricsUtil = DependencyInjector.Get<IMetricsUtils, MetricsUtils>();
        private readonly ILogUtils logUtil = DependencyInjector.Get<ILogUtils, LogUtils>();


        /// <summary>
        /// This API is used for adding a new Idea.
        /// </summary>
        /// <param name="req">Request must required the parameter IdeaID and CommentDescription and In Authorization  Field must requird SessionToken and CallerID which is generated by Authenticate API.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPISubmitIdeaResponse</returns>
        [HttpPost]
        [Route("ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPISubmitIdeaResponse InsertIdea(RestAPISubmitIdeaRequest req)
        {
            RestAPISubmitIdeaResponse response = new RestAPISubmitIdeaResponse();
            ideaUtil.SubmitIdeas(response, req, UserID);

            return response;
        }

        /// <summary>
        /// This API is used for adding a new Idea Attachment.
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>SubmitIdeaAttachmentResponse</returns>
        [HttpPost]
        [Route("ideas/attachment")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBody]
        public async Task<SubmitIdeaAttachmentResponse> AddIdeaAttachments()
        {
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider(MimeRequestType.Idea)).ConfigureAwait(false);
            return ideaUtil.SubmitIdeaAttachment(provider, UserID);
        }

        /// <summary>
        /// This API is used for updating Idea stealth mode.
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIUpdateSensitiveResponse</returns>
        [HttpPut]
        [Route("ideas/private")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBody]
        public RestAPIUpdateSensitiveResponse UpdateSensitive(int IdeaId, [FromUri] bool isSensitive = false)
        {
            RestAPIUpdateSensitiveResponse response = new RestAPIUpdateSensitiveResponse();
            submitIdeaUtil.UpdateSensitive(response, UserID, IdeaId, isSensitive);
            logUtil.InsertIdeaLog(response, IdeaId, LogMessages.UpdateSensitive, (int)IdeaLogTypes.Info, Enum.GetName(typeof(IdeaMethodTypes), IdeaMethodTypes.UpdateSensitive), EnumDescriptor.GetEnumDescription(IdeaMethodTypes.UpdateSensitive), UserID);

            return response;
        }

        /// <summary>
        /// This API is used for updating the idea status.
        /// </summary>
        /// <param name="IdeaID">Request must required the parameter IdeaID and IdeaStatus</param>      
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIAddIdeaStateResponse</returns>
        [HttpPut]
        [Route("ideas/status")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBody]
        public RestAPIAddIdeaStateResponse UpdateIdeaStatus([FromUri] int IdeaID)
        {
            RestAPIAddIdeaStateResponse response = new RestAPIAddIdeaStateResponse();
            submitIdeaUtil.InsertIdeaStatus(response, IdeaID, UserID);
            logUtil.InsertIdeaLog(response, IdeaID, LogMessages.UpdateIdeaStatus, (int)IdeaLogTypes.Info, Enum.GetName(typeof(IdeaMethodTypes), IdeaMethodTypes.UpdateIdeaStatus), EnumDescriptor.GetEnumDescription(IdeaMethodTypes.UpdateIdeaStatus), UserID);

            if (response.ErrorList.Count == 0)
            {
                ISubmitIdeaUtil ideautils = DependencyInjector.Get<ISubmitIdeaUtil, SubmitIdeaUtil>();

                RESTAPIIdeaBasicDetailsInterchange ideaDetails = new RESTAPIIdeaBasicDetailsInterchange();

                ideaDetails = ideautils.GetIdeaBasicDetails(IdeaID);
                List<IdeaEmailToDetails> emailTo = submitIdeaUtil.GetAllStakeholdersEmailAdd(IdeaID);
                EmailUtil emailObj = new EmailUtil();

                // TODO: The below email logic is written here since there is no ISubmitIdeaUtil in InnovationPortalService, the database util is directly called, this needs to be changed in code refactoring.

                // Determine the current state of Idea: if the highest status is 2, it is currently reviewed if 3 it is sponsored
                if (emailTo[0].IdeaState == (int)IdeaStatusTypes.SponsorPending) 
                {
                    // This is an Idea Status update request for changing state from Submitted to Reviewed
                    foreach (var usr in emailTo)
                    {
                        if (usr.IdeaState == (int)IdeaStatusTypes.ReviewPending)
                        {
                            // Send Email to Submitter 
                            EmailTemplate emailTemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaReviewedSubmitter.ToString());
                            emailObj.SendEmail(usr.EmailAddress, emailTemp.Body, emailTemp.Subject);
                        }
                        if (usr.IdeaState == (int)IdeaStatusTypes.SponsorPending) 
                        {
                            // Send Email to Reviwer
                            EmailTemplate emailTemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaReviewedReviewer.ToString());
                            emailObj.SendEmail(usr.EmailAddress, emailTemp.Body, emailTemp.Subject);
                        }

                        if (usr.IdeaState == (int)IdeaStatusTypes.Default)
                        {
                            //The user in Assignment table should be Sponsor 
                            EmailTemplate emailTemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaAssignedSponsor.ToString());
                            emailObj.SendEmail(usr.EmailAddress, emailTemp.Body, emailTemp.Subject);
                        }
                    }

                }
                else if (emailTo[0].IdeaState == (int)IdeaStatusTypes.Sponsored)
                {
                    // This is an Idea Status update request for changing state from Reviewed to Sponsored
                    foreach (var usr in emailTo)
                    {
                        if (usr.IdeaState == (int)IdeaStatusTypes.ReviewPending)
                        {
                            // Send Email to Submitter 
                            EmailTemplate emailTemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaSponsoredSubmitter.ToString());
                            emailObj.SendEmail(usr.EmailAddress, emailTemp.Body, emailTemp.Subject);
                        }
                        if (usr.IdeaState == (int)IdeaStatusTypes.SponsorPending)
                        {
                            // Send Email to Reviewer
                            EmailTemplate emailTemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaSponsoredReviewer.ToString());
                            emailObj.SendEmail(usr.EmailAddress, emailTemp.Body, emailTemp.Subject);
                        }
                        if (usr.IdeaState == (int)IdeaStatusTypes.Sponsored)
                        {
                            // Send Email to Sponsor
                            EmailTemplate emailTemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaSponsoredSponsor.ToString());
                            emailObj.SendEmail(usr.EmailAddress, emailTemp.Body, emailTemp.Subject);
                        }
                        //if (usr.IdeaState == 0)
                        //{
                        //       //This user should be a Sales Guy. No action needs to be taken currently. This case is kept for future requirement.
                        //}
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// This API is used for Adding the Idea Comment.
        /// </summary>
        /// <param name="req">Request must required the parameter IdeaID and CommentDescription and In Authorization  Field must requird SessionToken and CallerID which is generated by Authenticate API.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIAddUserCommentResponse</returns>
        [HttpPost]
        [Route("ideas/comments")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIAddUserCommentResponse AddIdeaComment(RestAPIAddUserCommentRequest req)
        {
            RestAPIAddUserCommentResponse response = new RestAPIAddUserCommentResponse();
            logUtil.InsertIdeaLog(response, req.IdeaID, LogMessages.AddIdeaComment, (int)IdeaLogTypes.Info, Enum.GetName(typeof(IdeaMethodTypes), IdeaMethodTypes.AddIdeaComment), EnumDescriptor.GetEnumDescription(IdeaMethodTypes.AddIdeaComment), UserID);
            submitIdeaUtil.InsertIdeaComment(response, req.IdeaID, req.CommentDescription, UserID);

            return response;
        }

        /// <summary>
        /// This API is used for fetching User Comments for a specific Idea.
        /// </summary>
        /// <param name="IdeaId">Request must required  IdeaId an Authorization  Field must requird SessionToken and CallerID which is generated by Authenticate API.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetUserCommentsResponse</returns>
        [HttpGet]
        [Route("ideas/comments/{IdeaId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetUserCommentsResponse GetIdeaComments([FromUri] int IdeaId)
        {
            RestAPIGetUserCommentsResponse response = new RestAPIGetUserCommentsResponse();
            submitIdeaUtil.GetUserComments(response, IdeaId);

            return response;
        }

        /// <summary>
        /// This API is used for Deleting user Comment.
        /// </summary>
        /// <param name="IdeaCommentId">Request must required the parameter IdeaCommentId and In Authorization  Field must requird SessionToken and CallerID which is generated by Authenticate API</param>
        /// <returns>RestAPIDeleteIdeaResponse</returns>
        [HttpDelete]
        [Route("ideas/comments/{IdeaCommentId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBodyAttribute]
        public RestAPIDeleteIdeaResponse DeleteIdeaComment([FromUri] int IdeaCommentId)
        {
            RestAPIDeleteIdeaResponse response = new RestAPIDeleteIdeaResponse();
            submitIdeaUtil.DeleteIdeaComment(response, IdeaCommentId);

            return response;
        }

        /// <summary>
        /// This API is used for fetching list of Ideas.
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetUserIdeaResponse</returns>
        [HttpGet]
        [Route("ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetUserIdeaResponse GetIdeaList([FromUri] bool IsDraft = false)
        {
            RestAPIGetUserIdeaResponse response = new RestAPIGetUserIdeaResponse();
            submitIdeaUtil.GetIdeas(response, UserID, IsDraft);

            return response;
        }

        /// <summary>
        /// This API is used for fetching details of idea.
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RESTGetUserIdeaDetailsResponse</returns>
        [HttpGet]
        [Route("ideas/details/{IdeaId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RESTGetUserIdeaDetailsResponse GetDetails([FromUri] int IdeaId)
        {
            RESTGetUserIdeaDetailsResponse response = new RESTGetUserIdeaDetailsResponse();
            submitIdeaUtil.GetIdeasDetails(response, IdeaId);

            return response;
        }

        /// <summary>
        /// This API is used for fetching reviewers Ideas List .
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetUserIdeaStatusResponse</returns>
        [HttpGet]
        [Route("reviewers/ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetUserIdeaStatusResponse GetIdeaReviewsList()
        {
            RestAPIGetUserIdeaStatusResponse response = new RestAPIGetUserIdeaStatusResponse();
            submitIdeaUtil.GetIdeaReviewsList(response, UserID);

            return response;
        }

        /// <summary>
        /// This API is used for fetching sponsors Ideas List .
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetUserIdeaStatusResponse</returns>
        [HttpGet]
        [Route("sponsors/ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetUserIdeaStatusResponse GetIdeaSponsorsList()
        {
            RestAPIGetUserIdeaStatusResponse response = new RestAPIGetUserIdeaStatusResponse();
            submitIdeaUtil.GetIdeaSponsorsList(response, UserID);

            return response;
        }

        /// <summary>
        /// This API is used for fetching list of public ideas .
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetUserIdeaResponse</returns>
        [HttpGet]
        [Route("ideas/public")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetUserIdeaResponse GetPublicList([FromUri] int CategoryId = 0, [FromUri] bool Sort = false)
        {
            RestAPIGetUserIdeaResponse response = new RestAPIGetUserIdeaResponse();
            submitIdeaUtil.GetPublicIdeas(response, UserID, CategoryId, Sort);

            return response;
        }

        /// <summary>
        /// This API is used for Adding reply to idea comment.
        /// </summary>
        /// <param name="req">Request must required the parameter IdeaID and CommentDescription and In Authorization  Field must requird SessionToken and CallerID which is generated by Authenticate API.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIAddCommentReplyResponse</returns>
        [HttpPost]
        [Route("ideas/comments/reply")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIAddCommentReplyResponse AddIdeaCommentReply(RestAPIAddCommentReplyRequest req)
        {
            RestAPIAddCommentReplyResponse response = new RestAPIAddCommentReplyResponse();
            submitIdeaUtil.InsertCommentReply(response, req.IdeaCommentID, UserID, req.DiscussionDescription);
            logUtil.InsertIdeaLog(response, req.IdeaCommentID, LogMessages.AddIdeaCommentReply, (int)IdeaLogTypes.Info, Enum.GetName(typeof(IdeaMethodTypes), IdeaMethodTypes.AddIdeaCommentReply), EnumDescriptor.GetEnumDescription(IdeaMethodTypes.AddIdeaCommentReply), UserID);

            return response;
        }

        /// <summary>
        ///  This API is used for searching Idea.
        /// </summary>
        /// <param name="SearchText">Request must required the parameters.</param>
        /// <returns>RestAPISearchIdeaResponse</returns>
        [HttpGet]
        [Route("ideas/search")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPISearchIdeaResponse SearchIdea([FromUri] string SearchText)
        {
            RestAPISearchIdeaResponse response = new RestAPISearchIdeaResponse();
            submitIdeaUtil.SearchIdea(response, SearchText, UserID);

            return response;
        }

        /// <summary>
        /// This API is used for updating the idea details.
        /// </summary>
        /// <param name="IdeaId">Request must required the parameter IdeaID</param>
        ///  <param name="req">Request must required the parameter  IdeaRequest </param>     
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIUpdateIdeaResponse</returns>
        [HttpPut]
        [Route("ideas/details/{IdeaId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIUpdateIdeaResponse UpdateDetails(RestAPIUpdateIdeaRequest req, [FromUri] int IdeaId)
        {
            RestAPIUpdateIdeaResponse response = new RestAPIUpdateIdeaResponse();
            submitIdeaUtil.UpdateIdea(response, UserID, IdeaId, req);
            logUtil.InsertIdeaLog(response, IdeaId, LogMessages.UpdateDetails, (int)IdeaLogTypes.Info, Enum.GetName(typeof(IdeaMethodTypes), IdeaMethodTypes.UpdateDetails), EnumDescriptor.GetEnumDescription(IdeaMethodTypes.UpdateDetails), UserID);

            return response;
        }

        /// <summary>
        ///  This API is used for sending an email notifications.
        /// </summary>
        /// <param name="EmailAddress">Request must required the parameters.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("email/{EmailAddress}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        //[CredentialsHeader]
        public HttpResponseMessage SendEmail([FromUri]string EmailAddress)
        {
            EmailUtil emailObj = new EmailUtil();
            //var ideaBasicDetails = userUtil.GetUserEmail(UserID);
            EmailTemplate emailtemp = emailObj.GetEmailTemplate(EmailTemplateType.IdeaCreatedSubmitter.ToString());

            return emailObj.SendEmail(EmailAddress, emailtemp.Body, "HP Innovation Portal Email testing");
        }

        /// <summary>
        /// This API is used for Adding the intellectual property for Idea.
        /// </summary>
        /// <param >Request must required the FormData.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIAddIntellectualResponse</returns>
        [HttpPost]
        [Route("ideas/intellectual")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBody]
        public async Task<RestAPIAddIntellectualResponse> AddIntellectualProperty()
        {
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider(MimeRequestType.IntelectualProperty)).ConfigureAwait(false);
            return ideaUtil.SubmitIntellectual(provider, UserID);
        }

        /// <summary>
        /// This API is used for Deleting Intellectual Idea.
        /// </summary>
        /// <param name="IntellectualId">Request must required the parameter IntellectualId and In Authorization  Field must requird SessionToken and CallerID which is generated by Authenticate API</param>
        /// <returns>RestAPIDeleteIntellectResponse</returns>
        [HttpDelete]
        [Route("ideas/intellectual")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBodyAttribute]
        public RestAPIDeleteIntellectResponse DeleteIntellectualProperty([FromUri] int IntellectualId)
        {
            RestAPIDeleteIntellectResponse response = new RestAPIDeleteIntellectResponse();
            submitIdeaUtil.DeleteIntellectProperty(response, IntellectualId);

            return response;
        }

        /// <summary>
        /// This API is used for updating the idea intellectual property details.
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIUpdateIntellectResponse</returns>
        [HttpPut]
        [Route("ideas/intellectual")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBody]
        public async Task<RestAPIUpdateIntellectResponse> UpdateIntellectualProperty()
        {
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider(MimeRequestType.UpdateIntellectualProperty)).ConfigureAwait(false);
            return ideaUtil.UpdateIntellectual(provider, UserID);
        }

        /// <summary>
        /// This API is used for fetching Intellectual property details of idea.
        /// </summary>
        /// <param name="IntellectualId">Request must required the parameter IntellectualId</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetIntellectualResponse</returns>
        [HttpGet]
        [Route("ideas/intellectual/{IntellectualId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetIntellectualResponse GetIntellectualProperty([FromUri] int IntellectualId)
        {
            RestAPIGetIntellectualResponse response = new RestAPIGetIntellectualResponse();
            submitIdeaUtil.GetIntellectualProperties(response, IntellectualId);

            return response;
        }

        /// <summary>
        /// This API is used for updating the IsDraft flag of existing Idea.
        /// </summary>
        /// <param name="req">Request must required the parameter IdeaID and CommentDescription and In Authorization Field must requird SessionToken and CallerID which is generated by Authenticate API.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIUpdateDraftResponse</returns>
        [HttpPatch]
        [Route("ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBodyAttribute]
        public RestAPIUpdateDraftResponse UpdateIdeaDraft(RestAPIUpdateIdeaDraftRequest req)
        {
            RestAPIUpdateDraftResponse response = new RestAPIUpdateDraftResponse();
            ideaUtil.UpdateIdeaDraft(response, req, UserID);

            return response;
        }

        /// <summary>
        /// This API is used for Deleting IdeaAttachment.
        /// </summary>
        /// <param name="IdeaAttachmentId">Request must required the parameter IdeaAttachmentId and In Authorization Field must requird SessionToken and CallerID which is generated by Authenticate API</param>
        /// <returns>RestAPIDeleteAttachmentResponse</returns>
        [HttpDelete]
        [Route("ideas/attachments/{IdeaAttachmentId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        [AllowEmptyBody]
        public RestAPIDeleteAttachmentResponse DeleteIdeaAttachment([FromUri] int IdeaAttachmentId)
        {
            RestAPIDeleteAttachmentResponse response = new RestAPIDeleteAttachmentResponse();
            submitIdeaUtil.DeleteIdeaAttachment(response, IdeaAttachmentId);

            return response;
        }

        /// <summary>
        /// This API is used for fetching Ideas metrics.
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetIdeaMetricsResponse</returns>
        [HttpGet]
        [Route("ideas/metrics")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetIdeaMetricsResponse GetIdeaMetrics([FromUri] int IdeaId)
        {
            RestAPIGetIdeaMetricsResponse response = new RestAPIGetIdeaMetricsResponse();
            metricsUtil.GetIdeasMetrics(response, IdeaId);

            return response;
        }

        /// <summary>
        /// This API is used for Idea Archive.
        /// </summary>
        /// <param name="req">Request must required the parameter IdeaID and CommentDescription and In Authorization Field must requird SessionToken and CallerID which is generated by Authenticate API.</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIArchiveIdeaResponse</returns>
        [HttpPost]
        [Route("ideas/archive")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIArchiveIdeaResponse ArchiveIdea(RestAPIArchiveIdeaRequest req)
        {
            RestAPIArchiveIdeaResponse response = new RestAPIArchiveIdeaResponse();
            submitIdeaUtil.ArchiveIdea(response, UserID, req.IdeaId, req.Description, req.IsArchive);

            return response;
        }

        /// <summary>
        /// This API is used for fetching list of Archived Ideas.
        /// </summary>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetArchiveIdeaResponse</returns>
        [HttpGet]
        [Route("ideas/archive")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetArchiveIdeaResponse GetArchivedIdeaList()
        {
            RestAPIGetArchiveIdeaResponse response = new RestAPIGetArchiveIdeaResponse();
            submitIdeaUtil.GetArchiveIdeas(response, UserID);

            return response;
        }

        /// <summary>
        /// This API is used for fetching attachments list of idea.
        /// </summary>
        /// <param name="IdeaId">Request must required the parameter IdeaId</param>
        /// <response code="200">Successfully processed - Check ErrorList for possible errors</response>
        /// <response code="400">Invalid JSON - Syntax error in request body</response>
        /// <returns>RestAPIGetAttachmentsResponse</returns>
        [HttpGet]
        [Route("ideas/attachments/{IdeaId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [CredentialsHeader]
        public RestAPIGetAttachmentsResponse GetIdeaAttachments([FromUri] int IdeaId)
        {
            RestAPIGetAttachmentsResponse response = new RestAPIGetAttachmentsResponse();
            ideaUtil.GetIdeaAttachments(response, IdeaId);

            return response;
        }

        [HttpOptions]
        [Route("ideas/status")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void UpdateIdeaStatus() { }

        [HttpOptions]
        [Route("ideas/comments")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void AddIdeaComment() { }

        [HttpOptions]
        [Route("ideas/comments/{IdeaId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetIdeaComments() { }

        [HttpOptions]
        [Route("ideas/comments/{IdeaCommentId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void DeleteIdeaComment() { }

        [HttpOptions]
        [Route("ideas/private")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void UpdateSensitive() { }

        [HttpOptions]
        [Route("ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetideaList() { }

        [HttpOptions]
        [Route("ideas/public")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetPublicList() { }

        [HttpOptions]
        [Route("ideas/comments/reply")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void AddIdeaCommentReply() { }

        [HttpOptions]
        [Route("ideas/details/{IdeaId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetDetails() { }

        [HttpOptions]
        [Route("ideas/search")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void SearchIdea() { }

        [HttpOptions]
        [Route("reviewers/ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetIdeareviewsList() { }

        [HttpOptions]
        [Route("sponsors/ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetIdeasponsorsList() { }

        [HttpOptions]
        [Route("email/{EmailAddress}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void SendEmail() { }

        [HttpOptions]
        [Route("ideas/details/{IdeaId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void UpdateDetails() { }

        [HttpOptions]
        [Route("ideas/intellectual")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void AddintellectualProperty() { }

        [HttpOptions]
        [Route("ideas/intellectual")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void DeleteIntellectualProperty() { }

        [HttpOptions]
        [Route("ideas/intellectual")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void UpdateintellectualProperty() { }

        [HttpOptions]
        [Route("ideas")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void Insertidea() { }

        [HttpOptions]
        [Route("ideas/intellectual/{IntellectualId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetIntellectualProperty() { }

        [HttpOptions]
        [Route("ideas/attachments/{IdeaAttachmentId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void DeleteIdeaAttachment() { }

        [HttpOptions]
        [Route("ideas/metrics")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetIdeaMetrics() { }

        [HttpOptions]
        [Route("ideas/archive")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetArchivedIdealist() { }

        [HttpOptions]
        [Route("ideas/attachments/{IdeaId}")]
        [SwaggerOperation(Tags = new[] { "Idea" })]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void GetIdeaAttachments() { }
    }
}


