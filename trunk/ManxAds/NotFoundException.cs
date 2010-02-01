using System;
using System.Collections.Generic;
using System.Text;

namespace ManxAds
{
    public class NotFoundException : Exception
    {
        public int DatabaseId;

        public NotFoundException() : this(null) { }
        public NotFoundException(int databaseId) : this(databaseId, null) { }

        public NotFoundException(int databaseId, Exception innerException)
            : base(
                "An item with ID of " + databaseId + " was " +
                "requested, but was not found in the database.", innerException)
        {
            this.DatabaseId = databaseId;
        }

        public NotFoundException(Exception innerException)
            : base(
                "An item was requested, but was not found in the " +
                "database. The item was not requested using an ID.", innerException) { }
    }
}
