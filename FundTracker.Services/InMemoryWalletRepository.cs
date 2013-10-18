using System.Collections.Generic;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public class InMemoryWalletRepository : IHaveAListOfWallets
    {
        public InMemoryWalletRepository()
        {
            Wallets = new List<Wallet>();
        }

        public List<Wallet> Wallets { get; private set; }
    }
}