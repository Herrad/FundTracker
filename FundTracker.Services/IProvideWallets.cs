using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IProvideWallets
    {
        Wallet GetBy(string name);
    }
}