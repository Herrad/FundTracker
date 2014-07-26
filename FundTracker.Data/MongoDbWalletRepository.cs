using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Data.Mappers;
using FundTracker.Domain;
using FundTracker.Services;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class MongoDbWalletRepository : ISaveWallets, IKnowAboutWallets
    {
        private readonly IMapMongoWalletsToWallets _mongoWalletToWalletMapper;
        private readonly ICacheThings<WalletIdentification, Wallet> _cache;
        private readonly IProvideMongoCollections _databaseAdapter;
        private readonly IProvideMongoWallets _walletReadRepository;
        private readonly IKnowWhichChangesBelongToWallets _recurringChangeRepository;

        public MongoDbWalletRepository(
            IMapMongoWalletsToWallets mongoWalletToWalletMapper, 
            ICacheThings<WalletIdentification, Wallet> cache, 
            IProvideMongoCollections databaseAdapter, 
            IInflateMongoRecurringChanges mongoRecurringChangeMapper,
            IProvideMongoWallets walletReadRepository, 
            IKnowWhichChangesBelongToWallets recurringChangeRepository)
        {
            _mongoWalletToWalletMapper = mongoWalletToWalletMapper;
            _cache = cache;
            _databaseAdapter = databaseAdapter;
            _walletReadRepository = walletReadRepository;
            _recurringChangeRepository = recurringChangeRepository;
        }


        public Wallet Get(WalletIdentification identification)
        {
            var cachedWallet = _cache.Get(identification);
            if (cachedWallet != null)
            {
                return cachedWallet;
            }

            var mongoWallet = _walletReadRepository.GetMongoWallet(identification);
            var mongoRecurringChanges = _recurringChangeRepository.GetAllRecurringChangesFor(mongoWallet);

            var inflatedWallet = _mongoWalletToWalletMapper.InflateWallet(mongoWallet, mongoRecurringChanges);
            Cache(inflatedWallet);
            return inflatedWallet;
        }

        private void Cache(Wallet wallet)
        {
            _cache.Store(wallet.Identification, wallet);
        }

        public void Save(IAmIdentifiable wallet)
        {
            _databaseAdapter.GetCollection<MongoWallet>("Wallets").Insert(new MongoWallet
            {
                Name = wallet.Identification.Name
            });
        }
    }
}