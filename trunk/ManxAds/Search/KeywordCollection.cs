using System;
using System.Collections.Generic;
using System.Text;

namespace ManxAds.Search
{
    public class KeywordCollection
    {
        private float totalWeight;
        private List<Keyword> keywordList;

        public float TotalWeight
        {
            get { return totalWeight; }
        }

        public KeywordCollection()
        {
            keywordList = new List<Keyword>();
        }

        public void Add(Keyword keyword)
        {
            keywordList.Add(keyword);
            totalWeight += keyword.TotalWeight;
        }

        internal string HighlightContext(string details, int limit)
        {
            return details;
        }

        public static int CompareScores(Listing x, Listing y)
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
    }
}
