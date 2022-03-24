using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMango
{
    public class EasyMango
    {
        private readonly IMongoCollection<BsonDocument> collection;

        public enum SortingOrder
        {
            Default,
            Ascending,
            Descending
        }

        public EasyMango(string connectionString, string databaseName, string collectionName)
        {
            // Connect to the MongoDB Database
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(databaseName);
            collection = database.GetCollection<BsonDocument>(collectionName);
        }

        public bool GetSingleDatabaseEntry<T>(string field, T value, out BsonDocument result)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);

            result = collection.Find(filter).FirstOrDefault();

            return (result != null);
        }

        public bool GetSingleDatabaseEntry<T>(string field, T value, SortingOrder sortingOrder, string sortingField,
            out BsonDocument result)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);

            switch (sortingOrder)
            {
                case SortingOrder.Ascending:
                    result = collection.Find(filter).SortBy(bson => bson[sortingField]).FirstOrDefault();
                    break;
                case SortingOrder.Descending:
                    result = collection.Find(filter).SortByDescending(bson => bson[sortingField]).FirstOrDefault();
                    break;
                default:
                    GetSingleDatabaseEntry(field, value, out result);
                    break;
            }

            return (result != null);
        }

        public bool GetMultipleDatabaseEntries<T>(string field, T value, out List<BsonDocument> result)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);
            result = collection.Find(filter).ToList();

            return (result != null);
        }

        public bool GetMultipleDatabaseEntries<T>(string field, T value, SortingOrder sortingOrder, string sortingField,
            out List<BsonDocument> result)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);

            switch (sortingOrder)
            {
                case SortingOrder.Ascending:
                    result = collection.Find(filter).SortBy(bson => bson[sortingField]).ToList();
                    break;
                case SortingOrder.Descending:
                    result = collection.Find(filter).SortByDescending(bson => bson[sortingField]).ToList();
                    break;
                default:
                    GetMultipleDatabaseEntries(field, value, out result);
                    break;
            }

            return (result != null);
        }

        public bool ReplaceSingleDatabaseEntry<T>(string field, T value, BsonDocument entry)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);
            try
            {
                collection.FindOneAndReplace(filter, entry);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool ReplaceMultipleDatabaseEntries<T>(string field, T value, BsonDocument entry)
        {
            if (!GetMultipleDatabaseEntries(field, value, out List<BsonDocument> result))
            {
                return false;
            }

            try
            {
                foreach (BsonDocument document in result)
                {
                    FilterDefinition<BsonDocument> filter =
                        Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));
                    collection.FindOneAndReplace(filter, entry);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool AddSingleDatabaseEntry(BsonDocument entry)
        {
            try
            {
                collection.InsertOne(entry);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool AddMultipleDatabaseEntries(List<BsonDocument> entries)
        {
            try
            {
                collection.InsertMany(entries);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool RemoveSingleDatabaseEntry<T>(string field, T value)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);
            try
            {
                collection.FindOneAndDelete(filter);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool RemoveMultiplesDatabaseEntries<T>(string field, T value)
        {
            if (!GetMultipleDatabaseEntries(field, value, out List<BsonDocument> result))
            {
                return false;
            }

            try
            {
                foreach (BsonDocument document in result)
                {
                    FilterDefinition<BsonDocument> filter =
                        Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));
                    collection.FindOneAndDelete(filter);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}