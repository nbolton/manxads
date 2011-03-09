using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Xml;
using ManxAds.Sql;
using System.Collections.Generic;

public partial class StoredProcedures
{
    [SqlProcedure]
    public static void ListingIndexUpdate(int listingId, SqlXml catalogueXml)
    {
        XmlDocument catalogueDocument = getCatalogueDocument(catalogueXml);
        List<string> keywordStrings = getKeywordStrings(catalogueDocument);

        SearchDataGateway data = new SearchDataGateway();
        data.Open();
        try
        {
            ICollection<PartialKeyword> keywords = data.PersistKeywords(keywordStrings);
            data.ClearListingKeywords(listingId);
            data.InsertListingKeywords(listingId, catalogueDocument, keywords);
            //data.CleanupOrphanedKeywords();
        }
        finally
        {
            data.Close();
        }
    }

    private static XmlDocument getCatalogueDocument(SqlXml catalogueXml)
    {
        XmlDocument document = new XmlDocument();
        document.LoadXml(catalogueXml.Value);
        return document;
    }

    private static List<string> getKeywordStrings(XmlDocument document)
    {
        List<string> keywordStrings = new List<string>();
        foreach (XmlNode node in document.SelectNodes("//Keyword/@Name"))
            keywordStrings.Add(node.Value);
        return keywordStrings;
    }
}
