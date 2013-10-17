namespace FundTracker.Services
{
    public class WalletService : IProvideWallets
    {
        public Wallet Get()
        {
            return new Wallet();
        }
    }
}