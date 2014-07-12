using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Data;
using Test.Acceptance.FundTracker.Web.Pages;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class AddingAndRemovingFundsSteps : WebDriverTests
    {
        [Given(@"I removed (.*) in funds (.*) days ago for ""(.*)""")]
        public void GivenIRemovedInFundsDaysAgoFor(int amountRemoved, int daysSinceRemovalTookPlace, string nameOfRemoval)
        {
            var changeTimeStamp = DateTime.Now.AddDays(0 - daysSinceRemovalTookPlace);
            var walletName = ScenarioContext.Current["wallet name"].ToString();

            TestDbAdapter.CreateRecurringChange(walletName, nameOfRemoval, 0-amountRemoved, changeTimeStamp);
        }

        [Given(@"I have a deposit of (.*) due in (.*) days for ""(.*)""")]
        public void GivenIHaveADepositOfDueInDaysFor(int depositAmount, int daysUntilItsDue, string depositName)
        {
            var depositDueDate = DateTime.Now.AddDays(daysUntilItsDue);
            var walletName = ScenarioContext.Current["wallet name"].ToString();

            TestDbAdapter.CreateRecurringChange(walletName, depositName, depositAmount, depositDueDate);
        }


        [When(@"I view my withdrawals for (.*) days ago")]
        public void WhenIViewMyWithdrawalsForDaysAgo(int daysFromToday)
        {
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);

            administerWalletPage = administerWalletPage.ViewFor(DateTime.Now.AddDays(0 - daysFromToday));
            ScenarioContext.Current["page being viewed"] =  administerWalletPage.ViewWithdrawals();
        }

        [When(@"I view my deposits for (.*) days ahead")]
        public void WhenIViewMyDepositsForDaysAhead(int daysAheadOfToday)
        {
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);

            administerWalletPage = administerWalletPage.ViewFor(DateTime.Now.AddDays(daysAheadOfToday));
            ScenarioContext.Current["page being viewed"] = administerWalletPage.ViewWithdrawals();
        }



        [Then(@"I can see an entry for ""(.*)""")]
        public void ThenICanSeeAnEntryFor(string expectedEntry)
        {
            var pageBeingViewed = ScenarioContext.Current["page being viewed"];
            Assert.That(pageBeingViewed, Is.TypeOf<RecurringChangeListPage>());

            var recurringChangeListPage = (RecurringChangeListPage) pageBeingViewed;
            var entryExists = recurringChangeListPage.HasEntryFor(expectedEntry);

            Assert.That(entryExists, "Expected a recurring change called " + expectedEntry);
        }
    }
}