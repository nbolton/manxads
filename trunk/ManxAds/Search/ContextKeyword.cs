using System;
using System.Collections.Generic;
using System.Text;

namespace ManxAds.Search
{
    /// <summary>
    /// A keyword used in a specific context (i.e. per listing). This should
    /// be weighted according to it's parent object.
    /// </summary>
    public class ContextKeyword
    {
        public string Name;
        public float Weight;

        public ContextKeyword(string name, float weight)
        {
            this.Name = name;
            this.Weight = weight;
        }
    }
}
