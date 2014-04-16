using Coypu;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Pages;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class AdministeringAWalletSteps : WebDriverTests 
    {
        [Given(@"my available funds are (.*)")]
        public void GivenMyAvailableFundsAre(decimal expectedAvailableFunds)
        {
            var availableFundsOnPage = AdministerWalletPage.GetAvailableFunds();

            if (expectedAvailableFunds > availableFundsOnPage)
            {
                var fundDifference = expectedAvailableFunds - availableFundsOnPage;
                AdministerWalletPage.AddFundsToWallet(fundDifference);
            }
            else if (expectedAvailableFunds < availableFundsOnPage)
            {
                AdministerWalletPage.RemoveFundsFromWallet(availableFundsOnPage - expectedAvailableFunds);
            }

            Assert.That(AdministerWalletPage.GetAvailableFunds(), Is.EqualTo(expectedAvailableFunds), "funds were not set as expected (is adding/removing funds working?)");
        }

        [Given(@"I have created a wallet with a unique name starting with ""(.*)""")]
        public void GivenIHaveCreatedAWalletWithAUniqueNameStartingWith (string walletName)
        {
            IndexPage.CreateWalletWithAUniqueNameStartingWith(walletName);
        }

        [Given(@"A wallet already exists called ""(.*)""")]
        public void GivenAWalletAlreadyExistsCalled(string walletName)
        {

        }

        [When(@"I load the wallet with name ""(.*)""")]
        public void WhenILoadTheWalletWithName(string walletName)
        {
            IndexPage.FindWalletCalled(walletName);
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
            AdministerWalletPage.AddFundsToWallet(fundAmount);
        }

        [When(@"I remove (.*) in funds from my wallet")]
        public void WhenIRemoveInFundsFromMyWallet(decimal fundsToRemove)
        {
            AdministerWalletPage.RemoveFundsFromWallet(fundsToRemove);
        }

        [When(@"I add a recurring withdrawal of (.*)")]
        public void WhenIAddARecurringWithdrawalOf(decimal fundsToWithdraw)
        {
            AdministerWalletPage.AddWithdrawal(fundsToWithdraw);
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
    }
}
