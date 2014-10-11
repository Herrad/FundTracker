using System;

namespace FundTracker.Domain
{
    public interface IKnowAboutAvailableFunds : IHaveRecurringChanges
    {
        decimal GetAvailableFundsOn(DateTime targetDate);
    }
}