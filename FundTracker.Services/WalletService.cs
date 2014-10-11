using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using FundTracker.Data;
using FundTracker.Data.Entities;
using FundTracker.Data.Mappers;
using FundTracker.Domain;
using FundTracker.Services.Annotations;

namespace FundTracker.Services
{
    [UsedImplicitly]
    public class WalletService : IProvideWallets, IStoreCreatedWallets
    {
        private readonly IProvideMongoCollections _mongoCollectionProvider;
        private readonly IMapMongoWalletsToWallets _mongoWalletToWalletMapper;
        private readonly IKnowWhichChangesBelongToWallets _walletChangeIdentifier;
        private readonly IProvideMongoWallets _walletReadRepository;
        private readonly ICacheThings<WalletIdentification, Wallet> _cache;

        public WalletService(IProvideMongoCollections mongoCollectionProvider,
            IMapMongoWalletsToWallets mongoWalletToWalletMapper, 
            IKnowWhichChangesBelongToWallets walletChangeIdentifier, 
            IProvideMongoWallets walletReadRepository, 
            ICacheThings<WalletIdentification, Wallet> cache)
        {
            _mongoCollectionProvider = mongoCollectionProvider;
            _mongoWalletToWalletMapper = mongoWalletToWalletMapper;
            _walletChangeIdentifier = walletChangeIdentifier;
            _walletReadRepository = walletReadRepository;
            _cache = cache;
        }

        public IHaveRecurringChanges FindRecurringChanger(WalletIdentification walletIdentification)
        {
            return GetWallet(walletIdentification);
        }

        public IKnowAboutAvailableFunds FindFundChanger(WalletIdentification walletIdentification)
        {
            return GetWallet(walletIdentification);
        }

        public void Add(IAmIdentifiable wallet)
        {
            _mongoCollectionProvider.GetCollection<MongoWallet>("Wallets").Insert(new MongoWallet
            {
                Name = wallet.Identification.Name
            });
        }

        private Wallet GetWallet(WalletIdentification identification)
        {
            if (_cache.EntryExistsFor(identification))
            {
                return _cache.Get(identification);
            }

            var mongoWallet = _walletReadRepository.GetMongoWallet(identification);
            var mongoRecurringChanges = _walletChangeIdentifier.GetAllRecurringChangesFor(mongoWallet);

            var inflatedWallet = InflateAndCacheWallet(identification, mongoWallet, mongoRecurringChanges);
            return inflatedWallet;
        }

        private Wallet InflateAndCacheWallet(WalletIdentification identification, MongoWallet mongoWallet,
            IEnumerable<MongoRecurringChange> mongoRecurringChanges)
        {
            var inflatedWallet = _mongoWalletToWalletMapper.InflateWallet(mongoWallet, mongoRecurringChanges);
            _cache.Store(identification, inflatedWallet);
            return inflatedWallet;
        }
    }
}