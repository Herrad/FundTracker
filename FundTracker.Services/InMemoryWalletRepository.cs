using System.Collections.Generic;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public class InMemoryWalletRepository : IHaveAListOfWallets
    {
        public InMemoryWalletRepository()
        {
            Wallets = new List<IWallet>();
        }

        public List<IWallet> Wallets { get; private set; }
    }
}