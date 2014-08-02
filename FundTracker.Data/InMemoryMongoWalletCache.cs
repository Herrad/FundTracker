using System;
using System.Collections.Generic;
using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using MicroEvent;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class InMemoryMongoWalletCache : InMemoryCache<WalletIdentification, MongoWallet>
    {
        public InMemoryMongoWalletCache()
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