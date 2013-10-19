using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IProvideWallets
    {
        IWallet GetBy(string name);
    }
}