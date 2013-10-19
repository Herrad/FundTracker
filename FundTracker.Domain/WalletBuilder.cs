namespace FundTracker.Domain
{
    public class WalletBuilder : ICreateWallets
    {
        private readonly IStoreCreatedWalets _walletStore;

        public WalletBuilder(IStoreCreatedWalets walletStore)
        {
            _walletStore = walletStore;
        }

        public void CreateWallet(string name)
        {
            _walletStore.Add(new Wallet(name));
        }
    }
}