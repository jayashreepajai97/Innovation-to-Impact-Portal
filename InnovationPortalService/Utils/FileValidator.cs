using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

// Third party libraries for file content validations
using Spire.Pdf.Exceptions;
using Spire.Pdf;
using Spire.Doc;

namespace InnovationPortalService.Utils
{
    #region Class FileOperation
    public class FileOperation
    {
        public static void CreateDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        public static bool IsDirectoryExist(string directoryName)
        {
            return Directory.Exists(directoryName);
        }
    }
    #endregion Class FileOperation

    #region Class FileValidator
    public class FileValidator
    {
        #region Protected Methods
        /// <summary>
        /// This class snould not be instantiated out of the hierarchy chain. 
        /// It serves only as a base for other specific file validators.
        /// </summary>
        protected FileValidator()
        {
        }

        protected string ExtractFileExtension(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            fileExtension = fileExtension?.TrimStart('.').ToLower();
            return fileExtension;
        }

        protected MemoryStream GetDataStream(string fileContent)
        {
            MemoryStream dataStream = new MemoryStream(Convert.FromBase64String(fileContent));
            return dataStream;
        }
        #endregion Protected Methods

        /// <summary>
        /// Valid attachment file types supported in CDAX API request
        /// </summary>
        public enum FileType
        {
            None    = 0,
            Txt     = 1,
            Img     = 2,
            Pdf     = 3,
            Word    = 4,
        }

        public static IFileValidator GetInstance(FileValidator.FileType fileType)
        {
            IFileValidator fileValidator = null;
            switch (fileType)
            {
                case FileValidator.FileType.Txt:
                    fileValidator = new TextFileValidator();
                    break;
                case FileValidator.FileType.Img:
                    fileValidator = new ImageFileValidator();
                    break;
                case FileValidator.FileType.Pdf:
                    fileValidator = new PdfFileValidator();
                    break;
                case FileValidator.FileType.Word:
                    fileValidator = new WordFileValidator();
                    break;
                case FileValidator.FileType.None:
                    break;
            }
            return fileValidator;
        }

        public static FileType GetFileType(string mimeType)
        {
            FileType fileType = FileType.None;

            string[] tokens = mimeType.Split('/');

            if (tokens.Length != 2)
            {
                return fileType;
            }

            string type = tokens[0];
            string subType = tokens[1];

            if (type == "text"
                && subType == "plain")
            {
                fileType = FileType.Txt;
            }
            else if (type == "image"
                && (subType == "jpg" || subType == "jpeg"))
            {
                fileType = FileType.Img;
            }
            else if (type == "application")
            {
                if (subType == "pdf")
                {
                    fileType = FileType.Pdf;
                }
                if (subType == "msword" || subType == "vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    fileType = FileType.Word;
                }
            }
            return fileType;
        }
    }
    #endregion Class FileValidator

    #region Class TextFileValidator
    public class TextFileValidator : FileValidator, IFileValidator
    {
        public bool IsValidFileExtension(string fileName)
        {
            string fileExtension = ExtractFileExtension(fileName);
            return (fileExtension == "txt") ? true : false;
        }

        public bool IsValidFileContent(string fileContent)
        {
            return true;
        }
    }
    #endregion Class TextFileValidator

    #region Class ImageFileValidator
    public class ImageFileValidator : FileValidator, IFileValidator
    {
        public bool IsValidFileExtension(string fileName)
        {
            string fileExtension = ExtractFileExtension(fileName);
            return (fileExtension == "jpg" || fileExtension == "jpeg") 
                ? true : false;
        }

        public bool IsValidFileContent(string fileContent)
        {
            Image image;
            try
            {
                MemoryStream dataStream = GetDataStream(fileContent);
                image = Image.FromStream(dataStream);
            }
            catch (Exception)
            {
                return false;
            }
            if (image == null)
            {
                return false;
            }
            if (ImageFormat.Jpeg.Equals(image.RawFormat))
            {
                return true;
            }
            return false;
        }
    }
    #endregion Class ImageFileValidator

    #region Class PdfFileValidator
    public class PdfFileValidator : FileValidator, IFileValidator
    {
        public bool IsValidFileExtension(string fileName)
        {
            string fileExtension = ExtractFileExtension(fileName);
            return (fileExtension == "pdf") ? true : false;
        }

        public bool IsValidFileContent(string fileContent)
        {
            try
            {
                MemoryStream dataStream = GetDataStream(fileContent);
                PdfDocument doc = new PdfDocument(dataStream);
            }
            catch (PdfDocumentException)
            {
                // PdfDocumentException is thrown if the input data 
                // stream does not contain valid pdf file data
                return false;
            }
            catch (Exception)
            {
                // Return false in case of any other exception as well
                return false;
            }
            return true;

        }
    }
    #endregion Class PdfFileValidator

    #region Class WordFileValidator
    public class WordFileValidator : FileValidator, IFileValidator
    {
        private bool IsOpenSuccessful(MemoryStream dataStream, Spire.Doc.FileFormat format)
        {
            try
            {
                Document doc = new Document(dataStream, format);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsValidFileExtension(string fileName)
        {
            string fileExtension = ExtractFileExtension(fileName);
            return (fileExtension == "doc" || fileExtension == "docx") ? true : false;
        }

        public bool IsValidFileContent(string fileContent)
        {
            MemoryStream dataStream = GetDataStream(fileContent);
            // Microsoft Word 97 - 2003 Format
            bool status = IsOpenSuccessful(dataStream, Spire.Doc.FileFormat.Doc);
            // Microsoft Word 2007 Format
            status = (status == false) ? IsOpenSuccessful(dataStream, Spire.Doc.FileFormat.Docx) : status;
            // Microsoft Word 2010 Format
            status = (status == false) ? IsOpenSuccessful(dataStream, Spire.Doc.FileFormat.Docx2010) : status;
            // Microsoft Word 2013 Format
            status = (status == false) ? IsOpenSuccessful(dataStream, Spire.Doc.FileFormat.Docm2013) : status;

            return status;
        }
    }
    #endregion Class WordFileValidator
}