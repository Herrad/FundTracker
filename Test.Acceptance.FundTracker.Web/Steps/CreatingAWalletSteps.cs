﻿using System;
using System.Globalization;
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

        [Given(@"my available funds are (.*)")]
        public void GivenMyAvailableFundsAre(decimal expectedAvailableFunds)
        {
            
            var availableFundsOnPage = ParseAvailableFundsFromPage();

            if (expectedAvailableFunds > availableFundsOnPage)
            {
                var fundDifference = expectedAvailableFunds - availableFundsOnPage;
                AddFundsToWallet(fundDifference);
            }
            else if (expectedAvailableFunds < availableFundsOnPage)
            {
                RemoveFundsFromWallet(availableFundsOnPage - expectedAvailableFunds);
            }

            Assert.That(ParseAvailableFundsFromPage(), Is.EqualTo(expectedAvailableFunds), "funds were not set as expected (is adding/removing funds working?)");
        }

        [Given(@"I have created a wallet with a unique name starting with ""(.*)""")]
        public void GivenIHaveCreatedAWalletWithAUniqueNameStartingWith (string walletName)
        {
            CreateAWalletWithAUniqueNameStartingWith(walletName);
        }

        private static decimal ParseAvailableFundsFromPage()
        {
            var availableFunds = _chromeDriver.FindElementById("available-funds").Text;
            decimal parsedAvailableFunds;
            var successfullyParsed = decimal.TryParse(availableFunds, out parsedAvailableFunds);
            if (!successfullyParsed)
                Assert.Fail("failed to parse " + availableFunds);
            return parsedAvailableFunds;
        }


        [When(@"I create a wallet with the unique name starting with ""(.*)""")]
        public void WhenICreateAWalletWithTheUniqueNameStartingWith(string walletName)
        {
            CreateAWalletWithAUniqueNameStartingWith(walletName);
        }

        private static void CreateAWalletWithAUniqueNameStartingWith(string walletName)
        {
            walletName += Guid.NewGuid();
            var nameBox = _chromeDriver.FindElementByName("name");
            nameBox.SendKeys(walletName);

            var createButton = _chromeDriver.FindElementByClassName("button");
            createButton.Click();

            var redirectLink = _chromeDriver.FindElementByPartialLinkText("Click here");
            redirectLink.Click();
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
            AddFundsToWallet(fundAmount);
        }

        [When(@"I remove (.*) in funds from my wallet")]
        public void WhenIRemoveInFundsFromMyWallet(decimal fundsToRemove)
        {
            RemoveFundsFromWallet(fundsToRemove);
        }


        private static void AddFundsToWallet(decimal fundAmount)
        {
            var addFundsAmount = _chromeDriver.FindElementByName("fundsToAdd");
            addFundsAmount.Clear();
            addFundsAmount.SendKeys(fundAmount.ToString(CultureInfo.InvariantCulture));

            var addFundsButton = _chromeDriver.FindElementByName("submit-add");
            addFundsButton.Click();
        }

        private static void RemoveFundsFromWallet(decimal fundsToRemove)
        {
            var removeFundsAmount = _chromeDriver.FindElementByName("fundsToRemove");
            removeFundsAmount.Clear();
            removeFundsAmount.SendKeys(fundsToRemove.ToString(CultureInfo.InvariantCulture));

            var submitButton = _chromeDriver.FindElementByName("submit-remove");
            submitButton.Click();
        }

        [Then(@"I am taken to the display wallet page")]
        public void ThenIAmTakenToTheDisplayWalletPage()
        {
            var pageTitle = _chromeDriver.FindElementByTagName("h2").Text;
            Assert.That(pageTitle, Is.EqualTo("Wallet"));
        }

        [Then(@"the name starts with ""(.*)""")]
        public void ThenTheNameStartsWith(string expectedWalletName)
        {
            var walletNameDisplayed = _chromeDriver.FindElementByClassName("wallet-name").Text;
            Assert.That(walletNameDisplayed.StartsWith(expectedWalletName));
        }

        [Then(@"the amount in the wallet is (.*)")]
        public void ThenTheAmountInTheWalletIs(decimal expectedFunds)
        {
            var availableFundsFromPage = ParseAvailableFundsFromPage();

            Assert.That(availableFundsFromPage, Is.EqualTo(expectedFunds));
        }

        [AfterFeature]
        public static void TearDown()
        {
            TearDownChromeDriver();
        }
    }
}
