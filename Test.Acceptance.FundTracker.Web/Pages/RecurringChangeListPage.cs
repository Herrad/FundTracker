using System.Linq;
using Coypu;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class RecurringChangeListPage
    {
        public bool HasEntryFor(string entryToCheck)
        {
            return GetRowContaining(entryToCheck) != null;
        }
        public SnapshotElementScope GetRowContaining(string entryToCheck)
        {
            entryToCheck = entryToCheck.ToLower();

            var tableRows = WebDriverTests.WebDriver.FindAllCss("tr").ToList();
            foreach (var tableRow in tableRows)
            {
                var rowCells = tableRow.FindAllCss("td");
                if (rowCells.Any(x => x.Text.ToLower() == entryToCheck))
                {
                    return tableRow;
                }
            }

            return null;
        }

        public void StopChangeCalled(string depositName)
        {
            GetRowContaining(depositName).FindLink("Stop from today").Click();
        }
    }
}