using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace ManxAds
{
    public class PageDescriptions
    {
        private Dictionary<string, string> descriptions;
        private string defaultDescription;

        public string this[string page]
        {
            get
            {
                page = page.ToLower();
                if (this.descriptions.ContainsKey(page))
                {
                    return this.descriptions[page];
                }
                return defaultDescription;
            }
        }

        private string cleanText(string text)
        {
            // Separate the block of text in to new lines.
            string[] splitters = new string[] { Environment.NewLine };
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;
            string[] dirtyLines = text.Split(splitters, options);
            List<string> cleanLines = new List<string>();
            string tempClean;

            // Trim the whitespace off each line.
            foreach (string thisLine in dirtyLines)
            {
                tempClean = thisLine.Trim();
                if (tempClean != String.Empty)
                {
                    cleanLines.Add(tempClean);
                }
            }

            // Reutrned the joined result.
            return String.Join(" ", cleanLines.ToArray());
        }

        public PageDescriptions(string xmlFilePath)
        {
            this.descriptions = new Dictionary<string, string>();
            FileStream stream = new FileStream(
                xmlFilePath, FileMode.Open, FileAccess.Read);
            XmlTextReader reader = new XmlTextReader(stream);

            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "default":
                            defaultDescription = cleanText(reader.ReadString());
                            break;

                        case "description":
                            reader.MoveToAttribute("page");
                            descriptions.Add(
                                reader.Value.ToLower(),
                                cleanText(reader.ReadString()));
                            break;
                    }
                }
            }

            reader.Close();
            stream.Close();
        }
    }
}
