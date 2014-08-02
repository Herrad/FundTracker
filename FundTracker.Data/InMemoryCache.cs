using System;
using System.Collections.Generic;
using System.Linq;
using MicroEvent;

namespace FundTracker.Data
{
    public abstract class InMemoryCache<TKey, TValue> : Subscription, ICacheThings<TKey, TValue>
    {
        private readonly int _maximumCacheSize;
        private readonly Dictionary<TKey, TValue> _cache;
        protected InMemoryCache(IEnumerable<Type> eventTypes, int maximumCacheSize) : base(eventTypes)
        {
            _maximumCacheSize = maximumCacheSize;
            _cache = new Dictionary<TKey, TValue>();
        }

        public void Store(TKey key, TValue value)
        {
            if (_cache.Count < _maximumCacheSize)
            {
                if (EntryExistsFor(key))
                {
                    Delete(key);
                }
                _cache.Add(key, value);
            }
            else
            {
                var keys = _cache.Keys.ToList();
                Delete(keys[_maximumCacheSize-1]);
            }
        }

        public TValue Get(TKey key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            throw new KeyNotFoundException("Could not find " + key + " in cache.");
        }

        protected void Delete(TKey key)
        {
            if (EntryExistsFor(key))
            {
                _cache.Remove(key);
            }
        }

        public bool EntryExistsFor(TKey key)
        {
            return _cache.ContainsKey(key);
        }
    }
}