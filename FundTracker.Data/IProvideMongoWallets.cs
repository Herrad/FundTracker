using FundTracker.Data.Entities;
using FundTracker.Domain;

namespace FundTracker.Data
{
    public interface IProvideMongoWallets
    {
        MongoWallet GetMongoWallet(IProvideMongoCollections databaseAdapter, WalletIdentification identification);
    }
}