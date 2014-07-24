using Coypu;
using FundTracker.Data.Annotations;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Data;
using Test.Acceptance.FundTracker.Web.Pages;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding, UsedImplicitly]
    public class AdministeringAWalletSteps : WebDriverTests
    {
        [Given(@"this wallet exists")]
        public void GivenThisWalletExists(Table table)
        {
            var walletRow = table.Rows[0];
            var name = walletRow["Unique Name"];

            ScenarioContext.Current["wallet name"] = name;

            TestDbAdapter.CreateWalletCalled(name);
        }

        [Given(@"my wallet has no recurring changes")]
        public void GivenItHasNoRecurringChanges()
        {
            TestDbAdapter.RemoveAllRecurringChangesAssociatedWith(ScenarioContext.Current["wallet name"].ToString());
        }


        [Given(@"I am administering this wallet")]
        public void GivenIAmAdministeringThisWallet()
        {
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            walletName = walletName.Replace(" ", "%20");
            WebDriver.Visit("/Wallet/Display?walletName=" + walletName);
        }


        [Given(@"I have created a wallet with a unique name starting with ""(.*)""")]
        public void GivenIHaveCreatedAWalletWithAUniqueNameStartingWith (string walletName)
        {
            IndexPage.CreateWalletWithAUniqueNameStartingWith(walletName);
        }

        [Given(@"A wallet already exists called ""(.*)""")]
        public void GivenAWalletAlreadyExistsCalled(string walletName)
        {
            TestDbAdapter.CreateWalletCalled(walletName);
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
            var nameBox = WebDriver.FindCss(".name");
            nameBox.SendKeys(walletName);

            var createButton = WebDriver.FindCss(".button");
            createButton.Click();

            var redirectLink = WebDriver.FindCss("a");
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
            var pageTitle = WebDriver.FindCss("h2", new Options {Match = Match.First}).Text;
            Assert.That(pageTitle, Is.Not.Null);
        }

        [Then(@"the name starts with ""(.*)""")]
        public void ThenTheNameStartsWith(string expectedWalletName)
        {
            var walletNameDisplayed = WebDriver.FindCss("h2").Text;
            Assert.That(walletNameDisplayed.StartsWith(expectedWalletName));
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
            var walletFound = TestDbAdapter.FindWalletCalled(walletName);

            Assert.That(walletFound.Name, Is.EqualTo(walletName));
        }
    }
}
