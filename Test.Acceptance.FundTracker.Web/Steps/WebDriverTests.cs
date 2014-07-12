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
            var appHost = ConfigurationManager.AppSettings["host"];

            var portNumber = int.Parse(ConfigurationManager.AppSettings["port"]);

            var sessionConfiguration = new SessionConfiguration
            {
                AppHost = appHost,
                Port = portNumber,
                Driver = typeof(SeleniumWebDriver),
                Browser = Coypu.Drivers.Browser.Firefox
            };
            Driver = new BrowserSession(sessionConfiguration);
            Driver.Visit("/");
        }

        [AfterScenario]
        public static void TearDown()
        {
            if (ScenarioContext.Current.ContainsKey("wallet name"))
            {
                TestDbAdapter.RemoveWallet(ScenarioContext.Current["wallet name"].ToString());
            }

            Driver.Dispose();
        }

        public static BrowserSession Driver { get; private set; }
    }
}