# EasyMango
## EasyMango is a simple library that makes working with MongoDB easy

https://www.nuget.org/packages/EasyMango

## setting up

#### instanciate an EasyMango object with the following parameters :

- connectionString : connection string to MongoDb
- databaseName : name of your MongoDB batabase
- collectionName : name of your MongoDB collection

## usage

### all the following methods return true if they were sucessful and false if a problem occured

### - get a single entry from the database

#### to get a single entry from the database, use the getSingleDatabaseEntry method with the following parameters :

- field (string) : the field used to search the entry in the database
- value (any type) : the value of said field
- out result (BsonDocument) : the result of the search as a BsonDocument

#### if you wish to get the first result from a sorted array of the search result, you can use the following parameters :

- field (string) : the field used to search the entry in the database
- value (any type) : the value of said field
- sortingOrder (SortingOrder) : the order to sort with, the available orders are : SortingOrder.Default, SortingOrder.Ascending, SortingOrder.Descending
- sortingField (string) : the field used to sort the results
- out result (BsonDocument) : the result of the search as a BsonDocument

### - get multiple entries from the database

#### to get multiple entries from the database, use the GetMultipleDatabaseEntries method with the same parameters as the getSingleDatabaseEntry method, but change the last parameter from :

out result (BsonDocument)

#### to :

out result (List\<BsonDocument\>)
  
#### you can also sort the resulting list, with the same parameters as the getSingleDatabaseEntry method

### - replace a single entry in the database

#### to replace a single entry in the database, use the ReplaceSingleDatabaseEntry method with the following parameters :

- field (string) : the field used to search the entry in the database
- value (any type) : the value of said field
- entry (BsonDocument) : the new entry to replace the retrieved Entry with

### - replace mutiple entries in the database

#### to replace multiple entries in the database, use the ReplaceMultipleDatabaseEntries method with the with the same parameters as the ReplaceSingleDatabaseEntry method. all the entries that have a *field* equal to the *value*, will be replaced by the *entry*

### - add a single entry to the database

#### to add a single entry to the database, use the AddSingleDatabaseEntry method with the following parameter :

- entry (BsonDocument) : the entry to add to the database

### - add multiple entries to the database

#### to add multiple entries to the database, use the AddSingleDatabaseEntry method with the following parameter :

- entries (List\<BsonDocument\>) : the entries to add to the database

### - remove a single entry from the datatbase

#### to remove a single entry from the database, use the RemoveSingleDatabaseEntry method with the following parameters :

- field (string) : the field used to search the entry in the database
- value (any type) : the value of said field

#### to remove multiple entries from the database, use the RemoveMultiplesDatabaseEntries method with the with the same parameters as the RemoveSingleDatabaseEntry method. all the entries that have a *field* equal to the *value*, will be removed from the database
