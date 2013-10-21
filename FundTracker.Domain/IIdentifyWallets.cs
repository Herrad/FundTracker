using System.Collections.Generic;

namespace FundTracker.Domain
{
    public interface IIdentifyWallets
    {
        IWallet FindFirstMatchingWalletIn(IEnumerable<IWallet> wallets);
    }
}