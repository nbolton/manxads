using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;
using System.Xml;

namespace ManxAds.Search
{
    public class Keyword
    {
        private const float repeatWordWeightFactor = .6F;

        private int knownId = -1;
        private string name;
        private Dictionary<int, KeywordManifest> manifiestList;

        /// <summary>
        /// Gets or sets the database ID, if known (-1 is returned if unknown).
        /// </summary>
        public int KnownId
        {
            get { return knownId; }
            set { knownId = value; }
        }

        public float TotalWeight
        {
            get
            {
                float weight = 0;
                foreach (KeyValuePair<int, KeywordManifest> kvp in manifiestList)
                {
                    weight += kvp.Value.Weight;
                }
                return weight;
            }
        }

        /// <summary>
        /// [Deprecated] Gets the string ID list as integers.
        /// </summary>
        public List<int> ListingIDs
        {
            get
            {
                List<int> list = new List<int>();
                foreach (KeyValuePair<int, KeywordManifest> kvp in manifiestList)
                {
                    list.Add(kvp.Key);
                }
                return list;
            }
        }

        public Dictionary<int, KeywordManifest> Manifests
        {
            get { return manifiestList; }
        }

        public string Name
        {
            get { return name; }
        }

        public Keyword(string name)
        {
            this.name = name;
            this.manifiestList = new Dictionary<int, KeywordManifest>();
        }

        public Keyword(string name, IListing initialListing, float initialWeight)
            : this(name)
        {
            this.AddListing(initialListing, initialWeight, 1);
        }

        /// <summary>
        /// Add a listing and determin weight factor based on factor
        /// number and number of occurances in keyword.
        /// </summary>
        public void AddListing(IListing listing, float weightFactor, int reverseThreshold)
        {
            KeywordManifest keywordManifest =
                new KeywordManifest(listing, weightFactor, 1);

            if (!manifiestList.ContainsKey(listing.DatabaseId))
            {
                manifiestList.Add(listing.DatabaseId, keywordManifest);
            }
            else
            {
                // Use exponential ranking for repeated keywords (reverses on threshold).
                int occurrences = ++manifiestList[listing.DatabaseId].Occurrences;
                double difference = (reverseThreshold - occurrences) * repeatWordWeightFactor;

                int weightIncrement = (int)(weightFactor * difference);
                manifiestList[listing.DatabaseId].Weight += weightIncrement;
            }
        }

        /// <summary>
        /// Add a manifest with a predetermined weight.
        /// </summary>
        public void AddManifiest(KeywordManifest manifest)
        {
            manifiestList.Add(manifest.Listing.DatabaseId, manifest);
        }

        public override string ToString()
        {
            throw new NotSupportedException();
        }
    }
}
