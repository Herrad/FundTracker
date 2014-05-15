using System;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;

namespace Test.FundTracker.Web.ViewModels.Builders
{
    [TestFixture]
    public class TestCalendarDayViewModel
    {
        [Test]
        public void Sets_DaysInPreviousMonth_to_last_5_days_of_previous_month()
        {
            var selectedDate = new DateTime(2014, 10, 14);
            const int daysInSeptember = 30;

            var calendarDayViewModel = new CalendarDayViewModelBuilder().Build(selectedDate);

            var daysInPreviousMonth = calendarDayViewModel.DaysInPreviousMonth;

            Assert.That(daysInPreviousMonth.Count, Is.EqualTo(5));
            Assert.That(daysInPreviousMonth[0], Is.EqualTo(daysInSeptember - 4));
            Assert.That(daysInPreviousMonth[1], Is.EqualTo(daysInSeptember - 3));
            Assert.That(daysInPreviousMonth[2], Is.EqualTo(daysInSeptember - 2));
            Assert.That(daysInPreviousMonth[3], Is.EqualTo(daysInSeptember - 1));
            Assert.That(daysInPreviousMonth[4], Is.EqualTo(daysInSeptember));
        }

        [TestCase(9, 5)]
        [TestCase(10, 4)]
        [TestCase(2, 7)]
        public void Sets_DaysInNextMonth_to_first_few_days_depending_on_days_in_current_month(int currentMonth, int expectedNumberOfDaysInNextMonth)
        {
            var calendarDayViewModel = new CalendarDayViewModelBuilder().Build(new DateTime(2014, currentMonth, 14));

            Assert.That(calendarDayViewModel.DaysInNextMonth, Is.Not.Null);
            Assert.That(calendarDayViewModel.DaysInNextMonth.Count, Is.EqualTo(expectedNumberOfDaysInNextMonth));
        }
    }
}