using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Data.SqlTypes;
using System.Data;

namespace ManxAds.Sql
{
    class SearchDataGateway
    {
        private SqlConnection connection = new SqlConnection("Context Connection=true");

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        internal ICollection<PartialKeyword> PersistKeywords(List<string> keywordStrings)
        {
            // get keywords, possibly with some ids missing
            IEnumerable<PartialKeyword> partialKeywords = getPartialKeywords(keywordStrings);

            ICollection<string> keywordsToInsert = getKeywordsToInsert(partialKeywords);

            // insert the keywords that didn't have ids
            if (keywordsToInsert.Count != 0)
                insertKeywords(keywordsToInsert);

            // return a full list of keywords with ids
            return getPartialKeywords(keywordStrings);
        }

        internal void ClearListingKeywords(int listingId)
        {
            SqlCommand command = new SqlCommand(
                "delete from ListingKeywords where ListingId = @ListingId",
                connection);

            command.Parameters.AddWithValue("ListingId", listingId);
            command.ExecuteNonQuery();
        }

        internal void InsertListingKeywords(
            int listingId,
            XmlDocument catalogueDocument,
            IEnumerable<PartialKeyword> keywords)
        {
            SqlXml listingKeywordsXml = getListingKeywordsXml(catalogueDocument, keywords);

            SqlCommand command = new SqlCommand(
                "ListingKeywordInsertXml", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("ListingId", listingId);
            command.Parameters.AddWithValue("ListingKeywordsXml", listingKeywordsXml);
            command.ExecuteNonQuery();
        }

        private SqlXml getListingKeywordsXml(
            XmlDocument catalogue, IEnumerable<PartialKeyword> keywords)
        {
            XmlDocument document = new XmlDocument();
            XmlElement root = document.CreateElement("ListingKeywords");
            document.AppendChild(root);

            Dictionary<string, int> keywordToIdDictionary = getKeywordToIdDictionary(keywords);

            foreach (XmlNode node in catalogue.SelectNodes("//Keyword"))
            {
                string keyword = node.Attributes["Name"].Value;

                // HACK: some times values will not be added to the dictionary
                // (maybe some unicode chars are not allowed?) -- in this
                // case, just ignore the keyword and move to the next.
                if (!keywordToIdDictionary.ContainsKey(keyword))
                    continue;

                XmlElement listingKeywordElement = document.CreateElement("ListingKeyword");
                root.AppendChild(listingKeywordElement);

                XmlElement keywordIdElement = document.CreateElement("KeywordId");
                listingKeywordElement.AppendChild(keywordIdElement);

                XmlElement weightElement = document.CreateElement("Weight");
                listingKeywordElement.AppendChild(weightElement);

                int keywordId = keywordToIdDictionary[keyword];

                keywordIdElement.InnerText = keywordId.ToString();
                weightElement.InnerText = node.Attributes["Weight"].Value;
            }

            return getSqlXml(document);
        }

        private Dictionary<string, int> getKeywordToIdDictionary(
            IEnumerable<PartialKeyword> keywords)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (PartialKeyword keyword in keywords)
            {
                result[keyword.Name] = keyword.KeywordId.Value;
            }
            return result;
        }

        internal void CleanupOrphanedKeywords()
        {
            string sql =
                "delete Keywords from Keywords as k " +
                "left join ListingKeywords as lk on lk.KeywordId = k.KeywordId " +
                "where lk.ListingKeywordId is null";

            SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        private ICollection<PartialKeyword> getPartialKeywords(IEnumerable<string> keywords)
        {
            SqlXml keywordsXml = getKeywordsXml(keywords);

            SqlCommand command = new SqlCommand("KeywordGetIds", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("KeywordsXml", keywordsXml);

            List<PartialKeyword> keywordList = new List<PartialKeyword>();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                PartialKeyword keyword = new PartialKeyword();
                keyword.KeywordId = getNullableInt(reader["KeywordId"]);
                keyword.Name = (string)reader["Word"];
                keywordList.Add(keyword);
            }
            reader.Close();

            return keywordList;
        }

        private int? getNullableInt(object dbObject)
        {
            if (dbObject is DBNull)
                return null;
            else
                return (int)dbObject;
        }

        private SqlXml getKeywordsXml(IEnumerable<string> keywords)
        {
            XmlDocument document = new XmlDocument();
            XmlElement root = document.CreateElement("Keywords");
            document.AppendChild(root);

            foreach (string keyword in keywords)
            {
                XmlElement keywordElement = document.CreateElement("Keyword");
                root.AppendChild(keywordElement);

                // add keyword as text node to save on memory
                keywordElement.InnerText = keyword;
            }

            return getSqlXml(document);
        }

        private static SqlXml getSqlXml(XmlDocument document)
        {
            StringReader reader = new StringReader(document.InnerXml);
            return new SqlXml(new XmlTextReader(reader));
        }

        private void insertKeywords(IEnumerable<string> keywords)
        {
            SqlCommand command = new SqlCommand("KeywordInsertXml", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("KeywordsXml", getKeywordsXml(keywords));
            command.ExecuteNonQuery();
        }

        private static ICollection<string> getKeywordsToInsert(IEnumerable<PartialKeyword> partialKeywords)
        {
            List<string> result = new List<string>();
            foreach (PartialKeyword keyword in partialKeywords)
            {
                // keyword needs to be inserted if it has no id
                if (!keyword.KeywordId.HasValue)
                    result.Add(keyword.Name);
            }
            return result;
        }

        /*
        public void PersistListingKeyword(int listingId, int keywordId, float weight)
        {
            int? tempListingKeywordId = getListingKeywordId(listingId, keywordId);

            if (tempListingKeywordId.HasValue)
                updateListingKeyword(tempListingKeywordId.Value, weight);
            else
                createListingKeyword(listingId, keywordId, weight);
        }

        private void createListingKeyword(int listingId, int keywordId, float weight)
        {
            SqlCommand command = new SqlCommand(
                "insert into ListingKeywords values (@ListingId, @KeywordID, @Weight)",
                connection);

            command.Parameters.AddWithValue("ListingId", listingId);
            command.Parameters.AddWithValue("KeywordId", keywordId);
            command.Parameters.AddWithValue("Weight", weight);

            command.ExecuteNonQuery();
        }

        private void updateListingKeyword(int listingKeywordId, float weight)
        {
            SqlCommand command = new SqlCommand(
                "update ListingKeywords set " +
                "Weight = @Weight " +
                "where ListingKeywordId = @ListingKeywordId",
                connection);

            command.Parameters.AddWithValue("ListingKeywordId", listingKeywordId);
            command.Parameters.AddWithValue("Weight", weight);

            command.ExecuteNonQuery();
        }

        public int PersistKeyword(string keyword)
        {
            int? tempKeywordId = getKeywordId(keyword);

            if (tempKeywordId.HasValue)
                return tempKeywordId.Value;
            else
                return createKeyword(keyword);
        }

        private int? getListingKeywordId(int listingId, int keywordId)
        {
            SqlCommand command = new SqlCommand(
                "select ListingKeywordId from ListingKeywords " +
                "where ListingId = @ListingID " +
                "and KeywordId = @KeywordID",
                connection);

            command.Parameters.AddWithValue("ListingID", listingId);
            command.Parameters.AddWithValue("KeywordID", keywordId);

            return (int?)command.ExecuteScalar();
        }

        private int createKeyword(string keyword)
        {
            SqlCommand command = new SqlCommand(
                "insert into Keywords values (@Keyword); " +
                "select SCOPE_IDENTITY()", connection);

            command.Parameters.AddWithValue("Keyword", keyword);

            return (int)(decimal)command.ExecuteScalar();
        }

        private int? getKeywordId(string keyword)
        {
            SqlCommand command = new SqlCommand(
                "select KeywordId from Keywords where Word = @Keyword", connection);

            command.Parameters.AddWithValue("Keyword", keyword);

            return (int?)command.ExecuteScalar();
        }
        */
    }
}
