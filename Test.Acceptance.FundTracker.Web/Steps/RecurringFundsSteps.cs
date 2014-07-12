using System;
using System.Globalization;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class RecurringFundsSteps : WebDriverTests
    {
        [When(@"I add a recurring withdrawal of (.*)")]
        public void WhenIAddARecurringWithdrawalOf(decimal withdrawalAmount)
        {
            var recurringWithdrawal = Driver.FindCss(".recurring.withdrawal");
            recurringWithdrawal.Click();

            Driver.FindCss(".withdrawal-amount").SendKeys(withdrawalAmount.ToString(CultureInfo.InvariantCulture));

            Driver.FindCss(".withdrawal-submit").Click();
        }

        [When(@"I add a recurring deposit of (.*)")]
        public void WhenIAddARecurringDepositOf(decimal depositAmount)
        {
            var recurringWithdrawal = Driver.FindCss(".recurring.deposit");
            recurringWithdrawal.Click();

            Driver.FindCss(".deposit-amount").SendKeys(depositAmount.ToString(CultureInfo.InvariantCulture));

            Driver.FindCss(".deposit-submit").Click();
        }


        [Then(@"the outgoing total value is (.*)")]
        public void ThenTheOutgoingTotalValueIs(decimal expectedWithdrawalAmount)
        {
            var withdrawalAmount = Driver.FindCss(".recurring-amount.withdrawal");

            var amount = decimal.Parse(withdrawalAmount.FindCss(".amount").Text);

            Assert.That(amount, Is.EqualTo(expectedWithdrawalAmount));
        }

        [Then(@"the incoming total value is (.*)")]
        public void ThenTheIncomingTotalValueIs(decimal expectedDepositAmount)
        {
            var withdrawalAmount = Driver.FindCss(".recurring-amount.deposit");

            var amount = decimal.Parse(withdrawalAmount.FindCss(".amount").Text);

            Assert.That(amount, Is.EqualTo(expectedDepositAmount));
        }

    }
}
