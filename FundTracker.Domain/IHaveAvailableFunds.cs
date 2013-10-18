namespace FundTracker.Domain
{
    public interface IHaveAvailableFunds
    {
        decimal AvailableFunds { get; }
    }
}