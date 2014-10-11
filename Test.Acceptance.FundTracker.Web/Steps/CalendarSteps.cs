using System;
using System.Globalization;
using Coypu.Matchers;
using FundTracker.Data.Annotations;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Pages;
using Shows = Coypu.NUnit.Matchers.Shows;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding, UsedImplicitly]
    public class CalendarSteps : WebDriverTests
    {
        [When(@"I view next month")]
        public void WhenIViewNextMonth()
        {
            var administerWalletPage = new AdministerWalletPage().GoToNextMonth();
            ScenarioContext.Current["current page"] = administerWalletPage;
        }

        [When(@"I view last month")]
        public void WhenIViewLastMonth()
        {
            var administerWalletPage = new AdministerWalletPage().GoToLastMonth();
            ScenarioContext.Current["current page"] = administerWalletPage;
        }

        [Then(@"the calander should have the first day of next month selected")]
        public void ThenTheCalanderShouldHaveTheFirstDayOfNextMonthSelected()
        {
            var page = (AdministerWalletPage) ScenarioContext.Current["current page"];
            var currentMonth = page.GetCurrentMonth();
            var selectedDay = page.GetSelectedDay();
            var nextMonth = DateTime.Today.AddMonths(1);
            Assert.That(currentMonth, Is.EqualTo(nextMonth.ToString("MMMMMMMMM yyyy")));
            Assert.That(selectedDay, Is.EqualTo("01"));
        }

        [Then(@"the calander should have the last day of last month selected")]
        public void ThenTheCalanderShouldHaveTheLastDayOfLastMonthSelected()
        {
            var page = (AdministerWalletPage)ScenarioContext.Current["current page"];
            var currentMonth = page.GetCurrentMonth();
            var selectedDay = page.GetSelectedDay();
            var lastMonth = DateTime.Today.AddMonths(-1);
            Assert.That(currentMonth, Is.EqualTo(lastMonth.ToString("MMMMMMMMM yyyy")));
            Assert.That(selectedDay, Is.EqualTo(DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month).ToString(CultureInfo.InvariantCulture)));
        }


        [Then(@"the calander should have today's date selected")]
        public void ThenTheCalanderShouldHaveTodaysDateSelected()
        {
            var calendar = WebDriver.FindCss("#calendar");
            var selected = calendar.FindCss(".selected");

            Assert.That(selected.Text, Is.EqualTo(DateTime.Today.ToString("dd")));
        }

        [Then(@"the first not selected day is green")]
        public void ThenTheFirstNotSelectedDayIsGreen()
        {
            var page = (AdministerWalletPage)ScenarioContext.Current["current page"];
            var firstNotSelectedDay = page.GetFirstNotSelectedDay();


            Assert.That(firstNotSelectedDay, Shows.Css("good"));
        }

    }
}