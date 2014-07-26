using System;
using System.Globalization;
using Coypu;
using NUnit.Framework;
using OpenQA.Selenium;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class AdministerWalletPage : WebDriverTests
    {
        public void AddFundsToWallet(decimal fundAmount)
        {
            var addFundsAmount = WebDriver.FindCss(".funds-to-add");
            addFundsAmount.SendKeys(fundAmount.ToString(CultureInfo.InvariantCulture));

            var description = WebDriver.FindCss(".add").FindField("changeName");
            description.SendKeys("foo bar");

            var addFundsButton = WebDriver.FindCss(".submit-add");
            addFundsButton.Click();
        }

        public void RemoveFundsFromWallet(decimal fundsToRemove)
        {
            var removeFundsAmount = WebDriver.FindCss(".funds-to-remove");
            removeFundsAmount.SendKeys(fundsToRemove.ToString(CultureInfo.InvariantCulture));

            var description = WebDriver.FindCss(".remove").FindField("changeName");
            description.SendKeys("foo bar");

            var submitButton = WebDriver.FindCss(".submit-remove");
            submitButton.Click();
        }

        public static void AddWithdrawal(decimal fundsToWithdraw)
        {
            var addWithdrawalElement = WebDriver.FindCss(".withdrawal");
            addWithdrawalElement.Click();

            var withdrawalAmount = WebDriver.FindCss("." + "withdrawal-amount");
            withdrawalAmount.SendKeys(fundsToWithdraw.ToString(CultureInfo.InvariantCulture));

            var withdrawalSubmit = WebDriver.FindCss("." + "withdrawal-submit");
            withdrawalSubmit.Click();
        }

        public static decimal GetAvailableFunds()
        {
            var availableFunds = WebDriver.FindCss("#available-funds-value").Text;
            decimal parsedAvailableFunds;
            var successfullyParsed = Decimal.TryParse(availableFunds, out parsedAvailableFunds);
            if (!successfullyParsed)
                Assert.Fail("failed to parse " + availableFunds);
            return parsedAvailableFunds;
        }

        public AdministerWalletPage ViewFor(DateTime targetDate)
        {
            var calander = WebDriver.FindCss("#calendar");
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
            WebDriver.FindLink("Total Withdrawals").Click();
            return new RecurringChangeListPage();
        }

        public RecurringChangeListPage ViewDeposits()
        {
            WebDriver.FindLink("Total Deposits").Click();
            return new RecurringChangeListPage();
        }

        public AdministerWalletPage GoToNextMonth()
        {
            var calander = WebDriver.FindCss("#calendar");
            calander.FindCss(".other-month.next", Options.First).Click();
            return this;
        }

        public string GetCurrentMonth()
        {
            var calander = WebDriver.FindCss("#calendar");
            return calander.FindCss(".month").Text;
        }

        public AdministerWalletPage GoToLastMonth()
        {
            var calander = WebDriver.FindCss("#calendar");
            calander.FindCss(".other-month.previous", Options.First).Click();
            return this;
        }

        public string GetSelectedDay()
        {
            var calander = WebDriver.FindCss("#calendar");
            return calander.FindCss(".selected", Options.First).Text;
        }

        public AddDepositPage AddNewRecurringDeposit()
        {
            var change = WebDriver.FindCss(".recurring.deposit");
            change.Click();
            return new AddDepositPage();
        }
    }
}