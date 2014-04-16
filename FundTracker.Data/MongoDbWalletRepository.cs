using FundTracker.Data.Entities;
using FundTracker.Domain;
using FundTracker.Services;
using MongoDB.Driver;

namespace FundTracker.Data
{
    public class MongoDbWalletRepository : ISaveWallets
    {
        const string ConnectionString = "mongodb://localhost";

        public void Save(IWallet wallet)
        {
            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase("FundTracker");
            var walletCollection = mongoDatabase.GetCollection<MongoWallet>("Wallets");
            walletCollection.Insert(new MongoWallet
            {
                Name = wallet.Identification.Name
            });
        }
    }
}