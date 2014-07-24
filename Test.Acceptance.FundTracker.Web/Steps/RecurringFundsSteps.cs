using System;
using System.Globalization;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Data;
using Test.Acceptance.FundTracker.Web.Pages;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class RecurringFundsSteps : WebDriverTests
    {

        [Given(@"I removed (.*) in funds (.*) days ago for ""(.*)""")]
        public void GivenIRemovedInFundsDaysAgoFor(int amountRemoved, int daysSinceRemovalTookPlace, string nameOfRemoval)
        {
            var firstApplicationDate = DateTime.Today.AddDays(0 - daysSinceRemovalTookPlace).ToString("yyyy-MM-dd");
            var walletName = ScenarioContext.Current["wallet name"].ToString();

            TestDbAdapter.CreateRecurringChange(walletName, nameOfRemoval, 0 - amountRemoved, firstApplicationDate, "Just today");
        }

        [Given(@"I have a deposit of (.*) due in (.*) days for ""(.*)""")]
        public void GivenIHaveADepositOfDueInDaysFor(int depositAmount, int daysUntilItsDue, string depositName)
        {
            var depositDueDate = DateTime.Today.AddDays(daysUntilItsDue).ToString("yyyy-MM-dd");
            var walletName = ScenarioContext.Current["wallet name"].ToString();

            TestDbAdapter.CreateRecurringChange(walletName, depositName, depositAmount, depositDueDate, "Just today");
        }

        [Given(@"the following recurring deposit exists")]
        public void GivenTheFollowingRecurringDepositExists(Table table)
        {
            var tableRow = table.Rows[0];
            var name = tableRow["Name"];
            var amount = int.Parse(tableRow["Amount"]);
            var startDate = tableRow["Start Date"];
            var repetitionRule = tableRow["Repetition Rule"];

            var walletName = ScenarioContext.Current["wallet name"].ToString();
            TestDbAdapter.CreateRecurringChange(walletName, name, amount, startDate, repetitionRule);
        }


        [When(@"I view my withdrawals for (.*) days ago")]
        public void WhenIViewMyWithdrawalsForDaysAgo(int daysFromToday)
        {
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);

            administerWalletPage = administerWalletPage.ViewFor(DateTime.Today.AddDays(0 - daysFromToday));
            ScenarioContext.Current["page being viewed"] = administerWalletPage.ViewWithdrawals();
        }

        [When(@"I view my deposits for (.*) days ahead")]
        public void WhenIViewMyDepositsForDaysAhead(int daysAheadOfToday)
        {
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);

            administerWalletPage = administerWalletPage.ViewFor(DateTime.Today.AddDays(daysAheadOfToday));
            ScenarioContext.Current["page being viewed"] = administerWalletPage.ViewWithdrawals();
        }

        [When(@"I add the following recurring deposit")]
        public void WhenIAddTheFollowingRecurringDeposit(Table recurringDepositParameters)
        {
            var recurringDepositToEnter = recurringDepositParameters.Rows[0];

            var depositAmount = recurringDepositToEnter["Amount"];
            var depositName = recurringDepositToEnter["Name"];

            var recurringDeposit = WebDriver.FindCss(".recurring.deposit");
            recurringDeposit.Click();


            WebDriver.FindCss(".amount").SendKeys(depositAmount.ToString(CultureInfo.InvariantCulture));
            WebDriver.FindCss(".name").SendKeys(depositName.ToString(CultureInfo.InvariantCulture));

            WebDriver.FindCss(".submit").Click();
        }

        [When(@"I add the following recurring withdrawal")]
        public void WhenIAddTheFollowingRecurringWithdrawal(Table recurringWithdrawalParameters)
        {
            var recurringDepositToEnter = recurringWithdrawalParameters.Rows[0];

            var depositAmount = recurringDepositToEnter["Amount"];
            var depositName = recurringDepositToEnter["Name"];

            var change = WebDriver.FindCss(".recurring.withdrawal");
            change.Click();

            WebDriver.FindCss(".amount").SendKeys(depositAmount.ToString(CultureInfo.InvariantCulture));
            WebDriver.FindCss(".name").SendKeys(depositName.ToString(CultureInfo.InvariantCulture));

            WebDriver.FindCss(".submit").Click();
        }

        [When(@"I view my deposits for ""(.*)""")]
        public void WhenIViewMyDepositsFor(string targetDate)
        {
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);

            administerWalletPage = administerWalletPage.ViewFor(DateTime.Parse(targetDate));
            ScenarioContext.Current["page being viewed"] = administerWalletPage.ViewDeposits();
        }
        
        [When(@"I stop the deposit called ""(.*)"" on ""(.*)""")]
        public void WhenIStopTheDepositCalledOn(string depositName, string rawDate)
        {
            var date = DateTime.Parse(rawDate);
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);
            administerWalletPage = administerWalletPage.ViewFor(date);
            var recurringChangeListPage = administerWalletPage.ViewDeposits();
            recurringChangeListPage.StopChangeCalled(depositName);
            ScenarioContext.Current["page being viewed"] = recurringChangeListPage;
        }
        [When(@"I remove the deposit called ""(.*)"" on ""(.*)""")]
        public void WhenIRemoveTheDepositCalledOn(string depositName, string rawDate)
        {
            var date = DateTime.Parse(rawDate);
            var walletName = ScenarioContext.Current["wallet name"].ToString();
            var administerWalletPage = IndexPage.SubmitSearchForWalletCalled(walletName);
            administerWalletPage = administerWalletPage.ViewFor(date);
            var recurringChangeListPage = administerWalletPage.ViewDeposits();
            recurringChangeListPage.RemoveChangeCalled(depositName);
            ScenarioContext.Current["page being viewed"] = recurringChangeListPage;
        }


        [Then(@"I can see an entry for ""(.*)""")]
        public void ThenICanSeeAnEntryFor(string expectedEntry)
        {
            var pageBeingViewed = ScenarioContext.Current["page being viewed"];
            Assert.That(pageBeingViewed, Is.TypeOf<RecurringChangeListPage>());

            var recurringChangeListPage = (RecurringChangeListPage)pageBeingViewed;
            var entryExists = recurringChangeListPage.HasEntryFor(expectedEntry);

            Assert.That(entryExists, "Expected a recurring change called " + expectedEntry);
        }

        [Then(@"no entry for ""(.*)"" is present")]
        public void ThenNoEntryForIsPresent(string expectedEntry)
        {
            var pageBeingViewed = ScenarioContext.Current["page being viewed"];
            Assert.That(pageBeingViewed, Is.TypeOf<RecurringChangeListPage>());

            var recurringChangeListPage = (RecurringChangeListPage)pageBeingViewed;
            var entryExists = recurringChangeListPage.HasEntryFor(expectedEntry);


            Assert.That(entryExists, Is.False, "Expected not to find a recurring change called " + expectedEntry);
        }

        [Then(@"no entry for ""(.*)"" is present on ""(.*)""")]
        public void ThenNoEntryForIsPresentOn(string changeName, string targetDate)
        {
            WebDriver.Visit("/");
            WhenIViewMyDepositsFor(targetDate);
            ThenNoEntryForIsPresent(changeName);
        }



        [Then(@"the outgoing total value is (.*)")]
        public void ThenTheOutgoingTotalValueIs(decimal expectedWithdrawalAmount)
        {
            var withdrawalAmount = WebDriver.FindCss(".recurring-amount.withdrawal");

            var amount = decimal.Parse(withdrawalAmount.FindCss(".amount").Text);

            Assert.That(amount, Is.EqualTo(expectedWithdrawalAmount));
        }

        [Then(@"the incoming total value is (.*)")]
        public void ThenTheIncomingTotalValueIs(decimal expectedDepositAmount)
        {
            var withdrawalAmount = WebDriver.FindCss(".recurring-amount.deposit");

            var amount = decimal.Parse(withdrawalAmount.FindCss(".amount").Text);

            Assert.That(amount, Is.EqualTo(expectedDepositAmount));
        }

    }
}
