using System.Web.Mvc;
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
                    x.For<IValidateWalletCreation>().Use<CreateWalletValidation>();
                });
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }
    }
}