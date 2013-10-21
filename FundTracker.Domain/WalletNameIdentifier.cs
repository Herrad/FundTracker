using System.Collections.Generic;
using System.Linq;

namespace FundTracker.Domain
{
    public class WalletNameIdentifier : IIdentifyWallets
    {
        private readonly string _name;

        public WalletNameIdentifier(string name)
        {
            _name = name;
        }

        public IWallet FindFirstMatchingWalletIn(IEnumerable<IWallet> wallets)
        {
            return wallets.First(wallet => wallet.Name == _name);
        }
    }
}