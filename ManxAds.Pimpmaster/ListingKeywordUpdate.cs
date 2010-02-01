using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;
using System.Xml;
using System.IO;


public partial class StoredProcedures
{
    //private const string transactionName = "KeywordUpdateTransaction";

    [SqlProcedure]
    public static void ListingKeywordUpdate(SqlXml catalogueXml)
    {
        string tempPath = @"C:\Windows\Temp"; // Environment variable didn't work.
        string keywordFilePath = tempPath + @"\ManxAdsCrawlerKeywords.temp";
        string manifestFilePath = tempPath + @"\ManxAdsCrawlerManifests.temp";

        StringBuilder keywordList = new StringBuilder();
        StringBuilder manifestList = new StringBuilder();
        StringBuilder keywordQuery = new StringBuilder();
        XmlDocument catalogue = new XmlDocument();

        catalogue.Load(catalogueXml.CreateReader());
        XmlNode root = catalogue.DocumentElement;

        // Use temporary keyword SQL var.
        keywordQuery.AppendLine("DECLARE @LastKeywordId int;");

        // Run everything in one query so users don't miss out on data.
        //updateQuery.AppendLine("BEGIN TRANSACTION " + transactionName + ";");

        // Reset must be done different for each, as one has dependances.
        keywordQuery.AppendLine("TRUNCATE TABLE ListingKeywords;");
        keywordQuery.AppendLine("DELETE FROM Keywords;");
        keywordQuery.AppendLine("DBCC CHECKIDENT ('Keywords', RESEED, 0);");

        foreach (XmlNode keywordNode in root.ChildNodes)
        {
            string word = keywordNode.Attributes["Name"].Value;
            /*updateQuery.AppendFormat("INSERT INTO Keywords (Word) VALUES ('{0}'); ", word);
            updateQuery.AppendLine("SET @LastKeywordId = SCOPE_IDENTITY();");*/

            // Add to text file instead.
            keywordList.AppendLine(word);

            /*foreach (XmlNode manifestNode in keywordNode.ChildNodes)
            {
                // Parsing makes sure that data is valid for SQL insertion.
                int listingId = int.Parse(manifestNode.Attributes["ListingId"].Value);
                float weight = float.Parse(manifestNode.Attributes["Weight"].Value);

                // Insert listing-keyword association row, using last inserted keyword ID.
                updateQuery.Append("INSERT INTO ListingKeywords (KeywordId, ListingId, Weight) ");
                updateQuery.AppendFormat(
                    "VALUES (@LastKeywordId, {0}, {1});",
                    listingId.ToString(),
                    weight.ToString());

                // Intentionally append blank line.
                updateQuery.AppendLine();


            }*/
        }

        // Temporarily write lists of keywords and manifests for bulk insert.
        File.WriteAllText(keywordFilePath, keywordList.ToString());
        keywordQuery.AppendLine("BULK INSERT Keywords FROM '" + keywordFilePath + "'");

        //File.WriteAllText(TempPath + manifestFileName, manifestList.ToString());

        //updateQuery.AppendLine("COMMIT TRANSACTION " + transactionName + ";");

        using (SqlConnection connection = new SqlConnection("Context Connection=true"))
        {
            connection.Open();

            // Execute query which resets the tables and inserts fresh records.
            SqlCommand command = new SqlCommand(keywordQuery.ToString(), connection);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
};
