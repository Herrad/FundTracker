namespace FundTracker.Domain
{
    public interface IHaveFundsThatFrequentlyChange : IHaveChangingFunds, IHaveRecurringChanges
    {
    }
}