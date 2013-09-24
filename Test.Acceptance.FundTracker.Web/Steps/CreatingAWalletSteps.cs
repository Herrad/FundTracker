﻿using System.Globalization;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class CreatingAWalletSteps : ChromeDriverTest
    {
        [BeforeFeature]
        public static void SetUp()
        {
            SetUpSeleniumOnChrome();
        }

        [Given(@"I have created a wallet called ""(.*)""")]
        public void GivenIHaveCreatedAWalletCalled(string walletName)
        {
            CreateAWalletWithTheName(walletName);
        }

        [Given(@"my available funds are (.*)")]
        public void GivenMyAvailableFundsAre(decimal expectedAvailableFunds)
        {
            AvailableFundsShouldBe(expectedAvailableFunds);
        }

        private static void AvailableFundsShouldBe(decimal expectedAvailableFunds)
        {
            var availableFunds = _chromeDriver.FindElementById("available-funds").Text;
            Assert.That(availableFunds, Is.EqualTo(expectedAvailableFunds.ToString(CultureInfo.InvariantCulture)));
        }

        [When(@"I create a wallet with the name ""(.*)""")]
        public void CreateAWalletWithTheName(string walletName)
        {
            var nameBox = _chromeDriver.FindElementByName("name");
            nameBox.SendKeys(walletName);

            var createButton = _chromeDriver.FindElementByClassName("button");
            createButton.Click();

            var redirectLink = _chromeDriver.FindElementByPartialLinkText("Click here");
            redirectLink.Click();
        }

        [When(@"I add (.*) in funds to my wallet")]
        public void WhenIAddInFundsToMyWallet(decimal fundAmount)
        {
            var addFundsAmount = _chromeDriver.FindElementByName("fund-amount-to-add");
            addFundsAmount.SendKeys(fundAmount.ToString(CultureInfo.InvariantCulture));

            var addFundsButton = _chromeDriver.FindElementByName("submit");
            addFundsButton.Click();
        }

        [Then(@"I am taken to the display wallet page")]
        public void ThenIAmTakenToTheDisplayWalletPage()
        {
            var pageTitle = _chromeDriver.FindElementByTagName("h2").Text;
            Assert.That(pageTitle, Is.EqualTo("Wallet"));
        }

        [Then(@"the name ""(.*)"" is displayed")]
        public void ThenTheNameIsDisplayed(string expectedWalletName)
        {
            var walletNameDisplayed = _chromeDriver.FindElementByClassName("wallet-name").Text;
            Assert.That(walletNameDisplayed, Is.EqualTo(expectedWalletName));
        }

        [Then(@"the amount in the wallet is (.*)")]
        public void ThenTheAmountInTheWalletIs(decimal expectedFunds)
        {
            AvailableFundsShouldBe(expectedFunds);
        }

        [AfterFeature]
        public static void TearDown()
        {
            TearDownChromeDriver();
        }
    }
}
