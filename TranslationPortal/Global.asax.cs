using IdeaDatabase;
using System;
using System.Data.EntityClient;
using Westwind.Globalization;

namespace TranslationPortal
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ConnectionConfig.GetConnectionStrings();

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
    }
}