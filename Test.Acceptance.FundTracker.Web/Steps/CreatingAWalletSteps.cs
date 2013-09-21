using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Test.Acceptance.FundTracker.Web.Steps
{
    [Binding]
    public class CreatingAWalletSteps : ChromeDriverTest
    {
        [BeforeFeature]
        public static void SetUp()
        {
            SetUpSeleniumOnChrome();
        }

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

        [Then(@"the name ""(.*)"" is displayed")]
        public void ThenTheNameIsDisplayed(string expectedWalletName)
        {
            var walletNameDisplayed = _chromeDriver.FindElementByClassName("wallet-name").Text;
            Assert.That(walletNameDisplayed, Is.EqualTo(expectedWalletName));
        }

        [AfterFeature]
        public static void TearDown()
        {
            TearDownChromeDriver();
        }
    }
}
