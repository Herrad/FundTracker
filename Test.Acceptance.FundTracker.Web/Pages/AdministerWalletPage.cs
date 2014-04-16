using System;
using System.Globalization;
using NUnit.Framework;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class AdministerWalletPage
    {
        public static void AddFundsToWallet(decimal fundAmount)
        {
            var addFundsAmount = WebDriverTests.Driver.FindCss(".funds-to-add");
            addFundsAmount.SendKeys(fundAmount.ToString(CultureInfo.InvariantCulture));

            var addFundsButton = WebDriverTests.Driver.FindCss(".submit-add");
            addFundsButton.Click();
        }

        public static void RemoveFundsFromWallet(decimal fundsToRemove)
        {
            var removeFundsAmount = WebDriverTests.Driver.FindCss(".funds-to-remove");
            removeFundsAmount.SendKeys(fundsToRemove.ToString(CultureInfo.InvariantCulture));

            var submitButton = WebDriverTests.Driver.FindCss(".submit-remove");
            submitButton.Click();
        }

        public static void AddWithdrawal(decimal fundsToWithdraw)
        {
            var addWithdrawalElement = WebDriverTests.Driver.FindCss(".withdrawal");
            addWithdrawalElement.Click();

            var withdrawalAmount = WebDriverTests.Driver.FindCss("." + "withdrawal-amount");
            withdrawalAmount.SendKeys(fundsToWithdraw.ToString(CultureInfo.InvariantCulture));

            var withdrawalSubmit = WebDriverTests.Driver.FindCss("." + "withdrawal-submit");
            withdrawalSubmit.Click();
        }

        public static decimal GetAvailableFunds()
        {
            var availableFunds = WebDriverTests.Driver.FindCss("#available-funds-value").Text;
            decimal parsedAvailableFunds;
            var successfullyParsed = Decimal.TryParse(availableFunds, out parsedAvailableFunds);
            if (!successfullyParsed)
                Assert.Fail("failed to parse " + availableFunds);
            return parsedAvailableFunds;
        }
    }
}