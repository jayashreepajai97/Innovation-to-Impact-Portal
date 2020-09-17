using IdeaDatabase.Responses;
using IdeaDatabase.Validation;
using InnovationPortalService.Filters;
using Responses;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {           
            if (actionContext.ModelState.IsValid == false)
            {
                ResponseBase r = new ResponseBase();
                HashSet<string> errors = new HashSet<string>();
                foreach (ModelState ms in actionContext.ModelState.Values)
                    foreach (ModelError e in ms.Errors)
                    {
                        // at first check if Message exists: it contains detailed description of given failure
                        if(!string.IsNullOrEmpty(e.ErrorMessage))
                            errors.Add(e.ErrorMessage);
                        // then check if Exception was thrown
                        else if (e.Exception != null)
                            errors.Add(e.Exception.Message);
                    }

                foreach (string s in errors)
                    r.ErrorList.Add(new Fault("InvalidJSON", s));

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, r, GlobalConfiguration.Configuration);
                return;
            }

            if (actionContext.Request.Method.Equals(HttpMethod.Options))
            {
                return;
            }

            if (actionContext.Request.Method == HttpMethod.Post || actionContext.Request.Method == HttpMethod.Put 
                || actionContext.Request.Method == HttpMethod.Delete || actionContext.Request.Method == new HttpMethod("PATCH"))
            {
                object req = actionContext.ActionArguments.Where(x => x.Key == "req").Select(x => x.Value).FirstOrDefault();
                if (req == null)
                {
                    // body should be present unless the method is properly marked
                    if ( actionContext.ActionDescriptor.GetCustomAttributes<AllowEmptyBodyAttribute>().Count == 0)
                    {
                        ResponseBase r = new ResponseBase();
                        r.ErrorList.Add(Faults.MissingRequestContent);
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, r, GlobalConfiguration.Configuration);
                        return;
                    }
                }
                else if (req is ValidableObject)
                {
                    ValidableObject vo = (ValidableObject)actionContext.ActionArguments["req"];
                    ResponseBase r = new ResponseBase();
                    if (!vo.IsValid(r))
                    {                                
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, r, GlobalConfiguration.Configuration);
                        return;
                    }
                }
            }

            if (actionContext.ActionDescriptor != null)
            {
                foreach (IUriValidationAttribute attribute in
                    actionContext.ActionDescriptor.GetCustomAttributes<IUriValidationAttribute>())
                {
                    ResponseBase r = new ResponseBase();
                    if (attribute.Validate(actionContext, r) == false)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, r, GlobalConfiguration.Configuration);
                        return;
                    }
                }
            }
        }

    }
}