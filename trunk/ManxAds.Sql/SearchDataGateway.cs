using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

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

        internal void ClearListingKeywords(int listingId)
        {
            SqlCommand command = new SqlCommand(
                "delete from ListingKeywords where ListingId = @ListingId",
                connection);

            command.Parameters.AddWithValue("ListingId", listingId);
            command.ExecuteNonQuery();
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
    }
}
