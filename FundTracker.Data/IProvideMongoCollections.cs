using MongoDB.Driver;

namespace FundTracker.Data
{
    public interface IProvideMongoCollections
    {
        MongoCollection<T> GetCollection<T>(string collectionName);
    }
}