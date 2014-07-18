using System;

namespace FundTracker.Domain
{
    public class RecurringChange
    {
        public RecurringChange(string name, decimal amount, DateTime startDate)
        {
            StartDate = startDate;
            Name = name;
            Amount = amount;
        }

        public string Name { get; private set; }
        public decimal Amount { get; private set; }

        public DateTime StartDate { get; private set; }
    }
}