using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;
using System.Xml;
using System.IO;

namespace ManxAds.Search
{
    public class SearchCriteria
    {
        private string phrase;
        private bool anyKeywords;
        private DateTime startDate = SqlDateTime.MinValue.Value;
        private DateTime endDate = SqlDateTime.MaxValue.Value;
        private decimal startPrice = 0M;
        private decimal endPrice = SqlMoney.MaxValue.ToDecimal();
        private int categoryId = -1;
        private int locationId = -1;
        private int sellerId = -1;

        public string Phrase
        {
            get { return phrase; }
        }

        public bool AnyKeywords
        {
            get { return anyKeywords; }
        }

        public DateTime StartDate
        {
            get { return getValidSqlDateTime(startDate); }
        }

        public DateTime EndDate
        {
            get { return getValidSqlDateTime(endDate); }
        }

        public decimal StartPrice
        {
            get { return startPrice; }
        }

        public decimal EndPrice
        {
            get { return endPrice; }
        }

        public int CategoryId
        {
            get { return categoryId; }
        }

        public int LocationId
        {
            get { return locationId; }
        }

        public int SellerId
        {
            get { return sellerId; }
        }

        private SearchCriteria() { }

        public SearchCriteria(string phrase, bool anyKeywords)
        {
            if (String.IsNullOrEmpty(phrase))
            {
                throw new ArgumentNullException("phrase", "Cannot be null or empty.");
            }

            this.phrase = phrase;
            this.anyKeywords = anyKeywords;
        }

        private DateTime getValidSqlDateTime(DateTime dateTime)
        {
            if (dateTime >= SqlDateTime.MaxValue.Value)
            {
                return SqlDateTime.MaxValue.Value;
            }

            if (dateTime <= SqlDateTime.MinValue.Value)
            {
                return SqlDateTime.MinValue.Value;
            }

            return dateTime;
        }

        public void SetStartDate(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                DateTime.TryParse(value, out startDate);
            }
        }

        public void SetEndDate(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                DateTime.TryParse(value, out endDate);
                if ((endDate.Hour == 0) && (endDate.Minute == 0))
                {
                    // Move to last possible second.
                    endDate = endDate.AddHours(23);
                    endDate = endDate.AddMinutes(59);
                    endDate = endDate.AddSeconds(59);
                }
            }
        }

        public void SetStartPrice(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                decimal.TryParse(value, out startPrice);
                if (startPrice < 0)
                {
                    startPrice = 0;
                }
            }
        }

        public void SetEndPrice(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                decimal.TryParse(value, out endPrice);
                if (endPrice < 0)
                {
                    endPrice = 0;
                }
            }
        }

        public void SetCategoryId(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                int.TryParse(value, out categoryId);
            }
        }

        public void SetLocationId(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                int.TryParse(value, out locationId);
            }
        }

        public void SetSellerId(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                int.TryParse(value, out sellerId);
            }
        }

        /*public SqlXml ToXml()
        {
            StringReader reader = null;
            XmlDocument criteraXml = new XmlDocument();
            XmlNode root = criteraXml.CreateElement("Root");
            criteraXml.AppendChild(root);

            if (this.isNull)
            {
                reader = new StringReader(criteraXml.InnerXml);
                return new SqlXml(new XmlTextReader(reader));
            }

            Dictionary<string, string> members = new Dictionary<string, string>();
            members.Add("StartDate", startDate.ToString());
            members.Add("EndDate", endDate.ToString());
            members.Add("StartPrice", startPrice.ToString());
            members.Add("EndPrice", endPrice.ToString());
            members.Add("CategoryId", categoryId.ToString());
            members.Add("LocationId", locationId.ToString());
            members.Add("SellerId", locationId.ToString());

            foreach (KeyValuePair<string, string> kvp in members)
            {
                XmlNode memberNode = criteraXml.CreateElement("Member");
                root.AppendChild(memberNode);

                XmlAttribute key = criteraXml.CreateAttribute("Key");
                memberNode.Attributes.Append(key);

                XmlAttribute value = criteraXml.CreateAttribute("Value");
                memberNode.Attributes.Append(value);

                XmlText keyText = criteraXml.CreateTextNode(kvp.Key);
                key.AppendChild(keyText);

                XmlText valueText = criteraXml.CreateTextNode(kvp.Value);
                value.AppendChild(valueText);
            }

            reader = new StringReader(criteraXml.InnerXml);
            return new SqlXml(new XmlTextReader(reader));
        }*/
    }
}
