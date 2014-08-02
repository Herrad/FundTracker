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

        public WalletService(IProvideMongoCollections mongoCollectionProvider,
            IMapMongoWalletsToWallets mongoWalletToWalletMapper, 
            IKnowWhichChangesBelongToWallets walletChangeIdentifier, 
            IProvideMongoWallets walletReadRepository)
        {
            _mongoCollectionProvider = mongoCollectionProvider;
            _mongoWalletToWalletMapper = mongoWalletToWalletMapper;
            _walletChangeIdentifier = walletChangeIdentifier;
            _walletReadRepository = walletReadRepository;
        }

        public IHaveRecurringChanges FindRecurringChanger(WalletIdentification walletIdentification)
        {
            return GetWallet(walletIdentification);
        }

        public IHaveChangingFunds FindFundChanger(WalletIdentification walletIdentification)
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
            var mongoWallet = _walletReadRepository.GetMongoWallet(identification);
            var mongoRecurringChanges = _walletChangeIdentifier.GetAllRecurringChangesFor(mongoWallet);

            return _mongoWalletToWalletMapper.InflateWallet(mongoWallet, mongoRecurringChanges);
        }
    }
}