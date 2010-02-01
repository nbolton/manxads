using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Threading;
using ManxAds.Properties;

namespace ManxAds.Search
{
    public class Catalogue
    {
        private const int generaitonPauseMs = 10;
        private const int titleReverseThreshold = 1;
        private const int detailsReverseThreshold = 3;
        private const float titleWeightFactor = 1.0f;
        private const float detailsWeightFactor = 0.05f;
        private const float listingCountEffect = 0.2f;
        private const float imageCountEffect = 0.2f;
        private const float initialGeneralFactor = 5f;
        private const int categoryCountThreshold = 1;
        private const float categoryPenaltyFactor = 0.1f;
        private const int imageCountMaximum = 5;
        private const int firstDayIncrease = 3;
        private const int fiveDayIncrease = 2;
        private const int tenDayIncrease = 1;

        private Dictionary<string, Keyword> index;
        private string connectionString;

        public Dictionary<string, Keyword> Index
        {
            get { return index; }
        }

        public int KeywordCount
        {
            get { return index.Count; }
        }

        [Obsolete("Use GenerateCatalogue instead.")] 
        public Catalogue() : this(null) { }

        private Catalogue(string connectionString)
        {
            this.index = new Dictionary<string, Keyword>();
            this.connectionString = connectionString;
        }

        public void UpdateKeywords()
        {
            Catalogue catalogue = this;

            /*string keywordsFormatFile = AppDomain.CurrentDomain.BaseDirectory + @"\Keywords.fmt";
            string manifestsFormatFile = AppDomain.CurrentDomain.BaseDirectory + @"\Manifests.fmt";
            string tempPath = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine) + @"\ManxAdsCrawler.temp";*/
            
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

        public static Catalogue GenerateCatalogue(string connectionString)
        {
            Catalogue catalogue = new Catalogue(connectionString);
            List<ListingBase> listingList = ListingBase.Fetch(connectionString);
            foreach (ListingBase listing in listingList)
            {
                float generalFactor = initialGeneralFactor;
                Listing fullListing = Listing.Fetch(listing.DatabaseId, connectionString);

                // Deduct points for every category listing is added to.
                generalFactor -= fullListing.GetCategories(connectionString).Count * categoryPenaltyFactor;

                // Increase weight for user's who have many listings.
                generalFactor += fullListing.GetSeller(connectionString).GetListingCount(connectionString) * listingCountEffect;

                // Increase weight for listings with images.
                bool tooManyImages = fullListing.GetImageCount(connectionString) > imageCountMaximum;
                float imageIncrease = fullListing.GetImageCount(connectionString) * imageCountEffect;
                float maximumIncrease = imageCountMaximum * imageCountEffect;
                generalFactor += tooManyImages ? maximumIncrease : imageIncrease;

                // Calculate weights for title and description separately.
                float titleWeight = titleWeightFactor * generalFactor;
                float detailsWeight = detailsWeightFactor * generalFactor;

                // Add context of title and description (with different weighting schemes).
                catalogue.AddWordContext(listing.Title, listing, titleWeight, titleReverseThreshold);
                catalogue.AddWordContext(listing.Details, listing, detailsWeight, detailsReverseThreshold);

                Thread.Sleep(generaitonPauseMs); // Do not use maximum CPU time.
            }

            return catalogue;
        }

        public static int CompareWeights(Listing x, Listing y)
        {
            float xWeight = x.SearchHighlight.TotalWeight;
            float yWeight = y.SearchHighlight.TotalWeight;

            if (xWeight < yWeight)
            {
                return +1;
            }
            else if (xWeight > yWeight)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public void AddWordContext(string context, ListingBase listing, float weightFactor, int reverseThreshold)
        {
            WordList wordList = new WordList(context, true);
            foreach (ContextKeyword word in wordList.InnerList)
            {
                this.AddWord(word, listing, weightFactor, reverseThreshold);
            }
        }

        protected void AddWord(ContextKeyword keyword, ListingBase listing, float weightFactor, int reverseThreshold)
        {
            string word = keyword.Name.Trim().ToLower();
            if (index.ContainsKey(word))
            {
                index[word].AddListing(listing, keyword.Weight * weightFactor, reverseThreshold);
            }
            else
            {
                index.Add(word, new Keyword(word, listing, keyword.Weight * weightFactor));
            }
        }

        protected void AddKeyword(Keyword keyword)
        {
            index.Add(keyword.Name, keyword);
        }

        public void Publish(int timeout)
        {
            //using (StoredProceedure sp = new StoredProceedure("CataloguePublish", timeout))
            using (StoredProceedure sp = new StoredProceedure("ListingKeywordUpdate", timeout))
            {
                sp.AddParam("@catalogueXml", this.ToXml());
                //sp.AddParam("@CatalogueXml", this.ToXml());
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        [Obsolete]
        public static Catalogue Parse(SqlXml xml)
        {
            Catalogue catalogue = new Catalogue();
            XmlDocument document = new XmlDocument();
            document.Load(xml.CreateReader());

            XmlNode root = document.FirstChild;
            foreach (XmlNode wordNode in root.ChildNodes)
            {
                string word = wordNode.Attributes["Name"].InnerText;
                Keyword keyword = new Keyword(word);
                catalogue.AddKeyword(keyword);

                foreach (XmlNode manifestNode in wordNode.ChildNodes)
                {
                    int listingId = int.Parse(manifestNode.Attributes["ListingId"].Value);
                    int weight = int.Parse(manifestNode.Attributes["Weight"].Value);
                    int occurances = int.Parse(manifestNode.Attributes["Occurances"].Value);

                    ListingBase listing = new ListingBase(listingId);
                    KeywordManifest manifest = new KeywordManifest(listing, weight, occurances);
                    keyword.AddManifiest(manifest);
                }
            }
            return catalogue;
        }

        public override string ToString()
        {
            throw new NotSupportedException();
        }

        public SqlXml ToXml()
        {
            XmlDocument catalogue = new XmlDocument();
            XmlNode root = catalogue.CreateElement("Catalogue");
            catalogue.AppendChild(root);

            foreach (KeyValuePair<string, Keyword> keywordKvp in index)
            {
                XmlNode wordNode = catalogue.CreateElement("Keyword");
                XmlAttribute wordAttribute = catalogue.CreateAttribute("Name");
                wordAttribute.InnerText = keywordKvp.Key;
                wordNode.Attributes.Append(wordAttribute);
                root.AppendChild(wordNode);

                foreach (KeyValuePair<int, KeywordManifest> manifestKvp in keywordKvp.Value.Manifests)
                {
                    KeywordManifest manifiest = manifestKvp.Value;
                    XmlNode manifestNode = catalogue.CreateElement("Manifest");
                    XmlAttribute listingId = catalogue.CreateAttribute("ListingId");
                    XmlAttribute weight = catalogue.CreateAttribute("Weight");

                    wordNode.AppendChild(manifestNode);
                    manifestNode.Attributes.Append(listingId);
                    manifestNode.Attributes.Append(weight);

                    listingId.InnerText = manifiest.Listing.DatabaseId.ToString();
                    weight.InnerText = manifiest.Weight.ToString();
                }
            }

            StringReader reader = new StringReader(catalogue.InnerXml);
            return new SqlXml(new XmlTextReader(reader));
        }
    }
}
