using System;

namespace FundTracker.Domain
{
    public interface IKnowAboutAvailableFunds : IHaveRecurringChanges
    {
        void ReportFundsOn(DateTime selectedDate);
    }
}