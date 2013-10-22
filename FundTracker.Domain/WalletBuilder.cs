namespace FundTracker.Domain
{
    public class WalletBuilder : ICreateWallets
    {
        private readonly IStoreCreatedWalets _walletStore;

        public WalletBuilder(IStoreCreatedWalets walletStore)
        {
            _walletStore = walletStore;
        }

        public void CreateWallet(WalletIdentification walletIdentification)
        {
            _walletStore.Add(new Wallet(walletIdentification));
        }
    }
}