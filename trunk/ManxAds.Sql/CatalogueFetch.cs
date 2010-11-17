using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace ManxAds.Sql
{
    public partial class StoredProcedures
    {
        [SqlProcedure]
        public static void CatalogueFetch(
            SqlXml WordsXml, SqlXml CriteriaXml, out SqlXml CatalogueXml)
        {
            CatalogueXml = GetCatalogueXml(WordsXml, CriteriaXml);
        }

        public static List<string> WordListXmlToStringList(SqlXml sqlXml)
        {
            XmlDocument wordList = new XmlDocument();
            wordList.Load(sqlXml.CreateReader());

            XmlNode root = wordList.FirstChild;
            List<string> list = new List<string>();
            
            foreach (XmlNode wordNode in root.ChildNodes)
            {
                list.Add(wordNode.InnerText);
            }
            return list;
        }

        protected static SqlXml GetCatalogueXml(SqlXml wordsXml, SqlXml criteriaXml)
        {
            XmlDocument catalogue = new XmlDocument();
            List<string> wordList = WordListXmlToStringList(wordsXml);

            using (SqlConnection connection = new SqlConnection("Context Connection=true"))
            {
                SqlDataReader reader;
                SqlCommand command;
                XmlDocument criteria = new XmlDocument();
                criteria.LoadXml(criteriaXml.Value);
                XmlNode criteriaRoot = criteria.FirstChild;

                if (criteriaRoot.ChildNodes.Count == 0)
                {
                    command = new SqlCommand("ListingKeywordFetchByWord", connection);
                    command.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    DateTime startDate = DateTime.MinValue;
                    DateTime endDate = DateTime.MaxValue;
                    decimal startPrice = 0M, endPrice = 0M;
                    int categoryId = -1, locationId = -1;

                    foreach (XmlNode member in criteriaRoot.ChildNodes)
                    {
                        string key = member.Attributes["Key"].InnerText;
                        string value = member.Attributes["Value"].InnerText;

                        switch (key)
                        {
                            case "StartDate":
                                startDate = DateTime.Parse(value);
                                break;

                            case "EndDate":
                                endDate = DateTime.Parse(value);
                                break;

                            case "StartPrice":
                                startPrice = decimal.Parse(value);
                                break;

                            case "EndPrice":
                                endPrice = decimal.Parse(value);
                                break;

                            case "CategoryId":
                                categoryId = int.Parse(value);
                                break;

                            case "LocationId":
                                locationId = int.Parse(value);
                                break;
                        }
                    }

                    bool enableCategory = (categoryId > 0 ? true : false);
                    bool enableLocation = (locationId > 0 ? true : false);

                    // Fetch all listing IDs related to the keyword.
                    string queryString =
                        "SELECT lk.ListingId, lk.Weight FROM Keywords AS k " +
                        "INNER JOIN ListingKeywords AS lk ON lk.KeywordId = k.KeywordId " +
                        "INNER JOIN Listings AS l ON lk.ListingId = l.ListingId ";

                    if (enableCategory)
                    {
                        // Join intermediate table to find category IDs.
                        queryString +=
                            "INNER JOIN ListingCategories AS lc " +
                            "ON lk.ListingId = lc.ListingId ";
                    }

                    // Always search by these criteria.
                    queryString +=
                        "WHERE k.Word = @Word " +
                        "AND l.CreateDate >= @StartDate " +
                        "AND l.CreateDate <= @EndDate " +
                        "AND l.PriceValue >= @StartPrice " +
                        "AND l.PriceValue <= @EndPrice ";

                    if (enableCategory)
                    {
                        queryString += "AND lc.CategoryId = @CategoryId ";
                    }

                    if (enableLocation)
                    {
                        queryString +=
                            "AND l.LocationId = @LocationId " +
                            "AND l.ShowLocation = 1 ";
                    }

                    command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@StartPrice", startPrice);
                    command.Parameters.AddWithValue("@EndPrice", endPrice);

                    if (enableCategory)
                    {
                        command.Parameters.AddWithValue("@CategoryId", categoryId);
                    }

                    if (enableLocation)
                    {
                        command.Parameters.AddWithValue("@LocationId", locationId);
                    }
                }

                XmlNode wordListRoot = catalogue.CreateElement("Catalogue");
                catalogue.AppendChild(wordListRoot);

                command.Parameters.Add("@Word", SqlDbType.NVarChar, 50);
                connection.Open();

                foreach (string word in wordList)
                {
                    command.Parameters["@Word"].Value = word;
                    reader = command.ExecuteReader();

                    // Create word node and set value as attribute.
                    XmlNode wordNode = catalogue.CreateElement("Keyword");
                    XmlAttribute wordAttribute = catalogue.CreateAttribute("Name");
                    wordAttribute.InnerText = word;
                    wordNode.Attributes.Append(wordAttribute);
                    wordListRoot.AppendChild(wordNode);

                    while (reader.Read())
                    {
                        XmlNode manifestNode = catalogue.CreateElement("Manifest");
                        XmlAttribute listingId = catalogue.CreateAttribute("ListingId");
                        XmlAttribute weight = catalogue.CreateAttribute("Weight");

                        wordNode.AppendChild(manifestNode);
                        manifestNode.Attributes.Append(listingId);
                        manifestNode.Attributes.Append(weight);

                        listingId.InnerText = reader["ListingId"].ToString();
                        weight.InnerText = reader["Weight"].ToString();
                    }

                    reader.Close();
                }
                connection.Close();
            }

            // Read output from XML document into SQL XML structure.
            StringReader stringReader = new StringReader(catalogue.InnerXml);
            return new SqlXml(new XmlTextReader(stringReader));
        }
    }
}