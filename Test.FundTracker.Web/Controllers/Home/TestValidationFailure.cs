using FundTracker.Web.Controllers;
using NUnit.Framework;

namespace Test.FundTracker.Web.Controllers.Home
{
    [TestFixture]
    public class TestValidationFailure
    {
        [Test]
        public void GetFailureMessage_returns_the_message_passed_in_at_construction()
        {
            const string expectedMessage = "foo a message";

            var validationFailure = new ValidationFailure(expectedMessage);

            Assert.That(validationFailure.GetFailureMessage(), Is.EqualTo(expectedMessage));
        } 
    }
}