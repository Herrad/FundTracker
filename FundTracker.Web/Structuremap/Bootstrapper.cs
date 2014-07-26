using System.Web.Mvc;
using FundTracker.Data;
using FundTracker.Data.Annotations;
using FundTracker.Data.Mappers;
using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;
using FundTracker.Services;
using FundTracker.Web.Controllers.ActionHelpers;
using MicroEvent;
using StructureMap;
using StructureMap.Pipeline;

namespace FundTracker.Web.Structuremap
{
    [UsedImplicitly]
    public class Bootstrapper
    {
        public static void Run()
        {
            var registry = new StructureMapFundTrackerRegistry();
            var eventBus = new EventBusFactory().BuildEventBus();

            ObjectFactory.Configure(x =>
                {
                    x.AddRegistry(registry);

                    x.For<IRedirectBasedOnWalletCreationValidation>().Use<CreateWalletValidationRules>();
                    x.For<IProvideWallets>().Use<WalletService>();
                    x.For<IValidateWalletNames>().Use<WalletNameValidator>();
                    x.For<IStoreCreatedWallets>().Use<WalletService>();
                    x.For<ICreateWallets>().Use<WalletBuilder>();
                    x.For<ISaveWallets>().Use<MongoDbWalletRepository>();
                    x.For<IKnowAboutWallets>().Use<MongoDbWalletRepository>();
                    x.For<IMapMongoWalletsToWallets>().Use<MongoWalletToWalletMapper>();
                    x.For<IMapMongoRecurringChangesToRecurringChanges>().Use<MongoRecurringChangeToRecurringChangeMapper>();
                    x.For<IBuildRecurranceSpecifications>().Use<RecurranceSpecificationFactory>();
                    x.For<ICacheThings<WalletIdentification, Wallet>>().Use<InMemoryWalletCache>();
                    x.For<IProvideMongoCollections>().Use<DatabaseAdapter>();
                    x.For<IInflateMongoRecurringChanges>().Use<MongoRecurringChangeMapper>();
                    x.For<IProvideMongoWallets>().Use<WalletReadRepository>();
                    x.For<IKnowWhichChangesBelongToWallets>().Use<MongoDbRecurringChangeRepository>();
                    x.For<IReceivePublishedEvents>().LifecycleIs(new SingletonLifecycle()).Use(() => eventBus);
                    x.For<IReadSubscriptions>().LifecycleIs(new SingletonLifecycle()).Use(() => eventBus);
                    x.For<Subscription>().OnCreationForAll(eventBus.Subscribe);
                });
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }
    }
}