using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Collections.Generic;

namespace CoreSIMPLECRM.DataLayer
{

    public class DataStorage
    {
        private readonly IMongoDatabase _database;

        public DataStorage(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public IEnumerable<T> ReadData<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(filter).ToList();
        }

        public void WriteData<T>(string collectionName, T data)
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.InsertOne(data);
        }
        public void UpdateData<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.UpdateOne(filter, update);
        }
        public void DeleteData<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.DeleteOne(filter);
        }
    }

}
