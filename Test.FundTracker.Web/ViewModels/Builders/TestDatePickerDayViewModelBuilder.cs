using System;
using System.Linq;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;

namespace Test.FundTracker.Web.ViewModels.Builders
{
    [TestFixture]
    public class TestDatePickerDayViewModelBuilder
    {
        [TestCase(9, 30)]
        [TestCase(10, 31)]
        [TestCase(2, 28)]
        public void Returns_entry_for_all_days_in_selected_month(int selectedMonth, int expectedNumberOfDays)
        {
            var selectedDate = new DateTime(2014, selectedMonth, 14);

            var datePickerViewModelBuilder = new DatePickerDayViewModelBuilder();
            var datePickerDays = datePickerViewModelBuilder.BuildDatePickerDayViewModels(selectedDate).ToList();

            Assert.That(datePickerDays, Is.Not.Null);
            Assert.That(datePickerDays.Count, Is.EqualTo(expectedNumberOfDays));
        }

        [Test]
        public void Sets_model_text_with_zeros_when_less_than_10()
        {
            var selectedDate = new DateTime(2014, 9, 14);

            var datePickerViewModelBuilder = new DatePickerDayViewModelBuilder();
            var datePickerDays = datePickerViewModelBuilder.BuildDatePickerDayViewModels(selectedDate).ToList();
            
            Assert.That(datePickerDays, Is.Not.Null);
            for (int i = 1; i < 10; i++)
            {
                var datePickerDay = datePickerDays[i - 1];
                Assert.That(datePickerDay.Text, Is.EqualTo("0" + i));
            }
        }

        [Test]
        public void Sets_model_text_without_zeros_when_10_or_more()
        {
            var selectedDate = new DateTime(2014, 9, 14);

            var datePickerViewModelBuilder = new DatePickerDayViewModelBuilder();
            var datePickerDays = datePickerViewModelBuilder.BuildDatePickerDayViewModels(selectedDate).ToList();
            
            Assert.That(datePickerDays, Is.Not.Null);
            for (int i = 10; i < datePickerDays.Count; i++)
            {
                var datePickerDay = datePickerDays[i - 1];
                Assert.That(datePickerDay.Text, Is.EqualTo("" + i));
            }
        }
    }
}