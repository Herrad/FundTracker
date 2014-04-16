using System.Configuration;
using Coypu;
using Coypu.Drivers.Selenium;
using TechTalk.SpecFlow;

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
        public static void TearDownWebDriver()
        {
            Driver.Dispose();
        }

        public static BrowserSession Driver { get; private set; }
    }
}