using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace IdeaDatabase.App_Start
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.EnableCors();
        }
    }
}