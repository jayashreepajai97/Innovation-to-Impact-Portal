using InnovationPortalService.Filters;
using Responses;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Description;

namespace Filters
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();
            if (ConfigurationUtils.CaptchaRequired && apiDescription.ActionDescriptor.GetCustomAttributes<CaptchaAuthorizationAttribute>().Count != 0)
            {
                operation.parameters.Add(new Parameter
                {
                    name = "ReCaptchaToken",
                    @in = "header",
                    type = "string",
                    required = false
                });
                operation.responses["200"].headers = new Dictionary<string, Header> {
                        { "Access-Control-Allow-Origin", new Header () { type = "string" } }
                    };
            }
            if (apiDescription.HttpMethod == HttpMethod.Options)
            {
                operation.produces.Clear();
                operation.consumes.Clear();
                operation.parameters.Clear();
                operation.produces.Add("application/json");
                operation.operationId = operation.operationId + "_Options";
                operation.responses.Add("200", new Response()
                {
                    headers = new Dictionary<string, Header> {
                        { "Access-Control-Allow-Origin",  new Header () { type = "string" } },
                        { "Access-Control-Allow-Methods", new Header () { type = "string" } },
                        { "Access-Control-Allow-Headers", new Header () { type = "string" } }
                    },
                    description = "200 response"
                });
            }
            else
            {
                //IList<IDictionary<string, IList<string>>> EnvironmentVariableTarget = new List<Dictionary<string, List<string>>>();
                //var t = new Dictionary<string, List<string>> { { "api_key", new List<string>() } };
                operation.security = /*(IList<IDictionary<string, IList<string>>>)*/
                    new List<IDictionary<string, IEnumerable<string>>>() { new Dictionary<string, IEnumerable<string>> { { "api_key", new List<string>() } } };

                /* for showing the header parameter in swagger for not anonymous methods */
                if( apiDescription.ActionDescriptor.GetCustomAttributes<CredentialsHeaderAttribute>().Count > 0)
                {
                    operation.parameters.Add(new Parameter
                    {
                        name = "Authorization",
                        @in = "header",
                        type = "string",
                        required = true
                    });
                }
                /* for showing the basic authorization header parameter in swagger for not anonymous methods */
                if (apiDescription.ActionDescriptor.GetCustomAttributes<BasicAuthorizationEnableAttribute>().Count > 0)
                {
                    operation.parameters.Add(new Parameter
                    {
                        name = "Authorization",
                        @in = "header",
                        type = "string",
                        required = true
                    });
                }

            }
        }
    }
}