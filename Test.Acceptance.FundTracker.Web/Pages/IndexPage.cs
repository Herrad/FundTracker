using System;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class IndexPage
    {
        public static void CreateWalletWithAUniqueNameStartingWith(string walletName)
        {
            walletName = UniqueifyWalletName(walletName);
            ScenarioContext.Current["wallet name"] = walletName;

            EnterNameTo(walletName, ".create-name");

            var createButton = WebDriverTests.WebDriver.FindCss(".create-submit");
            createButton.Click();

            var redirectLink = WebDriverTests.WebDriver.FindCss("a");
            redirectLink.Click();
        }

        private static string UniqueifyWalletName(string walletName)
        {
            return walletName + Guid.NewGuid();
        }

        public static AdministerWalletPage SubmitSearchForWalletCalled(string walletName)
        {
            ScenarioContext.Current["wallet name"] = walletName;
            EnterNameTo(walletName, ".find-name");

            var createButton = WebDriverTests.WebDriver.FindCss(".find-submit");
            createButton.Click();

            return new AdministerWalletPage();
        }

        private static void EnterNameTo(string walletName, string targetNameBox)
        {
            var nameBox = WebDriverTests.WebDriver.FindCss(targetNameBox);
            nameBox.SendKeys(walletName);
        }
    }
}