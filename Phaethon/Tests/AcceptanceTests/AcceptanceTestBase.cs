using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Tests.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        private WebClientFakeServer _webClientFakeServer;
        protected IWebDriver _chromeDriver;
        private WebDriverWait _wait;

        public AcceptanceTestBase()
        {
            _webClientFakeServer = new WebClientFakeServer();

            //in case you want firefox, just uncomment this, and comment the chrome one :D
            /* 
            var firefoxDriverService = FirefoxDriverService.CreateDefaultService();
            firefoxDriverService.HideCommandPromptWindow = true;
            _firefoxDriver = new FirefoxDriver(firefoxDriverService, new FirefoxOptions());
            */

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            _chromeDriver = new ChromeDriver(chromeDriverService, new ChromeOptions());
            _wait = new WebDriverWait(_chromeDriver, TimeSpan.FromSeconds(2));
        }

        protected void WaitForStaleness(IWebElement element)
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(element));
        }

        [SetUp]
        public void TestsSetup()
        {

            _webClientFakeServer.StartServer();
            _chromeDriver.Navigate().GoToUrl("http://localhost:49873/");
        }
        
        [TearDown]
        public void TestsTearDown()
        {
            _chromeDriver.Dispose();
            _webClientFakeServer.Dispose();

        }
    }
}
