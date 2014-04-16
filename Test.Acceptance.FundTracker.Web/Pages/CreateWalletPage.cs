using System;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class CreateWalletPage
    {
        public static void CreateWithAUniqueNameStartingWith(string walletName)
        {
            walletName += Guid.NewGuid();
            var nameBox = WebDriverTests.Driver.FindCss(".name");
            nameBox.SendKeys(walletName);

            var createButton = WebDriverTests.Driver.FindCss(".button");
            createButton.Click();

            var redirectLink = WebDriverTests.Driver.FindCss("a");
            redirectLink.Click();
        }
    }
}