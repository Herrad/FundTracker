using System;
using System.Globalization;
using Coypu;
using NUnit.Framework;
using OpenQA.Selenium;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class AdministerWalletPage
    {
        public void AddFundsToWallet(decimal fundAmount)
        {
            var addFundsAmount = WebDriverTests.Driver.FindCss(".funds-to-add");
            addFundsAmount.SendKeys(fundAmount.ToString(CultureInfo.InvariantCulture));

            var addFundsButton = WebDriverTests.Driver.FindCss(".submit-add");
            addFundsButton.Click();
        }

        public void RemoveFundsFromWallet(decimal fundsToRemove)
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

        public AdministerWalletPage ViewFor(DateTime targetDate)
        {
            var calander = WebDriverTests.Driver.FindCss("#calendar");
            var selectedDate = calander.FindCss(".current-month.selected");
            var targetDayOfMonth = targetDate.Day.ToString(CultureInfo.InvariantCulture);
            if (int.Parse(targetDayOfMonth) < 10)
            {
                targetDayOfMonth = "0" + targetDayOfMonth;
            }

            if (targetDayOfMonth == selectedDate.Text)
            {
                return this;
            }

            var daysInMonth = calander.FindAllCss(".current-month");
            foreach (var dayInMonth in daysInMonth)
            {
                if (targetDayOfMonth == dayInMonth.Text)
                {
                    dayInMonth.Click();
                    return this;
                }
            }
            throw new NoSuchElementException("Couldn't find: "+ targetDayOfMonth);
        }

        public RecurringChangeListPage ViewWithdrawals()
        {
            WebDriverTests.Driver.FindLink("Total Withdrawals").Click();
            return new RecurringChangeListPage();
        }
    }
}