using System.Collections.Generic;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IKnowAboutWallets
    {

        IWallet Get(WalletIdentification identification);
    }
}