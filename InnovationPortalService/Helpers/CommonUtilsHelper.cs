using Newtonsoft.Json.Linq;
using InnovationPortalService.Utils;

namespace InnovationPortalService.Helpers
{
    public static class CommonUtilsHelper
    {
        #region Fields
        private static string AccountLinkNodeName = "AccountLink";
        private static string AccountNodeName = "Account";
        #endregion Fields

        #region Internal Methods
        internal static int ExtractCustomerIdFromRequest(JObject requestJson)
        {
            int customerIdFromRequest = 0;
            string customerIdStr = null;
            customerIdStr = ExtractFieldValuefromNode(requestJson, "UserID");
            int.TryParse(customerIdStr, out customerIdFromRequest);
            return customerIdFromRequest;
        }

        internal static string ExtractFieldValuefromNode(JObject requestJson, string fieldName)
        {
            string fieldVal;
            JObject accountData = JsonUtils.ExtractField(requestJson, new string[] { AccountLinkNodeName });

            if (accountData.HasValues == true)
            {
                fieldVal = accountData?.SelectToken($"{AccountLinkNodeName}.{fieldName}")?.Value<string>();
            }
            else
            {
                accountData = JsonUtils.ExtractField(requestJson, new string[] { AccountNodeName });
                fieldVal = accountData?.SelectToken($"{AccountNodeName}.{fieldName}")?.Value<string>();
            }

            return fieldVal;
        }
        #endregion Internal Methods
    }
}