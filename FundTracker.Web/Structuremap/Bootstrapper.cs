﻿using System.Web.Mvc;
using FundTracker.Data;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.ActionHelpers;
using MicroEvent;
using StructureMap;
using StructureMap.Pipeline;

namespace FundTracker.Web.Structuremap
{
    public class Bootstrapper
    {
        public static void Run()
        {
            var registry = new StructureMapFundTrackerRegistry();
            var eventBus = new EventBusFactory().BuildEventBus();

            ObjectFactory.Configure(x =>
                {
                    x.AddRegistry(registry);

                    x.For<IRedirectBasedOnWalletCreationValidation>().Use<CreateWalletValidation>();
                    x.For<IProvideWallets>().Use<WalletService>();
                    x.For<IValidateWalletNames>().Use<WalletNameValidator>();
                    x.For<IStoreCreatedWallets>().Use<WalletService>();
                    x.For<ICreateWallets>().Use<WalletBuilder>();
                    x.For<ISaveWallets>().Use<MongoDbWalletRepository>();
                    x.For<IKnowAboutWallets>().Use<MongoDbWalletRepository>();
                    x.For<IMapMongoWalletsToWallets>().Use<MongoWalletToWalletMapper>();
                    x.For<IReceivePublishedEvents>().LifecycleIs(new SingletonLifecycle()).Use(() => eventBus);
                    x.For<IReadSubscriptions>().LifecycleIs(new SingletonLifecycle()).Use(() => eventBus);
                    x.For<Subscription>().OnCreationForAll(eventBus.Subscribe);
                });
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }
    }
}