using System;

namespace FundTracker.Domain
{
    public interface IHaveChangingFunds : IAmIdentifiable
    {
        void AddFunds(decimal fundsToAdd);
        decimal AvailableFunds { get; }

        decimal GetAvailableFundsFor(DateTime targetDate);
    }
}