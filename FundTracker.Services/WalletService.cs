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
            return _walletRepository.Get(walletIdentification);
        }

        public void Add(IWallet wallet)
        {
            _walletSaver.Save(wallet);
        }
    }
}