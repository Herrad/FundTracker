using System.Linq;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class RecurringChangeListPage
    {
        public bool HasEntryFor(string entryToCheck)
        {
            var listElements = WebDriverTests.Driver.FindAllCss("li");

            return listElements.Any(listElement => listElement.Text.ToLower() == entryToCheck);

        }

    }
}