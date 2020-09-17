using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace IdeaDatabase.Validation
{
    public interface IUriValidationAttribute
    {
        bool Validate(HttpActionContext actionContext, ResponseBase response);
    }
}