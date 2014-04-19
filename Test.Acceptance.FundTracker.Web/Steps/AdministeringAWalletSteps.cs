using System;
using Coypu;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Data;
using Test.Acceptance.FundTracker.Web.Pages;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class AdministeringAWalletSteps : WebDriverTests
    {
        [Given(@"this wallet exists")]
        public void GivenThisWalletExists(Table table)
        {
            var walletRow = table.Rows[0];
            var name = walletRow["Unique Name"];
            var availableFunds = decimal.Parse(walletRow["Starting Funds"]);

            ScenarioContext.Current["wallet name"] = name;
            ScenarioContext.Current["available funds"] = availableFunds;

            MongoDbAdapter.CreateWalletCalled(name, availableFunds);
        }

        [Given(@"my wallet has no recurring changes")]
        public void GivenItHasNoRecurringChanges()
        {
            MongoDbAdapter.RemoveRecurringChangesAssociatedWith(ScenarioContext.Current["wallet name"].ToString());
        }


        [Given(@"I am administering this wallet")]
        public void GivenIAmAdministeringThisWallet()
        {
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            walletName = walletName.Replace(" ", "%20");
            Driver.Visit("/Wallet/Display?walletName=" + walletName);
        }


        [Given(@"I have created a wallet with a unique name starting with ""(.*)""")]
        public void GivenIHaveCreatedAWalletWithAUniqueNameStartingWith (string walletName)
        {
            IndexPage.CreateWalletWithAUniqueNameStartingWith(walletName);
        }

        [Given(@"A wallet already exists called ""(.*)""")]
        public void GivenAWalletAlreadyExistsCalled(string walletName)
        {
            MongoDbAdapter.CreateWalletCalled(walletName, 0);
        }

        [When(@"I load the wallet with name ""(.*)""")]
        public void WhenILoadTheWalletWithName(string walletName)
        {
            IndexPage.SubmitSearchForWalletCalled(walletName);
        }


        [When(@"I create a wallet with the unique name starting with ""(.*)""")]
        public void WhenICreateAWalletWithTheUniqueNameStartingWith(string walletName)
        {
            IndexPage.CreateWalletWithAUniqueNameStartingWith(walletName);
        }

        [When(@"I create a wallet with the name ""(.*)""")]
        public void CreateAWalletWithTheName(string walletName)
        {
            var nameBox = Driver.FindCss(".name");
            nameBox.SendKeys(walletName);

            var createButton = Driver.FindCss(".button");
            createButton.Click();

            var redirectLink = Driver.FindCss("a");
            redirectLink.Click();
        }

        [When(@"I add (.*) in funds to my wallet")]
        public void WhenIAddInFundsToMyWallet(decimal fundAmount)
        {
            var walletName = (string) ScenarioContext.Current["wallet name"];
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);
            
            administerWalletPage.AddFundsToWallet(fundAmount);
        }

        [When(@"I remove (.*) in funds from my wallet")]
        public void WhenIRemoveInFundsFromMyWallet(decimal fundsToRemove)
        {
            var walletName = (string)ScenarioContext.Current["wallet name"];
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);
            
            administerWalletPage.RemoveFundsFromWallet(fundsToRemove);
        }

        [Then(@"I am taken to the display wallet page")]
        public void ThenIAmTakenToTheDisplayWalletPage()
        {
            var pageTitle = Driver.FindCss("h2", new Options {Match = Match.First}).Text;
            Assert.That(pageTitle, Is.EqualTo("Your Wallet"));
        }

        [Then(@"the name starts with ""(.*)""")]
        public void ThenTheNameStartsWith(string expectedWalletName)
        {
            var walletNameDisplayed = Driver.FindCss(".wallet-name").Text;
            Assert.That(walletNameDisplayed.StartsWith(expectedWalletName));
        }
        
        [Then(@"a withdrawal tile is shown with the outgoing amount set to (.*)")]
        public void ThenAWithdrawalTileIsShownWithTheOutgoingAmountSetTo(decimal expectedWithdrawalAmount)
        {
            var withdrawalTile =  Driver.FindCss(".withdrawal-tile");

            var amount = decimal.Parse(withdrawalTile.FindCss(".amount").Text);

            Assert.That(amount, Is.EqualTo(expectedWithdrawalAmount));
        }


        [Then(@"the amount in the wallet is (.*)")]
        public void ThenTheAmountInTheWalletIs(decimal expectedFunds)
        {
            var availableFundsFromPage = AdministerWalletPage.GetAvailableFunds();

            Assert.That(availableFundsFromPage, Is.EqualTo(expectedFunds));
        }

        [Then(@"the database contains a wallet with my name")]
        public void ThenTheDatabaseContainsAWalletWithMyName()
        {
            var walletName = (string) ScenarioContext.Current["wallet name"];
            var walletFound = MongoDbAdapter.FindWalletCalled(walletName);

            Assert.That(walletFound.Name, Is.EqualTo(walletName));
        }
    }
}
