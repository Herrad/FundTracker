using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public class WalletService : IProvideWallets
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
    }
}