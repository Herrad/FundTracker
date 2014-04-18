using System.Globalization;
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
    }
}
