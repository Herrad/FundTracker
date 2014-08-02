using FundTracker.Domain;

namespace FundTracker.Data
{
    public interface ICacheThings<in TKey, TValue>
    {
        void Store(TKey key, TValue value);
        TValue Get(TKey key);

        bool EntryExistsFor(TKey key);
    }
}