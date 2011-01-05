using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public partial class StoredProcedures
{
    public const SearchWord.Operator defaultOperator = SearchWord.Operator.And;

    [SqlProcedure]
    public static void ListingSearch(
        SqlXml wordsXml,
        bool anyKeywords,
        DateTime maxDate,
        DateTime minDate,
        decimal maxPrice,
        decimal minPrice,
        int categoryId,
        int locationId,
        int sellerId,
        int resultsLimit,
        int startIndex)
    {
        XmlDocument xmlListingsDocument = new XmlDocument();
        XmlElement root = xmlListingsDocument.CreateElement("ListingSearch");
        xmlListingsDocument.AppendChild(root);

        using (SqlConnection connection = new SqlConnection("Context Connection=true"))
        {
            connection.Open();

            bool enableCategory = (categoryId > 0) ? true : false;
            bool enableLocation = (locationId > 0) ? true : false;
            bool enableSeller = (sellerId > 0) ? true : false;
            
            Dictionary<int, Listing> listingTable = new Dictionary<int, Listing>();
            Dictionary<string, SearchWord> searchKeywordTable = WordsXmlToDictionary(wordsXml);

            // Convert table to simple word string list.
            List<string> searchKeywordList = new List<string>();
            foreach (KeyValuePair<string, SearchWord> searchWord in searchKeywordTable)
            {
                searchKeywordList.Add(searchWord.Value.Word);
            }

            bool wildcardSearch = (searchKeywordList.Count == 1) && (searchKeywordList[0] == "*");

            // Fetch all listing IDs related to the keyword.
            string indexQuery = "SELECT ";

            if (wildcardSearch)
                indexQuery += "TOP 1000 ";

            indexQuery +=
                "lk.ListingId, lk.Weight, k.Word FROM Keywords AS k " +
                "INNER JOIN ListingKeywords AS lk ON lk.KeywordId = k.KeywordId " +
                "INNER JOIN Listings AS l ON lk.ListingId = l.ListingId ";

            if (enableCategory)
            {
                // Join intermediate table to find category IDs.
                indexQuery += "INNER JOIN ListingCategories AS lc ON lk.ListingId = lc.ListingId ";
            }

            // Always search by these criteria.
            indexQuery +=
                "WHERE l.CreateDate >= @MinDate " +
                "AND l.CreateDate <= @MaxDate " +
                "AND l.PriceValue >= @MinPrice " +
                "AND l.PriceValue <= @MaxPrice ";

            if (enableCategory)
            {
                indexQuery += "AND lc.CategoryId = @CategoryId ";
            }

            if (enableLocation)
            {
                indexQuery += "AND l.LocationId = @LocationId AND l.ShowLocation = 1 ";
            }

            if (enableSeller)
            {
                indexQuery += "AND l.UserId = @SellerId ";
            }

            if (!wildcardSearch)
            {
                // Append all keywords to query.
                indexQuery += "AND (k.Word = '" +
                    String.Join("' OR k.Word = '", searchKeywordList.ToArray()) + "')";
            }
            else
            {
                indexQuery += "AND k.Word LIKE '%' ";
            }

            // since there are no keywords to order by weight, we should
            // order by something sensible like the boost date as a compromise.
            if (wildcardSearch)
                indexQuery += "ORDER BY l.CreateDate DESC";

            SqlCommand indexCommand = new SqlCommand(indexQuery, connection);
            indexCommand.Parameters.AddWithValue("@MinDate", minDate);
            indexCommand.Parameters.AddWithValue("@MaxDate", maxDate);
            indexCommand.Parameters.AddWithValue("@MinPrice", minPrice);
            indexCommand.Parameters.AddWithValue("@MaxPrice", maxPrice);

            if (enableCategory) indexCommand.Parameters.AddWithValue("@CategoryId", categoryId);
            if (enableLocation) indexCommand.Parameters.AddWithValue("@LocationId", locationId);
            if (enableSeller) indexCommand.Parameters.AddWithValue("@SellerId", sellerId);

            // Return all listings matching any keywords.
            using (SqlDataReader reader = indexCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    int listingId = (int)reader["ListingId"];
                    double weight = (double)reader["Weight"];

                    Keyword keyword = null;
                    if (!wildcardSearch)
                    {
                        SearchWord word = searchKeywordTable[(string)reader["Word"]];
                        keyword = new Keyword(word, weight);
                    }

                    if (!listingTable.ContainsKey(listingId))
                    {
                        // If listing not yet created, do so.
                        Listing listing = new Listing(listingId);
                        listingTable.Add(listingId, listing);
                    }

                    if (keyword != null)
                    {
                        // Then, add the keyword (but not for wildcards).
                        listingTable[listingId].Keywords.Add(keyword);
                    }
                }
            }

            // Simplify listings table for non-and exclusion.
            List<Listing> listingList = new List<Listing>();
            foreach (KeyValuePair<int, Listing> kvp in listingTable)
            {
                listingList.Add(kvp.Value);
            }

            if (!wildcardSearch)
            {
                // Now filter out all the non-and matches for and keywords.
                foreach (Listing listing in listingList)
                {
                    foreach (string searchKeyword in searchKeywordList)
                    {
                        SearchWord searchWord = searchKeywordTable[searchKeyword];
                        SearchWord.Operator swo = searchWord.SearchOperator;

                        if ((swo == SearchWord.Operator.And) &&
                            !listing.StringKeywords.Contains(searchKeyword))
                        {
                            listingTable.Remove(listing.DatabaseId);
                        }
                    }
                }
            }

            if (listingTable.Count > 0)
            {
                AssignListingsXml(listingTable, connection, ref xmlListingsDocument);
            }

            connection.Close();
        }

        // previously this was sent back as an out param. probably better to return results!
        SqlMetaData column = new SqlMetaData("ListingSearch", SqlDbType.NText);
        SqlDataRecord record = new SqlDataRecord(column);
        record.SetString(0, xmlListingsDocument.InnerXml);

        // sql's whacky way of extracting column meta data (pass record into two methods).
        SqlContext.Pipe.SendResultsStart(record);
        SqlContext.Pipe.SendResultsRow(record);
        SqlContext.Pipe.SendResultsEnd();
    }

    public static void AssignListingsXml(
        Dictionary<int, Listing> listingTable, SqlConnection connection, ref XmlDocument document)
    {
        if (listingTable.Count == 0)
        {
            throw new InvalidOperationException(
                "Cannot fetch listings when no Listing IDs were supplied.");
        }

        // Convert Listing IDs into string list usable in query.
        List<string> listingIdList = new List<string>();
        foreach (KeyValuePair<int, Listing> listing in listingTable)
        {
            listingIdList.Add(listing.Value.DatabaseId.ToString());
        }

        string idJoin = String.Join(" OR ListingId = ", listingIdList.ToArray());

        string listingQuery =
            "SELECT * FROM VW_ListingFetch " +
            "WHERE (ListingId = " + idJoin + ") " +
            "AND Enabled = 1 " +
            "ORDER BY CreateDate DESC"; // only really matters for wildcard searches

        SqlCommand listingCommand = new SqlCommand(listingQuery, connection);
        SqlDataReader reader = listingCommand.ExecuteReader();

        while (reader.Read())
        {
            XmlElement listing = document.CreateElement("Listing");
            document.DocumentElement.AppendChild(listing);

            XmlAttribute listingId = document.CreateAttribute("ListingId");
            XmlAttribute sellerId = document.CreateAttribute("SellerId");
            XmlAttribute title = document.CreateAttribute("Title");
            XmlAttribute details = document.CreateAttribute("Details");
            XmlAttribute priceType = document.CreateAttribute("PriceType");
            XmlAttribute priceValue = document.CreateAttribute("PriceValue");
            XmlAttribute createDate = document.CreateAttribute("CreateDate");
            XmlAttribute boostDate = document.CreateAttribute("BoostDate");
            XmlAttribute masterImageId = document.CreateAttribute("MasterImageId");
            XmlAttribute detailsType = document.CreateAttribute("DetailsType");

            listing.Attributes.Append(listingId);
            listing.Attributes.Append(sellerId);
            listing.Attributes.Append(title);
            listing.Attributes.Append(details);
            listing.Attributes.Append(priceType);
            listing.Attributes.Append(priceValue);
            listing.Attributes.Append(createDate);
            listing.Attributes.Append(boostDate);
            listing.Attributes.Append(masterImageId);
            listing.Attributes.Append(detailsType);

            int listingIdInt = (int)reader["ListingId"];
            int listingImageId = 0;
            if (reader["MasterImageId"] != DBNull.Value)
            {
                listingImageId = (int)reader["MasterImageId"];
            }

            listingId.InnerText = listingIdInt.ToString();
            masterImageId.InnerText = listingImageId.ToString();
            sellerId.InnerText = reader["UserId"].ToString();
            title.InnerText = reader["Title"].ToString();
            details.InnerText = reader["Details"].ToString();
            priceType.InnerText = reader["PriceType"].ToString();
            priceValue.InnerText = reader["PriceValue"].ToString();
            createDate.InnerText = ((DateTime)reader["CreateDate"]).ToString("u");
            boostDate.InnerText = ((DateTime)reader["BoostDate"]).ToString("u");
            detailsType.InnerText = reader["DetailsType"].ToString();

            XmlElement contextKeywords = document.CreateElement("RelevantKeywords");
            listing.AppendChild(contextKeywords);

            foreach (Keyword keyword in listingTable[listingIdInt].Keywords)
            {
                XmlElement keywordXml = document.CreateElement("Keyword");
                contextKeywords.AppendChild(keywordXml);

                XmlAttribute name = document.CreateAttribute("Name");
                XmlAttribute weight = document.CreateAttribute("Weight");

                keywordXml.Attributes.Append(name);
                keywordXml.Attributes.Append(weight);

                name.InnerText = keyword.SearchWord.Word;
                weight.InnerText = keyword.Weight.ToString();
            }
        }

        reader.Close();
    }

    public class Listing
    {
        public int DatabaseId;
        public List<Keyword> Keywords;

        public List<string> StringKeywords
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Keyword keyword in Keywords)
                {
                    list.Add(keyword.SearchWord.Word);
                }
                return list;
            }
        }

        public Listing(int databaseId)
        {
            this.DatabaseId = databaseId;
            this.Keywords = new List<Keyword>();
        }
    }

    public class Keyword
    {
        public SearchWord SearchWord;
        public double Weight;

        public Keyword(SearchWord searchWord, double weight)
        {
            this.SearchWord = searchWord;
            this.Weight = weight;
        }
    }

    public struct SearchWord
    {
        public enum Operator { And, Or }

        public string Word;
        public Operator SearchOperator;

        public SearchWord(string word, Operator searchOperator)
        {
            this.Word = word;
            this.SearchOperator = searchOperator;
        }
    }

    public static Dictionary<string, SearchWord> WordsXmlToDictionary(SqlXml sqlXml)
    {
        XmlDocument wordList = new XmlDocument();
        wordList.Load(sqlXml.CreateReader());

        XmlNode root = wordList.FirstChild;
        Dictionary<string, SearchWord> table = new Dictionary<string, SearchWord>();
        SearchWord.Operator nextOperator = defaultOperator;

        foreach (XmlNode wordNode in root.ChildNodes)
        {
            string word = wordNode.InnerText.ToLower();
            if (word == "and")
            {
                nextOperator = SearchWord.Operator.And;
            }
            else if (word == "or")
            {
                nextOperator = SearchWord.Operator.Or;
            }
            else if (!table.ContainsKey(word))
            {
                // Add with overriden operator if available.
                table.Add(word, new SearchWord(word, nextOperator));
                
                // Return to default operator.
                nextOperator = defaultOperator;
            }
        }
        return table;
    }
}