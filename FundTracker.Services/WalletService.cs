using FundTracker.Domain;

namespace FundTracker.Services
{
    public class WalletService : IProvideWallets, IStoreCreatedWallets
    {
        private readonly IKnowAboutWallets _walletRepository;
        private readonly ISaveWallets _walletSaver;

        public WalletService(IKnowAboutWallets walletRepository, ISaveWallets walletSaver)
        {
            _walletRepository = walletRepository;
            _walletSaver = walletSaver;
        }

        public IWallet FindFirstWalletWith(WalletIdentification walletIdentification)
        {
            return GetWallet(walletIdentification);
        }

        private IWallet GetWallet(WalletIdentification walletIdentification)
        {
            return _walletRepository.Get(walletIdentification);
        }

        public IHaveRecurringChanges FindRecurringChanger(WalletIdentification walletIdentification)
        {
            return GetWallet(walletIdentification);
        }

        public IHaveChangingFunds FindFundChanger(WalletIdentification walletIdentification)
        {
            return GetWallet(walletIdentification);
        }

        public void Add(IWallet wallet)
        {
            _walletSaver.Save(wallet);
        }
    }
}