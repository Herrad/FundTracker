using System;
using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Services
{
    public class WalletService : IProvideWallets, IStoreCreatedWalets
    {
        private readonly IHaveAListOfWallets _walletRepository;
        private readonly IValidateWalletNames _nameValidater;

        public WalletService(IHaveAListOfWallets walletRepository, IValidateWalletNames nameValidater)
        {
            _walletRepository = walletRepository;
            _nameValidater = nameValidater;
        }

        public IWallet FindFirstWalletWith(string name)
        {
            ValidateName(name);
            return _walletRepository.Wallets.First(wallet => wallet.Name == name);
        }

        private void ValidateName(string name)
        {
            if (!_nameValidater.IsNameValid(name)) throw new ArgumentException("Name must be non-empty string");
        }

        public void Add(IWallet wallet)
        {
            _walletRepository.Wallets.Add(wallet);
        }
    }
}