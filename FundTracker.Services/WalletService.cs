using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public class WalletService : IProvideWallets, IStoreCreatedWalets
    {
        private readonly IHaveAListOfWallets _walletRepository;
        private readonly ISaveWallets _walletSaver;

        public WalletService(IHaveAListOfWallets walletRepository, ISaveWallets walletSaver)
        {
            _walletRepository = walletRepository;
            _walletSaver = walletSaver;
        }

        public IWallet FindFirstWalletWith(WalletIdentification walletIdentification)
        {
            return _walletRepository.Wallets.First(wallet => Equals(wallet.Identification, walletIdentification));
        }

        public void Add(IWallet wallet)
        {
            _walletSaver.Save(wallet);
            _walletRepository.Wallets.Add(wallet);
        }
    }
}