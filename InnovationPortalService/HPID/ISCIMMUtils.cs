namespace InnovationPortalService.HPID
{
    public interface ISCIMMUtils
    {
        void AddAttribute<T>(string path, T value);
        void ReplaceAttribute<T>(string path, T value);
        void RemoveAttribute(string path);
        string GetJson();
        bool IsHPIDCustomerProfileUpdateRequired(CustomerData newdata, HPIDCustomerProfile source, bool SupportUpdateToProfileDetails);
    }
}