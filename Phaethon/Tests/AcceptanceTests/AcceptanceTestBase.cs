using System.Net.Http;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Tests.IntegrationTests;

namespace Tests.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        private WebClientFakeServer _webClientFakeServer;
        protected IWebDriver _firefoxDriver;

        public AcceptanceTestBase()
        {
            _webClientFakeServer = new WebClientFakeServer();

            var firefoxDriverService = FirefoxDriverService.CreateDefaultService();
            firefoxDriverService.HideCommandPromptWindow = true;
            _firefoxDriver = new FirefoxDriver(firefoxDriverService, new FirefoxOptions());
        }

        [SetUp]
        public void TestsSetup()
        {

            _webClientFakeServer.StartServer();
            _firefoxDriver.Navigate().GoToUrl("http://localhost:49873/");
        }
        
        [TearDown]
        public void TestsTearDown()
        {
            _firefoxDriver.Dispose();
            _webClientFakeServer.Dispose();

        }
    }
}
