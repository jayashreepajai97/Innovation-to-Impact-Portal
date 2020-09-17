using IdeaDatabase.Validation;
using Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;

namespace InnovationPortalService.Filters
{
    public class StringAttributeValidationFromUri : Attribute, IUriValidationAttribute
    {
        public string matchRegExpression;

        public StringAttributeValidationFromUri(string matchRegExpression=null)
        {
            this.matchRegExpression = matchRegExpression;
        }
        public bool Validate(HttpActionContext actionContext, ResponseBase response)
        {
            if (matchRegExpression == null)
            {
                return true;
            }

            KeyValuePair<string, object> req = actionContext.ActionArguments.FirstOrDefault();


            if (req.Value == null )
            {
                response.ErrorList.Add(new RequiredValidationFault(req.Key));
                return false;
            }

            Regex re = new Regex(matchRegExpression);
            if (!re.Match(req.Value.ToString()).Success)
            {
                response.ErrorList.Add(new FormatValidationFault(req.Key, matchRegExpression));
                return false;
            }
            return true;
        }

    }
}