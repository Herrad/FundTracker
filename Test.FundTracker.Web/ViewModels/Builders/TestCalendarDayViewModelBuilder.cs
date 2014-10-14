using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain;
using FundTracker.Web.ViewModels;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.ViewModels.Builders
{
    [TestFixture]
    public class TestWalletDatePickerViewModelBuilder
    {

        [Test]
        public void Sets_DaysInCurrentMonth_to_result_of_DatePickerDayBuilder()
        {
            var selectedDate = new DateTime(2014, 9, 1);

            var resultFromDatePickerDayBuilder = new List<DatePickerDayViewModel>
            {
                new DatePickerDayViewModel("foo"), 
                new DatePickerDayViewModel("bar"), 
                new DatePickerDayViewModel("foobar")
            };
            var datePickerDayViewModelBuilder = MockRepository.GenerateStub<IBuildDatePickerDayViewModels>();
            datePickerDayViewModelBuilder
                .Stub(x => x.BuildDatePickerDayViewModels(selectedDate))
                .Return(resultFromDatePickerDayBuilder);

            var walletDatePickerViewModelBuilder = new WalletDatePickerViewModelBuilder(datePickerDayViewModelBuilder);

            var wallet = MockRepository.GenerateStub<IKnowAboutAvailableFunds>();
            wallet
                .Stub(x => x.Identification)
                .Return(new WalletIdentification("foo"));

            var walletDatePickerViewModel = walletDatePickerViewModelBuilder.Build(selectedDate, wallet);

            var datePickerDayViewModels = walletDatePickerViewModel.DaysInCurrentMonth.ToList();
            Assert.That(datePickerDayViewModels[0], Is.EqualTo(resultFromDatePickerDayBuilder[0]));
            Assert.That(datePickerDayViewModels[1], Is.EqualTo(resultFromDatePickerDayBuilder[1]));
            Assert.That(datePickerDayViewModels[2], Is.EqualTo(resultFromDatePickerDayBuilder[2]));
        }

        [Test]
        public void Sets_SelectedDate()
        {
            var selectedDate = new DateTime(2014, 9, 14);
            var walletDatePickerViewModelBuilder = new WalletDatePickerViewModelBuilder(new DatePickerDayViewModelBuilder());
            var wallet = MockRepository.GenerateStub<IKnowAboutAvailableFunds>();
            wallet
                .Stub(x => x.Identification)
                .Return(new WalletIdentification("foo"));
            var calendarDayViewModel = walletDatePickerViewModelBuilder.Build(selectedDate, wallet);

            Assert.That(calendarDayViewModel.SelectedDate, Is.EqualTo(selectedDate));
        }

        [Test]
        public void Sets_WalletName()
        {
            const string expectedName = "fooName";
            var selectedDate = new DateTime(2014, 9, 14);

            var walletDatePickerViewModelBuilder = new WalletDatePickerViewModelBuilder(new DatePickerDayViewModelBuilder());
            var walletIdentification = new WalletIdentification(expectedName);
            var wallet = MockRepository.GenerateStub<IKnowAboutAvailableFunds>();
            wallet
                .Stub(x => x.Identification)
                .Return(walletIdentification);
            
            var calendarDayViewModel = walletDatePickerViewModelBuilder.Build(selectedDate, wallet);

            Assert.That(calendarDayViewModel.WalletName, Is.EqualTo(expectedName));
        }
    }
}