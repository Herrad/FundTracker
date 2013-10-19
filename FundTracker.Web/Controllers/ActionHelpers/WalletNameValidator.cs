using System;

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