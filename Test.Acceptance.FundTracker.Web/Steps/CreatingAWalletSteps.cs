using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class CreatingAWalletSteps : ChromeDriverTest
    {
        [When(@"I create a wallet with the name ""(.*)""")]
        public void WhenICreateAWalletWithTheName(string walletName)
        {
            var nameBox = _chromeDriver.FindElementByName("name");
            nameBox.SendKeys(walletName);

            var createButton = _chromeDriver.FindElementByClassName("button");
            createButton.Click();

            var redirectLink = _chromeDriver.FindElementByPartialLinkText("Click here");
            redirectLink.Click();
        }

        [Then(@"I am taken to the display wallet page")]
        public void ThenIAmTakenToTheDisplayWalletPage()
        {
            var pageTitle = _chromeDriver.FindElementByTagName("h2").Text;
            Assert.That(pageTitle, Is.EqualTo("Wallet"));
        }
    }

    public class ChromeDriverTest
    {
        protected static ChromeDriver _chromeDriver;

        [BeforeScenario]
        public void SetUpSeleniumOnChrome()
        {
            _chromeDriver = new ChromeDriver("WebDriver") {Url = "localhost:27554"};
            _chromeDriver.Navigate();
        }

        [AfterScenario]
        public void TearDownChromeDriver()
        {
            _chromeDriver.Close();
            _chromeDriver.Quit();
        }
    }
}
