using System.Configuration;
using Coypu;
using Coypu.Drivers.Selenium;
using TechTalk.SpecFlow;
using Test.Acceptance.FundTracker.Web.Data;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class WebDriverTests
    {
        [BeforeScenario]
        public static void ConfigureWebDriver()
        {
            var appHost = ConfigurationManager.AppSettings["host"] ?? "localhost";

            var portNumber = int.Parse(ConfigurationManager.AppSettings["port"] ?? "53463");

            var sessionConfiguration = new SessionConfiguration
            {
                AppHost = appHost,
                Port = portNumber,
                Driver = typeof(SeleniumWebDriver),
                Browser = Coypu.Drivers.Browser.Firefox
            };
            WebDriver = new BrowserSession(sessionConfiguration);
            WebDriver.Visit("/");
        }

        [AfterScenario]
        public static void TearDown()
        {
            if (ScenarioContext.Current.ContainsKey("wallet name"))
            {
                TestDbAdapter.RemoveWallet(ScenarioContext.Current["wallet name"].ToString());
            }

            WebDriver.Dispose();
        }

        public static BrowserSession WebDriver { get; private set; }
    }
}