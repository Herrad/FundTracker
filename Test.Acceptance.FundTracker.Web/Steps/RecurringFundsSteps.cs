using System.Globalization;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class RecurringFundsSteps : WebDriverTests
    {

        [When(@"I add the following recurring deposit")]
        public void WhenIAddTheFollowingRecurringDeposit(Table recurringDepositParameters)
        {
            var recurringDepositToEnter = recurringDepositParameters.Rows[0];

            var recurringDeposit = Driver.FindCss(".recurring.deposit");
            recurringDeposit.Click();

            var depositAmount = recurringDepositToEnter["Amount"];
            var depositName = recurringDepositToEnter["Name"];

            Driver.FindCss(".deposit-amount").SendKeys(depositAmount.ToString(CultureInfo.InvariantCulture));
            Driver.FindCss(".deposit-name").SendKeys(depositName.ToString(CultureInfo.InvariantCulture));

            Driver.FindCss(".deposit-submit").Click();
        }

        [When(@"I add the following recurring withdrawal")]
        public void WhenIAddTheFollowingRecurringWithdrawal(Table recurringWithdrawalParameters)
        {
            var recurringDepositToEnter = recurringWithdrawalParameters.Rows[0];

            var recurringDeposit = Driver.FindCss(".recurring.withdrawal");
            recurringDeposit.Click();

            var depositAmount = recurringDepositToEnter["Amount"];
            var depositName = recurringDepositToEnter["Name"];

            Driver.FindCss(".withdrawal-amount").SendKeys(depositAmount.ToString(CultureInfo.InvariantCulture));
            Driver.FindCss(".withdrawal-name").SendKeys(depositName.ToString(CultureInfo.InvariantCulture));

            Driver.FindCss(".withdrawal-submit").Click();
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
