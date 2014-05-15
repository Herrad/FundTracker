using MongoDB.Bson;

namespace FundTracker.Data.Entities
{
    public class MongoRecurringChange
    {
        public ObjectId Id { get; set; }
        public decimal Amount { get; set; }
        public ObjectId WalletId { get; set; }
    }
}