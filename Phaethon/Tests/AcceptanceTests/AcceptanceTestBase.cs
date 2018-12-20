using System.Net.Http;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Tests.IntegrationTests;

namespace Tests.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        private InternalApiFakeServer _internalFakeServer;
        private WebClientFakeServer _webClientFakeServer;
        protected IWebDriver _chromeDriver;

        public AcceptanceTestBase()
        {
            _internalFakeServer = new InternalApiFakeServer();
            _webClientFakeServer = new WebClientFakeServer();
            _chromeDriver = new ChromeDriver();
            _chromeDriver.Navigate().GoToUrl("http://localhost:49873/");
        }

        [SetUp]
        public void TestsSetup()
        {
            _internalFakeServer.StartServer();

            _webClientFakeServer.StartServer();
        }

        [TearDown]
        public void TestsTearDown()
        {
            _chromeDriver.Dispose();
            _internalFakeServer.Dispose();
            _webClientFakeServer.Dispose();

        }
    }
}
