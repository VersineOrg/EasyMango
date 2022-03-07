using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMango;

public class EasyMango
{
    private IMongoCollection<BsonDocument> collection;
    
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

    public bool GetSingleDatabaseEntry(string field, string value, out BsonDocument result)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);
        
        result = collection.Find(filter).FirstOrDefault();

        return (result != null);
    }
    public bool GetSingleDatabaseEntry(string field, string value,SortingOrder sortingOrder,string sortingField, out BsonDocument result)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);

        switch (sortingOrder)
        {
            case SortingOrder.Ascending:
                result = collection.Find(filter).SortBy(bson => bson[sortingField]).FirstOrDefault();
                break;
            case SortingOrder.Descending: 
                result= collection.Find(filter).SortByDescending(bson => bson[sortingField]).FirstOrDefault();
                break;
            default:
                result = collection.Find(filter).FirstOrDefault();
                break;
        }

        return (result != null);
    }
    public bool GetMultipleDatabaseEntries(string field, string value, out List<BsonDocument> result)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(field, value);
        result = collection.Find(filter).ToList();

        return (result != null);
    }
    
    public bool ReplaceSingleDatabaseEntry(string field, string value, BsonDocument entry)
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
    public bool ReplaceMultipleDatabaseEntries(string field, string value, BsonDocument entry)
    {
        List<BsonDocument> result = new List<BsonDocument>();
        if (!GetMultipleDatabaseEntries(field, value, out result))
        {
            return false;
        }
        try
        {
            foreach (BsonDocument document in result)
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));
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
    
    public bool RemoveSingleDatabaseEntry(string field, string value)
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
    public bool RemoveMultiplesDatabaseEntries(string field, string value)
    {
        List<BsonDocument> result = new List<BsonDocument>();
        if (!GetMultipleDatabaseEntries(field, value, out result))
        {
            return false;
        }
        try
        {
            foreach (BsonDocument document in result)
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));
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