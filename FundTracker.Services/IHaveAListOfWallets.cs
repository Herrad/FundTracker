using System.Collections.Generic;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IHaveAListOfWallets
    {
        List<Wallet> Wallets { get; }
    }
}