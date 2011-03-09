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
using ManxAds.DataGateway;

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
        private IDataGatewayProvider data;

        public Dictionary<string, Keyword> Index
        {
            get { return index; }
        }

        public int KeywordCount
        {
            get { return index.Count; }
        }

        public Catalogue(string connectionString)
            : this(new DataGatewayProvider(connectionString)) { }

        public Catalogue(IDataGatewayProvider data)
        {
            this.index = new Dictionary<string, Keyword>();
            this.data = data;
        }

        public void UpdateKeywords()
        {
            data.Search.OverwriteWithCatalogue(this);
        }

        [Obsolete("Use GenerateFromSingle instead.")]
        public void GenerateFromAll()
        {
            ICollection<IListingBase> listingList = data.Listing.FetchBases();
            foreach (IListingBase listing in listingList)
            {
                IListing fullListing = data.Listing.Fetch(listing.DatabaseId);

                GenerateFromSingle(fullListing);

                Thread.Sleep(generaitonPauseMs); // Do not use maximum CPU time.
            }
        }

        public void GenerateFromSingle(IListing listing)
        {
            float generalFactor = initialGeneralFactor;

            // Deduct points for every category listing is added to.
            generalFactor -= data.Listing.GetCategories(listing).Count * categoryPenaltyFactor;

            // Increase weight for user's who have many listings.
            generalFactor += data.Listing.GetSellerListingCount(listing) * listingCountEffect;

            // Increase weight for listings with images.
            int imageCount = data.Listing.GetImageCount(listing);
            bool tooManyImages = imageCount > imageCountMaximum;
            float imageIncrease = imageCount * imageCountEffect;
            float maximumIncrease = imageCountMaximum * imageCountEffect;
            generalFactor += tooManyImages ? maximumIncrease : imageIncrease;

            // Calculate weights for title and description separately.
            float titleWeight = titleWeightFactor * generalFactor;
            float detailsWeight = detailsWeightFactor * generalFactor;

            // Add context of title and description (with different weighting schemes).
            AddWordContext(listing.Title, listing, titleWeight, titleReverseThreshold);
            AddWordContext(listing.Details, listing, detailsWeight, detailsReverseThreshold);
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

        public void AddWordContext(string context, IListing listing, float weightFactor, int reverseThreshold)
        {
            WordList wordList = new WordList(context, true);
            foreach (ContextKeyword word in wordList.InnerList)
            {
                this.AddWord(word, listing, weightFactor, reverseThreshold);
            }
        }

        protected void AddWord(ContextKeyword keyword, IListing listing, float weightFactor, int reverseThreshold)
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

        public SqlXml GetSingleListingXml()
        {
            XmlDocument catalogue = new XmlDocument();
            XmlNode root = catalogue.CreateElement("Catalogue");
            catalogue.AppendChild(root);

            foreach (KeyValuePair<string, Keyword> keywordKvp in index)
            {
                XmlNode wordNode = catalogue.CreateElement("Keyword");
                XmlAttribute wordAttribute = catalogue.CreateAttribute("Name");
                wordNode.Attributes.Append(wordAttribute);
                XmlAttribute weight = catalogue.CreateAttribute("Weight");
                wordNode.Attributes.Append(weight);
                root.AppendChild(wordNode);

                wordAttribute.InnerText = keywordKvp.Key;

                if (keywordKvp.Value.Manifests.Count != 1)
                    throw new NotSupportedException(
                        "There must be exactly 1 manifest for keyword: " + keywordKvp.Key);

                // HACK: get the first listing from the manifest dictionary
                KeywordManifest manifest = null;
                foreach (KeyValuePair<int, KeywordManifest> manifestKvp in keywordKvp.Value.Manifests)
                    manifest = manifestKvp.Value;

                weight.InnerText = manifest.Weight.ToString();
            }

            StringReader reader = new StringReader(catalogue.InnerXml);
            return new SqlXml(new XmlTextReader(reader));
        }
    }
}
