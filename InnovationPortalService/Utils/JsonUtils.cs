using Newtonsoft.Json.Linq;
using System.Linq;

namespace InnovationPortalService.Utils
{
    public static class JsonUtils
    {
        /// <summary>
        /// Returns a new instance of the Json object without the given fields
        /// </summary>
        /// <param name="jsonObject">Json object to remove the fields from</param>
        /// <param name="fields">List of fields to remove from the Json object</param>
        /// <returns></returns>
        public static JObject RemoveField(JObject jsonObject, string[] fields)
        {
            JObject modifiedObject = new JObject(jsonObject);
            if (fields == null)
            {
                return modifiedObject;
            }
            foreach (string field in fields)
            {
                JToken token = modifiedObject.SelectToken(field);
                if (token != null)
                {
                    token.Parent.Remove();
                }
            }
            return modifiedObject;
        }

        /// <summary>
        /// Returns a Json object with the selected fields extracted from the given Json
        /// </summary>
        /// <param name="jsonObject">Json object to extract the fields from</param>
        /// <param name="fields">Fields to extract from the Json object</param>
        /// <returns></returns>
        public static JObject ExtractField(JObject jsonObject, string[] fields)
        {
            JObject extractedField = new JObject();
            if (fields == null)
            {
                return extractedField;
            }
            foreach (string field in fields)
            {
                JProperty prop = jsonObject.Property(field);
                if (prop != null)
                {
                    extractedField.Add(prop);
                }
            }
            return extractedField;
        }

        /// <summary>
        /// Checks whether at least on of the elements of 
        /// the input array contain non-null value
        /// </summary>
        /// <param name="array">JArray to verify</param>
        /// <returns>
        /// True  - If at least one of the elements has non-null value. 
        /// False - otherwise.
        /// </returns>
        public static bool DoesContainValidValue(JArray array)
        {
            if (array == null || array.Where(x => x.HasValues).Count() == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool DoesContainValidStringValue(JObject obj, string keyName)
        {
            if (obj == null)
            {
                return false;
            }
            string strVal = obj.Value<string>(keyName);
            return !string.IsNullOrWhiteSpace(strVal);
        }
    }
}