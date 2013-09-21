using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    public class ChromeDriverTest
    {
        protected static ChromeDriver _chromeDriver;

        private static ChromeDriver CreateChromeDriverInstance()
        {
            return new ChromeDriver("WebDriver") {Url = "localhost:27554"};
        }

        public static void SetUpSeleniumOnChrome()
        {
            _chromeDriver = CreateChromeDriverInstance();
            _chromeDriver.Navigate();
        }

        [BeforeScenario]
        public void NavigateToHomePage()
        {
            _chromeDriver.Url = "localhost:27554";
            _chromeDriver.Navigate();
        }

        public static void TearDownChromeDriver()
        {
            _chromeDriver.Close();
            _chromeDriver.Quit();
        }
    }
}