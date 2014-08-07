using System;
using System.Collections.Generic;
using FundTracker.Data.Annotations;
using FundTracker.Domain;
using MicroEvent;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class InMemoryWalletCache : InMemoryCache<WalletIdentification, Wallet>
    {
        public InMemoryWalletCache()
            : base(new List<Type> { typeof(BustCacheForWallet) }, 10)
        {
        }

        public override void Notify(AnEvent anEvent)
        {
            var bust = anEvent as BustCacheForWallet;
            if (bust != null)
            {
                var cacheBust = bust;
                Delete(cacheBust.TargetWalletIdentification);
            }
        }
    }
}