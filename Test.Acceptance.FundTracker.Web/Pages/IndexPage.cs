using System;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class IndexPage
    {
        public static void CreateWalletWithAUniqueNameStartingWith(string walletName)
        {
            EnterNameTo(walletName, ".create-name");

            var createButton = WebDriverTests.Driver.FindCss(".create-submit");
            createButton.Click();

            var redirectLink = WebDriverTests.Driver.FindCss("a");
            redirectLink.Click();
        }

        public static void SubmitSearchForWalletCalled(string walletName)
        {
            EnterNameTo(walletName, ".find-name");

            var createButton = WebDriverTests.Driver.FindCss(".find-submit");
            createButton.Click();
        }

        private static void EnterNameTo(string walletName, string targetNameBox)
        {
            walletName += Guid.NewGuid();
            ScenarioContext.Current["wallet name"] = walletName;
            var nameBox = WebDriverTests.Driver.FindCss(targetNameBox);
            nameBox.SendKeys(walletName);
        }
    }
}