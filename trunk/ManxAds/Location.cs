using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;

namespace ManxAds
{
    public class Location
    {
        private int databaseId;
        private string name;

        public int DatabaseId
        {
            get { return databaseId; }
            set { databaseId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static Dictionary<int, string> Directory
        {
            get
            {
                Dictionary<int, string> directory = new Dictionary<int, string>();
                using (StoredProceedure sp = new StoredProceedure("LocationFetch"))
                {
                    sp.Connection.Open();
                    sp.Reader = sp.Command.ExecuteReader();

                    while (sp.Reader.Read())
                    {
                        directory.Add(
                            sp.GetReaderValue<int>("LocationId"),
                            sp.GetReaderValue<string>("Location"));
                    }
                }
                return directory;
            }
        }

        public Location(int databaseId, string name)
        {
            this.databaseId = databaseId;
            this.name = name;
        }

        public static Location Fetch(int id)
        {
            Location location;
            using (StoredProceedure sp = new StoredProceedure("LocationFetchById"))
            {
                sp.AddParam("@LocationId", id);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader(CommandBehavior.SingleRow);
                sp.Reader.Read();

                location = new Location(
                    sp.GetReaderValue<int>("LocationId"),
                    sp.GetReaderValue<string>("Location"));
            }
            return location;
        }

        /// <summary>
        /// Fetches a location which matches the specified title string.
        /// </summary>
        /// <returns>Matching location.</returns>
        public static Location FetchByTitleString(string title)
        {
            using (StoredProceedure sp = new StoredProceedure("LocationFetchByTitle"))
            {
                sp.AddParam("@Title", title);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader(CommandBehavior.SingleRow);
                sp.Reader.Read();

                return new Location(
                    sp.GetReaderValue<int>("LocationId"),
                    sp.GetReaderValue<string>("Location"));
            }
        }

        public static void DataBind(ListControl control, int selectedIndex, string topItem)
        {
            DataBind(control, selectedIndex, new ListItem[] { new ListItem(topItem, "-1") });
        }

        public static void DataBind(ListControl control, int selectedIndex, ListItem[] prefixItems)
        {
            List<ListItem> listItemList = new List<ListItem>(prefixItems);
            foreach (KeyValuePair<int, string> kvp in Directory)
            {
                ListItem item = new ListItem(kvp.Value, kvp.Key.ToString());
                if (selectedIndex == kvp.Key)
                {
                    item.Selected = true;
                }
                listItemList.Add(item);
            }
            control.Items.Clear();
            control.Items.AddRange(listItemList.ToArray());
        }
    }
}
