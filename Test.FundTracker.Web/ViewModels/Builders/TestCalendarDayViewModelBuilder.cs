using System;
using FundTracker.Domain;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;

namespace Test.FundTracker.Web.ViewModels.Builders
{
    [TestFixture]
    public class TestCalendarDayViewModelBuilder
    {
        [TestCase(9, 30)]
        [TestCase(10, 31)]
        [TestCase(2, 28)]
        public void Fills_in_all_days_in_selected_month(int selectedMonth, int expectedNumberOfDays)
        {
            var selectedDate = new DateTime(2014, selectedMonth, 14);

            var walletDatePickerViewModelBuilder = new WalletDatePickerViewModelBuilder(new DatePickerDayViewModelBuilder());
            var calendarDayViewModel = walletDatePickerViewModelBuilder.Build(selectedDate, new WalletIdentification("fooName"));

            Assert.That(calendarDayViewModel.DaysInCurrentMonth, Is.Not.Null);
            Assert.That(calendarDayViewModel.DaysInCurrentMonth.Count, Is.EqualTo(expectedNumberOfDays));
        }

        [Test]
        public void Sets_SelectedDate()
        {
            var selectedDate = new DateTime(2014, 9, 14);
            var walletDatePickerViewModelBuilder = new WalletDatePickerViewModelBuilder(new DatePickerDayViewModelBuilder());
            var calendarDayViewModel = walletDatePickerViewModelBuilder.Build(selectedDate, new WalletIdentification("fooName"));

            Assert.That(calendarDayViewModel.SelectedDate, Is.EqualTo(selectedDate));
        }

        [Test]
        public void Sets_WalletName()
        {
            const string expectedName = "fooName";
            var selectedDate = new DateTime(2014, 9, 14);

            var walletDatePickerViewModelBuilder = new WalletDatePickerViewModelBuilder(new DatePickerDayViewModelBuilder());
            var calendarDayViewModel = walletDatePickerViewModelBuilder.Build(selectedDate, new WalletIdentification(expectedName));

            Assert.That(calendarDayViewModel.WalletName, Is.EqualTo(expectedName));
        }
    }
}