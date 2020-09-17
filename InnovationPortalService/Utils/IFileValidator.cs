namespace InnovationPortalService.Utils
{
    public interface IFileValidator
    {
        bool IsValidFileExtension(string fileName);
        bool IsValidFileContent(string fileContent);
    }
}
