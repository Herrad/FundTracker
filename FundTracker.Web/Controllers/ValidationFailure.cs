namespace FundTracker.Web.Controllers
{
    public class ValidationFailure : IValidationFailure
    {
        private readonly string _message;

        public ValidationFailure(string message)
        {
            _message = message;
        }

        public string GetFailureMessage()
        {
            return _message;
        }
    }
}