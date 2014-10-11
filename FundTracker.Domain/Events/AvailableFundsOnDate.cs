using System;
using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class AvailableFundsOnDate : AnEvent
    {
        public AvailableFundsOnDate(DateTime date, decimal funds)
        {
            Funds = funds;
            Date = date;
        }

        public DateTime Date { get; private set; }
        public decimal Funds { get; private set; }
    }
}