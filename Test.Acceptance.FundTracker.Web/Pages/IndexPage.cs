using System;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class IndexPage
    {
        public static void CreateWalletWithAUniqueNameStartingWith(string walletName)
        {
            walletName += Guid.NewGuid();
            var nameBox = WebDriverTests.Driver.FindCss(".name");
            nameBox.SendKeys(walletName);

            var createButton = WebDriverTests.Driver.FindCss(".button");
            createButton.Click();

            var redirectLink = WebDriverTests.Driver.FindCss("a");
            redirectLink.Click();
        }

        public static void FindWalletCalled(string walletName)
        {
            
        }
    }
}