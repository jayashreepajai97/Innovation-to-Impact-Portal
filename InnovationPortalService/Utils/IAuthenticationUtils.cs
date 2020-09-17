namespace InnovationPortalService.Utils
{
    public interface IAuthenticationUtils
    {       
        string GetProfileIdByEmail(string emailAddress);
        string GetProfileIdByUserId(string userId);

        
    }
}