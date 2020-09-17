using Hpcs.DependencyInjector;
using IdeaDatabase.DataContext;
using IdeaDatabase.Requests;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils;
using IdeaDatabase.Utils.IImplementation;
using IdeaDatabase.Utils.Interface;
using NLog;
using Responses;
using SettingsRepository;
using System;
using System.IO;
using System.Linq;

namespace InnovationPortalService.Utils
{
    public class FileUtil : IFileUtil
    {
        private static Logger logger = LogManager.GetLogger("InnovationPortalServiceLog");
        private static bool IsS3Enable { get; } = SettingRepository.Get<bool>("IsS3Enabled");
        private string DefaultDateFolder { get; } = DateTime.UtcNow.ToString("yyyyMMdd");
        ResponseBase response = new ResponseBase();


        private ISubmitIdeaUtil ideautils = DependencyInjector.Get<ISubmitIdeaUtil, SubmitIdeaUtil>();
        private IAWSS3Util aWSS3Util = DependencyInjector.Get<IAWSS3Util, AWSS3Util>();


        private static string ReplaceStringToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }
            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }

        public void UploadAttachmentAsync(InMemoryMultipartFormDataStreamProvider dataStreamProvider, ResponseBase responseBase, int userID, string defaultPath)
        {
            ResponseBase submitIdeaAttachmentResponse = new ResponseBase();
            RestAPIIdeaSupportingRequest  ideaSupportingRequest = dataStreamProvider.restAPIIdeaSupportingRequest;
            ideaSupportingRequest.ideaAttachments.ToList().ForEach((ideaAttachment) =>
            {
                ideaAttachment.IdeaId = ideaSupportingRequest.IdeaId;
            });
            foreach (IdeaAttachmentRequest ideaAttachmentRequest in ideaSupportingRequest.ideaAttachments)
            {
                string inputFileName = ideaAttachmentRequest.AttachedFileName;
                string folderpath = UploadPath(ideaAttachmentRequest, ideaSupportingRequest.IdeaId);
                logger.Info($"Folder path={folderpath}");
                string filepath = Path.Combine(folderpath, inputFileName);
                if (IsS3Enable)
                {
                    UploadRequestToS3(folderpath, filepath, ideaAttachmentRequest.stream);
                }
                else
                {
                    SaveAttachments(ideaAttachmentRequest.stream, inputFileName, folderpath);
                }

            }

        }

        private void UploadRequestToS3(string folderpath, string filepath, Stream inputs)
        {
            filepath = filepath.Replace("\\", "/");//key name
            folderpath = folderpath.Replace("\\", "/"); // folderPath
            aWSS3Util.UploadObject(inputs, filepath, folderpath);
        }

        public void UploadIntellectualAttachmentAsync(RestAPIAddIntellectualResponse IntellectualAttachmentResponse, InMemoryMultipartFormDataStreamProvider dataStreamProvider)
        {
            SubmitIntellectualRequest submitIntellectual = dataStreamProvider.SubmitIntellectualrequest;
                dataStreamProvider.SubmitIntellectualrequest.ideaAttachments.ToList().ForEach((ideaAttachment) =>
                {
                    ideaAttachment.IdeaId = submitIntellectual.IdeaId;                    
                });
           

            foreach (IdeaAttachmentRequest ideaAttachmentRequest in submitIntellectual.ideaAttachments)
            {
                string inputFileName = ideaAttachmentRequest.AttachedFileName;
                string folderpath = UploadPath(ideaAttachmentRequest,submitIntellectual.IdeaId);
                string filepath = Path.Combine(folderpath, inputFileName);
                logger.Info($"IntellectualAttachmentResponse==Folder path={folderpath}");
                
                
                if (IsS3Enable)
                {
                    UploadRequestToS3(folderpath, filepath, ideaAttachmentRequest.stream);
                }
                else
                {
                    SaveAttachments(ideaAttachmentRequest.stream, inputFileName, folderpath);
                }
            }
        }

        private void SaveAttachments(Stream inputs,string inputFileName, string folderpath) {

            logger.Info($"Folder path={folderpath}");
            FileInfo fileInfo = new FileInfo(inputFileName);

            if (!FileOperation.IsDirectoryExist(folderpath))
            {
                FileOperation.CreateDirectory(folderpath);
            }
            string filepath = Path.Combine(folderpath, inputFileName);

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            using (Stream file = File.OpenWrite(filepath))
            {
                inputs.CopyTo(file);
                file.Close();
            }
        }

        private string UploadPath( IdeaAttachmentRequest ideaAttachmentRequest, int? IdeaId )
        {
            string folderpath = string.Empty;
           
            return folderpath = Path.Combine(ideaAttachmentRequest.DefaultFolder,IdeaId.ToString(), ideaAttachmentRequest.DocumentTypeFolderName);
            
        }

        public void UploadIntellectualAttachmentAsync(RestAPIUpdateIntellectResponse IntellectualAttachmentResponse, InMemoryMultipartFormDataStreamProvider dataStreamProvider)
        {
            IdeaIntellectualProperty intellectualProperty = null;
            UpdateIntellectualRequest updateIntellectual = dataStreamProvider.UpdateIntellectualRequest;
            dataStreamProvider.UpdateIntellectualRequest.ideaAttachments.ToList().ForEach((ideaAttachment) =>
            {
                ideaAttachment.IdeaId = updateIntellectual.IdeaId;
            });

            DatabaseWrapper.databaseOperation(response,
             (context, query) =>
             {
                 intellectualProperty = query.GetIdeaIntellectualById(context, updateIntellectual.IntellectualId);
             }
              , readOnly: true
            );

            foreach (IdeaAttachmentRequest ideaAttachmentRequest in updateIntellectual.ideaAttachments)
            {
                string inputFileName = ideaAttachmentRequest.AttachedFileName;
                string folderpath = UploadPath(ideaAttachmentRequest, intellectualProperty.IdeaId);
                string filepath = Path.Combine(folderpath, inputFileName);
                logger.Info($"IntellectualAttachmentResponse==Folder path={folderpath}");


                if (IsS3Enable)
                {
                    UploadRequestToS3(folderpath, filepath, ideaAttachmentRequest.stream);
                }
                else
                {
                    SaveAttachments(ideaAttachmentRequest.stream, inputFileName, folderpath);
                }
            }
        }
    }
}