using System;

namespace FundTracker.Domain
{
    public interface IHaveChangingFunds : IAmIdentifiable
    {
        decimal GetAvailableFundsFor(DateTime targetDate);
    }
}