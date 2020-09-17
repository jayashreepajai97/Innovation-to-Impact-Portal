using InnovationPortalService.Responses;
using System;
using System.Xml;
using Responses;

namespace InnovationPortalService
{
    public class ExceptionMapping : IExceptionMapping
    {
        ///// <summary>
        ///// Function maps Generic Faults from HPP to our Fault object
        ///// </summary>
        ///// <param name="r">ResponseBase object - should not be null</param>
        ///// <param name="hppFaults">Array of GenericFaultType objects</param>
        //public void MapHPPGenericFaults(ResponseBase r, HPPService.genericFaultType[] hppFaults)
        //{
        //    if (hppFaults != null && hppFaults.Length > 0)
        //    {
        //        foreach (HPPService.genericFaultType f in hppFaults)
        //        {
        //            if (f == null) // just to be sure
        //                continue;
        //            // Those are errors that indicates if email was found or not in database
        //            // This kind of response should be hidden because of security issue
        //            // using brute-force checking someone could get list of registered mails
        //            if (f.ruleNumber != 473 && f.ruleNumber != 807)
        //            {
        //                // if other rule occur please add the fault to list in class Faults and use it here
        //                // building Fault using 'new' should be considered only in places where you don't know what type of error can happen 

        //                if (Faults.HppErrorMap.ContainsKey(f.ruleNumber))
        //                    r.ErrorList.Add(Faults.HppErrorMap[f.ruleNumber]);
        //                else
        //                    r.ErrorList.Add(new Fault("HPP", Faults.GenericError.ReturnCode, Convert.ToString(f.ruleNumber) + ": " + f.desc));

        //            }
        //        }
        //    }
        //}
        public void MapGenericFaults(ResponseBase r, string errXml)
        {
            if (!string.IsNullOrEmpty(errXml) && errXml.Length > 0)
            {
                XmlDocument xdoc = new XmlDocument();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
                nsmgr.AddNamespace("ns", "http://isac.hp.com/schema/registrations/CustomeErrorSchema.xsd");
                xdoc.LoadXml("<err>" + errXml + "</err>");

                XmlNodeList nodeFaults = xdoc.DocumentElement.FirstChild.ChildNodes;


                foreach (XmlNode nodeFault in nodeFaults)
                {
                    //this is needed to filter out nodes that dont provide error information
                    if (!string.IsNullOrEmpty(nodeFault.InnerText) && nodeFault.InnerText.Length > 2)
                    {
                        MapReturnCode(nodeFault.OuterXml, r);
                    }
                }
            }
        }

        public void MapReturnCode(string xml, ResponseBase r)
        {
            if (xml.Contains("<ns:FileldID>userId</ns:FileldID>") || xml.Contains("<fieldName>userId</fieldName><code>field.duplicate</code>"))
            {
                r.ErrorList.Add(Faults.UserIdExists);
            }
            else if (xml.Contains("<ns:FileldID>email</ns:FileldID>"))
            {
                r.ErrorList.Add(Faults.EmailExists);
            }
            else if (xml.Contains("<fieldName>userId</fieldName><code>field.same</code>"))
            {
                r.ErrorList.Add(Faults.UserNameIsTheSame);
            }
            else if (xml.Contains("<fieldName>password</fieldName><code>field.same</code>"))
            {
                r.ErrorList.Add(Faults.PasswordIsTheSame);
            }
            else if (xml.Contains("<fieldName>password</fieldName><code>field.short</code>") || xml.Contains("<ns:FileldID>password</ns:FileldID><ns:Desc>Password must contain at least 6 characters.</ns:Desc>"))
            {
                r.ErrorList.Add(Faults.PasswordTooShort);
            }
            else if (xml.Contains("<fieldName>userId</fieldName><code>field.nomatch</code>") || xml.Contains("<ruleNumber>228</ruleNumber><fieldName>userId</fieldName>"))
            {
                r.ErrorList.Add(Faults.InvalidCredentials);
            }
            else if (xml.Contains("<fieldName>email</fieldName><code>field.nomatch</code>"))
            {
                r.ErrorList.Add(Faults.EmailAddressNotFound);
            }
            else if (xml.Contains("<fieldName>profileId</fieldName><code>field.notfound</code>"))
            {
                r.ErrorList.Add(Faults.InvalidCredentials);
            }
            else if (xml.Contains("<fieldName>guid</fieldName><code>field.notfound</code>"))
            {
                r.ErrorList.Add(Faults.ResetGuidNotFound);
            }
            else if (xml.Contains("HPP_CUSTOMER_001"))
            {
                if (xml.Contains("HPP Authentication Service not available"))
                    r.ErrorList.Add(Faults.AuthServiceNotAvailable);
                else
                    r.ErrorList.Add(Faults.InvalidCredentials);
            }
            else if (xml.Contains("<ns:FaultCode>token.invalid</ns:FaultCode>") || xml.Contains("<ruleNumber>235</ruleNumber><fieldName>changeUserID</fieldName><code>token.expired</code>"))
            {
                r.ErrorList.Add(Faults.InvalidCredentials);
            }            
            else if (xml.Contains("<ruleNumber>422</ruleNumber><fieldName>currentPassword</fieldName>"))
            {
                r.ErrorList.Add(Faults.InvalidCredentials);
            }
            else if (xml.Contains("<ns:FaultCode>BW-HTTP-100300</ns:FaultCode>"))
            {
                r.ErrorList.Add(Faults.HPPInternalError);
            }
            else if (xml.Contains("ns:FaultDesc>Technical Problem has occurred while serving the request</ns:FaultDesc>"))
            {
                r.ErrorList.Add(Faults.HPPInternalError);
            }
            else if (xml.Contains("><ns:FileldID>password</ns:FileldID><ns:Desc>Password cannot be the same as User ID.</ns:Desc>"))
            {
                r.ErrorList.Add(Faults.MustUseDiffPasswordOrUserName);
            }
            else if (xml.Contains("HP_ID is mandatory, Please key in correct  value"))
            {
                r.ErrorList.Add(Faults.MissingHPIDidentification);
            }
            else if(xml.Contains("CKM_MODEL_ID  is mandatory, Please key in correct value") ||
                    xml.Contains("Customer or Product not found") ||
                    xml.Contains("CKM_PROD_SERIAL_ID  is mandatory, Please key in correct value"))
            {
                r.ErrorList.Add(Faults.ProductNotFound);
            }
            else
            {
                r.ErrorList.Add(Faults.UnMappedError);
            }
        }
    }
}