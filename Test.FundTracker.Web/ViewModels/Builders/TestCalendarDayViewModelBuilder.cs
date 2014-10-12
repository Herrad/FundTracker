using System;
using FundTracker.Domain;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.ViewModels.Builders
{
    [TestFixture]
    public class TestCalendarDayViewModelBuilder
    {

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