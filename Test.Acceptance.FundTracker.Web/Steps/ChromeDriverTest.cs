using System;
using System.Configuration;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    public class ChromeDriverTest
    {
        protected static ChromeDriver _chromeDriver;
        private static readonly string _baseUrl = ConfigurationManager.AppSettings["BaseSiteUrl"];

        private static ChromeDriver CreateChromeDriverInstance()
        {
            return new ChromeDriver("WebDriver") { Url = _baseUrl };
        }

        public static void SetUpSeleniumOnChrome()
        {
            _chromeDriver = CreateChromeDriverInstance();
            _chromeDriver.Navigate();
        }

        protected void NavigateToCurrentUrlWith(string queryString)
        {
            _chromeDriver.Url += queryString;
            _chromeDriver.Navigate();
        }

        [BeforeScenario]
        public void NavigateToHomePage()
        {
            _chromeDriver.Url = _baseUrl;
            _chromeDriver.Navigate();
        }

        public static void TearDownChromeDriver()
        {
            _chromeDriver.Close();
            _chromeDriver.Quit();
        }
    }
}