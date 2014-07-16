using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DBTest
{
    [TestClass]
    public class DatabaseTests
    {
        public string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Work\BionicUniversity\Blog1\Blog1\App_Data\WebBlog.mdf;Integrated Security=True";

        [TestMethod]
        public void ConnectToDb()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                System.Diagnostics.Debug.Print("Connection established successfuly\nTest Passed");
                connection.Close();
            }
        }

        [TestMethod]
        public void SelectFromDbTables()
        {
            string queryString = "SELECT * FROM dbo.Articles";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    System.Diagnostics.Debug.Print("Connection established successfuly\nRead From Articles");
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        System.Diagnostics.Debug.Print(reader[0].ToString() + reader[1].ToString() + reader[2].ToString() + reader[3].ToString() + reader[4].ToString() + reader[5].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                     System.Diagnostics.Debug.Fail(ex.Message);
                }
                System.Diagnostics.Debug.Print("Test Passed");
            }
        }
    }
}
