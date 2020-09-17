using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using System.Xml;

namespace IdeaDatabase
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConnectionConfig
    {
        public static string DatabaseConnectionReadOnlyString;
        public static string DatabaseConnectionReadWriteString;

        private const string DatabaseNameReadOnly = "CPMETAConnectionReadOnlyString";
        private const string DatabaseNameReadWrite = "CPMETAConnectionReadWriteString";


        public static void GetConnectionStrings()
        {
            //DatabaseConnectionString = null;


            string fileLocation = WebConfigurationManager.AppSettings["InnovationPortalConfiguration"];
            //InnovationPortalConfiguration

            //Check if configuration file is defined and can be read           
            if (!String.IsNullOrEmpty(fileLocation) && File.Exists(fileLocation))
            {
                using (XmlReader reader = XmlReader.Create(fileLocation))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                            if (reader.Name.Equals("DatabaseConnection"))
                            {
                                if (reader["name"].Equals(DatabaseNameReadOnly))
                                {
                                    DatabaseConnectionReadOnlyString = reader["connectionString"];
                                }
                                if (reader["name"].Equals(DatabaseNameReadWrite))
                                {
                                    DatabaseConnectionReadWriteString = reader["connectionString"];
                                }
                            }
                    }
                }
            }
            else //otherwise use standard settings from web.config
            {
                ConnectionStringSettings settingsReadOnly = ConfigurationManager.ConnectionStrings[DatabaseNameReadOnly];
                if (settingsReadOnly != null)
                    DatabaseConnectionReadOnlyString = settingsReadOnly.ConnectionString;
                ConnectionStringSettings settingsReadWrite = ConfigurationManager.ConnectionStrings[DatabaseNameReadWrite];
                if (settingsReadWrite != null)
                    DatabaseConnectionReadWriteString = settingsReadWrite.ConnectionString;
            }

            // in case we have only one string (RW), let's use the same one for RO
            if (String.IsNullOrEmpty(DatabaseConnectionReadOnlyString) == true)
                DatabaseConnectionReadOnlyString = DatabaseConnectionReadWriteString;

        }
    }
}