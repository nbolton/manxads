using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Xml;

namespace ManxAds.Pimpmaster
{
    public partial class StoredProcedures
    {
        [SqlProcedure]
        public static void CataloguePublish(SqlXml CatalogueXml)
        {
            XmlDocument catalogue = new XmlDocument();
            catalogue.Load(CatalogueXml.CreateReader());
            XmlNode root = catalogue.FirstChild;

            using (SqlConnection connection = new SqlConnection("Context Connection=true"))
            {
                connection.Open();

                SqlCommand deleteCommand = new SqlCommand(
                    "TRUNCATE TABLE ListingKeywords; " +
                    "DELETE FROM Keywords; " +
                    "DBCC CHECKIDENT ('Keywords', RESEED, 0);",
                    connection);
                deleteCommand.ExecuteNonQuery();

                SqlCommand command = new SqlCommand("ListingKeywordCreate");
                command.Parameters.Add("@Word", SqlDbType.NVarChar, 50);
                command.Parameters.Add("@ListingId", SqlDbType.Int);
                command.Parameters.Add("@Weight", SqlDbType.Float);
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;

                foreach (XmlNode wordNode in root.ChildNodes)
                {
                    // Pass the @Word parameter from Name attribute.
                    command.Parameters["@Word"].Value = wordNode.Attributes["Name"].InnerText;

                    foreach (XmlNode manifest in wordNode.ChildNodes)
                    {
                        int listingId = int.Parse(manifest.Attributes["ListingId"].Value);
                        float weight = float.Parse(manifest.Attributes["Weight"].Value);

                        command.Parameters["@ListingId"].Value = listingId;
                        command.Parameters["@Weight"].Value = weight;

                        command.ExecuteNonQuery();
                    }   
                }
                connection.Close();
            }
        }
    }
}