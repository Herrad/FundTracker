using System;
using System.Globalization;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class CalendarSteps : WebDriverTests
    {
        [Then(@"the calander should have today's date selected")]
        public void ThenTheCalanderShouldHaveTodaySDateSelected()
        {
            var calendar = Driver.FindCss("#calendar");
            var selected = calendar.FindCss(".selected");

            Assert.That(selected.Text, Is.EqualTo(DateTime.Today.Day.ToString(CultureInfo.InvariantCulture)));
        }

    }
}