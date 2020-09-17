using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using IdeaDatabase.Interchange;
using IdeaDatabase.Utils.Interface;
using SettingsRepository;
using System;
using System.Diagnostics;
using System.IO;

namespace IdeaDatabase.Utils.IImplementation
{
    public class AWSS3Util : IAWSS3Util
    {
        private string bucketName = null;
        private string AccessKey = null;
        private string SecretKey = null;
        private string DefaultImagePath = null;
        private IAmazonS3 amazonS3Client = null;
        bool isValid = false;

        public AWSS3Util()
        {
            LoadConfigFromDb();
            if (amazonS3Client == null)
            {               
                AmazonS3Config amazonS3Config = new AmazonS3Config();
                amazonS3Config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
                amazonS3Client = AWSClientFactory.CreateAmazonS3Client(AccessKey, SecretKey, amazonS3Config);
            }
        }

        private void LoadConfigFromDb()
        {
            AccessKey = SettingRepository.Get<string>("AWSKey");
            SecretKey = SettingRepository.Get<string>("AWSSecret");
            bucketName = SettingRepository.Get<string>("AWSBucketName");
            DefaultImagePath = SettingRepository.Get<string>("AWSDefaultImageURL");
        }

        public string GenerateURL(string KeyName)
        {
            string urlString = string.Empty;
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = KeyName
            };

            urlString = this.amazonS3Client.GetPreSignedURL(request);
            return urlString;

        }

        public void UploadObject(Stream stream, string keyName, string folderPath)
        {
            try
            {
                S3DirectoryInfo s3DirectoryInfo = new S3DirectoryInfo(this.amazonS3Client, bucketName, folderPath);
                isValid = s3DirectoryInfo.Name.Contains(Enum.GetName(typeof(folderNames),folderNames.DefaultImage));
                if(isValid)
                {                    
                    s3DirectoryInfo.Delete(true); // true will delete recursively in folder inside               
                }
                TransferUtility fileTransferUtility = new TransferUtility(this.amazonS3Client);
                fileTransferUtility.Upload(stream, bucketName, keyName);
            }
            catch (AmazonS3Exception ex)
            {
               throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}