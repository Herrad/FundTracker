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

        public Wallet GetBy(string name)
        {
            return _walletRepository.Wallets.First(wallet => wallet.Name == name);
        }

        public void Add(Wallet wallet)
        {
            _walletRepository.Wallets.Add(wallet);
        }
    }
}