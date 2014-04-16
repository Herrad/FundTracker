using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Test.Acceptance.FundTracker.Web.Data
{
    public class MongoDbAdapter
    {
        public static void CreateWalletCalled(string walletName)
        {
            const string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);

            var mongoServer = client.GetServer();
            var mongoDatabase = mongoServer.GetDatabase("FundTracker");
            var walletsCollection = mongoDatabase.GetCollection("Wallets");

            var walletToInsert = new BsonDocument(new Dictionary<string, object> {{"name", walletName}});
            walletsCollection.Insert(walletToInsert);
        }
    }
}