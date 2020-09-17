using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils;
using Responses;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Filters
{
    /// <summary>
    /// This filter catches all previously unhandled exceptions and build response 
    /// based on ResponseBase and puts Fault into ErrorList
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        string Failure = EnumUtils.ConvertValue<string>(ResponseStatusType.Failure);
        /// <summary>
        /// Method called to handle exception
        /// </summary>
        /// <param name="context">Provided by environment</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            ResponseBase r = new ResponseBase();
            r.Status = Failure;
            if (context.Exception is HttpResponseException && ((HttpResponseException)context.Exception).Response.StatusCode == HttpStatusCode.UnsupportedMediaType)
                r.ErrorList.Add(Faults.InvalidContentType);
            else
            {
                r.ErrorList.Add(new Fault(Faults.UnknownError, context.Exception));
                if(context.Exception.InnerException != null)
                    r.ErrorList.Add(new Fault(Faults.UnknownError, context.Exception.InnerException));
            }
            context.Response = context.Request.CreateResponse(HttpStatusCode.OK, r, GlobalConfiguration.Configuration);
        }
    }
}