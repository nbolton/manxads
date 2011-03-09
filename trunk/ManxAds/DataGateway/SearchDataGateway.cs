using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ManxAds.Properties;
using ManxAds.Search;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace ManxAds.DataGateway
{
    public class SearchDataGateway : ISearchDataGateway
    {
        private string connectionString;

        public SearchDataGateway(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region ISearchDataGateway Members

        public void OverwriteWithCatalogue(Catalogue catalogue)
        {
            // Use local physical storage to transfer data to the local DB.
            string tempKeywordFormat = Path.GetTempFileName();
            string tempManifestFormat = Path.GetTempFileName();
            string tempKeywordData = Path.GetTempFileName();
            string tempManifestData = Path.GetTempFileName();

            // Copy formats from the resources to the temp format files.
            File.WriteAllBytes(tempKeywordFormat, Resources.Keywords);
            File.WriteAllBytes(tempManifestFormat, Resources.Manifests);

            // Write values to text file for bulk insert.
            StringBuilder keywordList = new StringBuilder();
            foreach (KeyValuePair<string, Keyword> pair in catalogue.Index)
            {
                keywordList.AppendLine("-1," + pair.Key);
            }
            File.WriteAllText(tempKeywordData, keywordList.ToString());

            // Clear the keyword and manifest tables first.
            string clearQuery = "TRUNCATE TABLE ListingKeywords;";
            clearQuery += "DELETE FROM Keywords;";
            clearQuery += "DBCC CHECKIDENT ('Keywords', RESEED, 0);";

            string keywordsInsertQuery =
                "BULK INSERT Keywords FROM '" + tempKeywordData + "' " +
                "WITH (FORMATFILE = '" + tempKeywordFormat + "')";

            string manifestsInsertQuery =
                "BULK INSERT ListingKeywords FROM '" + tempManifestData + "' " +
                "WITH (FORMATFILE = '" + tempManifestFormat + "')";

            const string keywordsSelectQuery = "SELECT KeywordId, Word FROM Keywords";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand clearCommand = new SqlCommand(clearQuery, connection);
                SqlCommand keywordsInsertCommand = new SqlCommand(keywordsInsertQuery, connection);
                SqlCommand keywordsSelectCommand = new SqlCommand(keywordsSelectQuery, connection);
                SqlCommand manifestsInsertCommand = new SqlCommand(manifestsInsertQuery, connection);

                clearCommand.ExecuteNonQuery();
                keywordsInsertCommand.ExecuteNonQuery();
                SqlDataReader keywordsReader = keywordsSelectCommand.ExecuteReader();

                while (keywordsReader.Read())
                {
                    int keywordId = (int)keywordsReader["KeywordId"];
                    string word = (string)keywordsReader["Word"];

                    // SQL may have inserted incorrect value.
                    if (catalogue.Index.ContainsKey(word))
                    {
                        // Update all keywords with new known IDs.
                        catalogue.Index[word].KnownId = keywordId;
                    }
                }
                keywordsReader.Close();

                StringBuilder manifestList = new StringBuilder();
                foreach (KeyValuePair<string, Keyword> keywordPair in catalogue.Index)
                {
                    // Collect all manifests from the manifest dictionary.
                    Dictionary<int, KeywordManifest> manDict = keywordPair.Value.Manifests;
                    foreach (KeyValuePair<int, KeywordManifest> manifestPair in manDict)
                    {
                        // Make sure keyword is valid first.
                        if (keywordPair.Value.KnownId != -1)
                        {
                            // CSV format: ListingKeywordId,ListingId,KeywordId,Weight
                            manifestList.AppendLine(
                                "-1," + manifestPair.Value.Listing.DatabaseId + "," +
                                keywordPair.Value.KnownId + "," +
                                manifestPair.Value.Weight.ToString("F"));
                        }
                    }
                }
                File.WriteAllText(tempManifestData, manifestList.ToString());

                // Finally, insert manifests with keyword IDs.
                manifestsInsertCommand.ExecuteNonQuery();

                connection.Close();
            }

            File.Delete(tempKeywordData);
            File.Delete(tempKeywordFormat);
            File.Delete(tempManifestData);
            File.Delete(tempManifestFormat);
        }

        public void UpdateForSingleListing(int listingId, SqlXml catalogueXml)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("ListingIndexUpdate", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("listingId", listingId);
                command.Parameters.AddWithValue("catalogueXml", catalogueXml);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        #endregion
    }
}
