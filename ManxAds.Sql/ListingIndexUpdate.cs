using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Xml;
using ManxAds.Sql;

public partial class StoredProcedures
{
    [SqlProcedure]
    public static void ListingIndexUpdate(int listingId, SqlXml catalogueXml)
    {
        XmlDocument document = new XmlDocument();
        document.LoadXml(catalogueXml.Value);

        SearchDataGateway data = new SearchDataGateway();
        data.Open();

        try
        {
            data.ClearListingKeywords(listingId);

            foreach (XmlNode keywordNode in document.SelectNodes("//Keyword"))
            {
                string keyword = keywordNode.Attributes["Name"].Value;
                float weight = float.Parse(keywordNode.Attributes["Weight"].Value);

                int keywordId = data.PersistKeyword(keyword);
                data.PersistListingKeyword(listingId, keywordId, weight);
            }

            data.CleanupOrphanedKeywords();
        }
        finally
        {
            data.Close();
        }
    }
}
