using System.Web.Mvc;
using FundTracker.Data;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.ActionHelpers;
using StructureMap;

namespace FundTracker.Web.Structuremap
{
    public class Bootstrapper
    {
        public static void Run()
        {
            var registry = new StructureMapFundTrackerRegistry();
            ObjectFactory.Configure(x =>
                {
                    x.AddRegistry(registry);

                    x.For<IRedirectBasedOnWalletCreationValidation>().Use<CreateWalletValidation>();
                    x.For<IProvideWallets>().Use<WalletService>();
                    x.For<IValidateWalletNames>().Use<WalletNameValidator>();
                    x.For<IStoreCreatedWalets>().Use<WalletService>();
                    x.For<ICreateWallets>().Use<WalletBuilder>();
                    x.For<ISaveWallets>().Use<MongoDbWalletRepository>();
                    x.For<IKnowAboutWallets>().Use<MongoDbWalletRepository>();
                    x.For<IMapMongoWalletsToWallets>().Use<MongoWalletToWalletMapper>();
                });
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }
    }
}