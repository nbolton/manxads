using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ManxAds
{
    /// <summary>
    /// Represents a relationship between a category and an advert,
    /// and how often the advert rotates on the page.
    /// </summary>
    public class CategoryAdvert
    {
        private int databaseId;
        private int advertId;
        private int categoryId;
        private int rotateFrequency;

        public int DatabaseId
        {
            get { return databaseId; }
        }

        public int RotateFrequency
        {
            get { return rotateFrequency; }
            set { rotateFrequency = value; }
        }

        public Category Category
        {
            get { return Category.Fetch(categoryId); }
        }

        public Advert Advert
        {
            get { return Advert.Fetch(advertId); }
        }

        public CategoryAdvert(
            Advert advert,
            Category category,
            int rotateFrequency)
        {
            this.advertId = advert.DatabaseId;
            this.categoryId = category.DatabaseId;
            this.rotateFrequency = rotateFrequency;
        }

        internal CategoryAdvert(
            int advertId,
            int categoryId,
            int rotateFrequency)
        {
            this.advertId = advertId;
            this.categoryId = categoryId;
            this.rotateFrequency = rotateFrequency;
        }

        internal CategoryAdvert(
            int databaseId,
            int advertId,
            int categoryId,
            int rotateFrequency)
            : this (
            advertId,
            categoryId,
            rotateFrequency)
        {
            this.databaseId = databaseId;
        }

        internal CategoryAdvert(StoredProceedure sp)
            : this(sp, sp.GetReaderValue<int>("CategoryAdvertId")) { }

        internal CategoryAdvert(StoredProceedure sp, int databaseId)
            : this(
            databaseId,
            sp.GetReaderValue<int>("AdvertId"),
            sp.GetReaderValue<int>("CategoryId"),
            sp.GetReaderValue<int>("RotateFrequency")) { }

        public static List<CategoryAdvert> Fetch(Advert advert)
        {
            List<CategoryAdvert> caList = new List<CategoryAdvert>();
            using (StoredProceedure sp = new StoredProceedure("CategoryAdvertFetchByAdvertId"))
            {
                sp.AddParam("@AdvertId", advert.DatabaseId);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    caList.Add(new CategoryAdvert(sp));
                }
            }
            return caList;
        }
    }
}
