using System.Globalization;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    public class AddDepositPage
    {
        public AddDepositPage CreateNewDeposit(string depositAmount, string depositName, string recurranceRule)
        {
            WebDriverTests.WebDriver.FindCss(".amount").SendKeys(depositAmount.ToString(CultureInfo.InvariantCulture));
            WebDriverTests.WebDriver.FindCss(".name").SendKeys(depositName.ToString(CultureInfo.InvariantCulture));
            if (recurranceRule != null)
            {
                WebDriverTests.WebDriver.FindCss(".rule").Select(recurranceRule);
            }

            WebDriverTests.WebDriver.FindCss(".submit").Click();

            return this;
        }
    }
}