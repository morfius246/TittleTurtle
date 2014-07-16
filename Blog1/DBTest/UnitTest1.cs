using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;

using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DBTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Specify the provider name, server and database.
            string providerName = "System.Data.SqlClient";
            string serverName = @"(LocalDb)\v11.0";
            string databaseName = "HomeDB";

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            var entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/HomeDB.csdl|
                                       res://*/HomeDB.ssdl|
                                       res://*/HomeDB.msl";
            MessageBox.Show(entityBuilder.ToString());


            using (var ec = new EntityConnection())
            {
                try
                {
                    ec.ConnectionString = entityBuilder.ToString();
                    ec.Open();
                    var ecm = new EntityCommand("INSERT INTO User (UserFirstName, UserLastName) VALUES ('Vsya', 'Pupkin')", ec);
                    ecm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                
                
            }
        }
    }
}
