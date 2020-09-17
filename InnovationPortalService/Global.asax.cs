using System;
using Filters;
using Newtonsoft.Json;
using System.Web.Http;
using Hpcs.DependencyInjector;
using IdeaDatabase.Utils;
using Westwind.Globalization;
using IdeaDatabase.App_Start;
using InnovationPortalService.Utils;
using System.Web.Http.Validation;
using IdeaDatabase.DataContext;
using System.Net.Http.Formatting;
using System.Web.Http.Description;
 
using InnovationPortalService.Formatters;
using System.Data.Entity.Core.EntityClient;
using SettingsRepository;
using InnovationPortalService.Filters;
using MultipartDataMediaFormatter;
using MultipartDataMediaFormatter.Infrastructure;
using IdeaDatabase;

namespace InnovationPortalService
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Global : System.Web.HttpApplication
    {
        private static string ResourcesReloadEnabled = string.Empty;

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configuration.Services.Clear(typeof(IBodyModelValidator));
            System.Web.Mvc.AreaRegistration.RegisterAllAreas();
            ConnectionConfig.GetConnectionStrings();
            SettingRepository.SetSettingsRepositoryData(SettingsProvider.LoadSettingsFromDatabase());
            GlobalConfiguration.Configuration.Services.Replace(typeof(IApiExplorer), new ServiceRESTApiExplorer(GlobalConfiguration.Configuration));
            GlobalConfiguration.Configure(WebApiConfig.Register);
          
            /*
            **************************** Filter execution priority order ****************************
            1. Filter execution flow.
                Authorization Filters -> Action Filters -> Exception Filters
            2. Filter scope
                Global Scope -> Controller Scope -> Action Scope.
            3. Multiple filters of the same kind follows the order.
                ApiLoggerFilter -> ErrorCodeTranslationFilter -> ValidationFilter -> AnalyticsFilter
            4. Authorization filters will be always called first.
            5. Exception filters will be called at the last after all filters are executed.
            *****************************************************************************************
            */

            GlobalConfiguration.Configuration.Filters.Add(new ExceptionFilter());
            GlobalConfiguration.Configuration.Filters.Add(new ApiLoggerFilter());
            GlobalConfiguration.Configuration.Filters.Add(new ErrorCodeTranslationFilter()); 
            GlobalConfiguration.Configuration.Filters.Add(new ValidationFilter());

            GlobalConfiguration.Configuration.Formatters.Clear();  // by default there are four formatters
          //  GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter()); // we are using only JSON 
            GlobalConfiguration.Configuration.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter(new MultipartFormatterSettings()));
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Add(new TextMediaTypeFormatter());

            

            DbResourceConfiguration.ConfigurationMode = ConfigurationModes.ConfigFile;
            var providerCs = new EntityConnectionStringBuilder(ConnectionConfig.DatabaseConnectionReadWriteString).ProviderConnectionString;
            DbResourceConfiguration.Current.ConnectionString = providerCs;
            DbResourceConfiguration.Current.DbResourceDataManagerType = typeof(DbResourceMySqlDataManager);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.Response.Headers["X-Content-Type-Options"] = "nosniff";
            ResourcesReloadEnabled = SettingRepository.Get<string>("ResourcesReloadEnabled");
            if (!string.IsNullOrEmpty(ResourcesReloadEnabled) && ResourcesReloadEnabled.Equals("true"))
                DbRes.ClearResources();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }
    }
}
