using MongoDB.Bson;

namespace FundTracker.Data.Entities
{
    public class MongoWallet
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}
