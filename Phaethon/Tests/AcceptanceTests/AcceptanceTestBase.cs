using System.Net.Http;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Tests.IntegrationTests;

namespace Tests.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        private WebClientFakeServer _webClientFakeServer;
        protected IWebDriver _chromeDriver;

        public AcceptanceTestBase()
        {
            _webClientFakeServer = new WebClientFakeServer();

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            _chromeDriver = new ChromeDriver(chromeDriverService, new ChromeOptions());
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
