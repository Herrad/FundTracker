using System.Collections.Generic;
using FundTracker.Data.Annotations;
using FundTracker.Domain;
using MicroEvent;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class InMemoryWalletCache : Subscription, ICacheThings<WalletIdentification, Wallet>
    {
        private readonly Dictionary<WalletIdentification, Wallet> _cache;

        public InMemoryWalletCache() :base(typeof (BustCacheForWallet))
        {
            _cache = new Dictionary<WalletIdentification, Wallet>();
        }

        public void Delete(WalletIdentification id)
        {
            if (_cache.ContainsKey(id))
            {
                _cache.Remove(id);
            }
        }

        public void Store(WalletIdentification id, Wallet value)
        {
            if (_cache.Count < 10)
            {
                _cache.Add(id, value);
            }
            else
            {
                var ids = new WalletIdentification[10];
                _cache.Remove(ids[9]);
            }
        }

        public Wallet Get(WalletIdentification id)
        {
            if(_cache.ContainsKey(id))
            {
                return _cache[id];
            }
            return null;
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