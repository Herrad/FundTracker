using System.Configuration;
using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class DatabaseAdapter : IProvideMongoCollections
    {

        private readonly string _connectionString;
        private readonly string _databaseName;

        public DatabaseAdapter()
        {
            _connectionString = ConfigurationManager.AppSettings["MongoConnectionString"];
            _databaseName = ConfigurationManager.AppSettings["DatabaseName"];
        }

        private MongoDatabase GetMongoDatabase()
        {
            var mongoClient = new MongoClient(_connectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(_databaseName);
            return mongoDatabase;
        }

        public MongoCollection<T> GetCollection<T>(string collectionName)
        {
            return GetMongoDatabase().GetCollection<T>(collectionName);
        }
    }
}