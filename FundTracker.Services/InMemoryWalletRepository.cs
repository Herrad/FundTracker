using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public class InMemoryWalletRepository : IKnowAboutWallets
    {
        public InMemoryWalletRepository()
        {
            Wallets = new List<IWallet>();
        }

        public List<IWallet> Wallets { get; private set; }
        public IWallet Get(WalletIdentification identification)
        {
            return Wallets.First(wallet => Equals(wallet.Identification, identification));
        }
    }
}