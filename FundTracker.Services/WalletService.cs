using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public class WalletService : IProvideWallets, IStoreCreatedWalets
    {
        private readonly IHaveAListOfWallets _walletRepository;

        public WalletService(IHaveAListOfWallets walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public IWallet FindFirstWalletWith(WalletIdentification walletIdentification)
        {
            return _walletRepository.Wallets.First(wallet => Equals(wallet.Identification, walletIdentification));
        }

        public void Add(IWallet wallet)
        {
            _walletRepository.Wallets.Add(wallet);
        }
    }
}