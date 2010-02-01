using System;
using System.Collections.Generic;
using System.Text;

namespace ManxAds.Search
{
    public class KeywordManifest
    {
        private ListingBase listing;
        private float weight;
        private int occurrences = 1;

        public ListingBase Listing
        {
            get { return listing; }
            set { listing = value; }
        }

        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public int Occurrences
        {
            get { return occurrences; }
            set { occurrences = value; }
        }

        public KeywordManifest(ListingBase listing, float weight, int occurrences)
        {
            this.listing = listing;
            this.weight = weight;
            this.occurrences = occurrences;
        }
    }
}