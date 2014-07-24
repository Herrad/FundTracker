using System.Linq;
using Coypu;
using OpenQA.Selenium;
using Test.Acceptance.FundTracker.Web.Steps;

namespace Test.Acceptance.FundTracker.Web.Pages
{
    public class RecurringChangeListPage : WebDriverTests
    {
        public bool HasEntryFor(string entryToCheck)
        {
            return GetRowContaining(entryToCheck) != null;
        }

        private static SnapshotElementScope GetRowContaining(string entryToCheck)
        {
            entryToCheck = entryToCheck.ToLower();

            var tableRows = WebDriver.FindAllCss("tr").ToList();
            foreach (var tableRow in tableRows)
            {
                var rowCells = tableRow.FindAllCss("td").ToList();
                if (rowCells.Any(x => x.Text.ToLower() == entryToCheck))
                {
                    return tableRow;
                }
            }
            return null;
        }

        public void StopChangeCalled(string depositName)
        {
            var rowContainingChange = GetRowContaining(depositName);
            if (rowContainingChange == null)
            {
                throw new NoSuchElementException("Couldn't find a case insensitive table cell called: " + depositName);
            }
            rowContainingChange.FindLink("Stop from today").Click();
        }

        public void RemoveChangeCalled(string depositName)
        {
            var rowContainingChange = GetRowContaining(depositName);
            if (rowContainingChange == null)
            {
                throw new NoSuchElementException("Couldn't find a case insensitive table cell called: " + depositName);
            }
            rowContainingChange.FindLink("Remove from today").Click();
        }
    }
}