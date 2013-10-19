using System;
using FundTracker.Domain;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class WalletNameValidator : IValidateWalletNames
    {
        public bool IsNameValid(string name)
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}