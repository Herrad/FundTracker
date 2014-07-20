namespace FundTracker.Domain
{
    public interface IAmIdentifiable
    {
        WalletIdentification Identification { get; }
    }
}