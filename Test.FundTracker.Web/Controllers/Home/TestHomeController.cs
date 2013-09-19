using FundTracker.Web.Controllers;
using FundTracker.Web.Controllers.ValidationFailure;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.Controllers.Home
{
    [TestFixture]
    public class TestHomeController
    {
        [Test]
        public void Index_returns_ViewResult_with_empty_ViewName_when_no_failures_are_passed()
        {
            var homeController = new HomeController();
            var viewResult = homeController.Index(null);

            Assert.That(viewResult.ViewName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Index_action_calls_GetFailureMessage_on_validation_failure()
        {
            var validationFailure = MockRepository.GenerateMock<IValidationFailure>();

            var homeController = new HomeController();
            homeController.Index(validationFailure);

            validationFailure.AssertWasCalled(
                failure => failure.GetFailureMessage(),
                c => c.Repeat.Once());
        }
    }
}
