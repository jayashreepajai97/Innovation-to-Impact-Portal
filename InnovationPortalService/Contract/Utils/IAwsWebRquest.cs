using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InnovationPortalService.Contract.Utils
{
    public interface IAwsWebRequest
    {
        WebRequest RequestPost(string canonicalUri, string canonicalQueriString, string jsonString);
        string WebResponse(WebRequest request);
    }
}
