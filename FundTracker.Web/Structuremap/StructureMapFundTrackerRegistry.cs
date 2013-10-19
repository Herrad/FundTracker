using System;
using FundTracker.Services;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace FundTracker.Web.Structuremap
{
    public class StructureMapFundTrackerRegistry : Registry
    {
        public StructureMapFundTrackerRegistry()
        {
            Scan(
                x =>
                    {
                        x.AssemblyContainingType<WalletService>();
                        x.TheCallingAssembly();
                        x.SingleImplementationsOfInterface();
                    });
        }
    }
}