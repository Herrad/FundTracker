using System;
using MongoDB.Bson;

namespace FundTracker.Data.Entities
{
    public class MongoRecurringChange
    {
        public ObjectId Id { get; set; }
        public decimal Amount { get; set; }
        public ObjectId WalletId { get; set; }
        public string Name { get; set; }
        public string FirstApplicationDate { get; set; }
        public string RecurranceRule { get; set; }
        public string LastApplicationDate { get; set; }
    }
}