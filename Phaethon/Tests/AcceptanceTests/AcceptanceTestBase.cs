using System.Net.Http;
using NUnit.Framework;
using Tests.IntegrationTests;

namespace Tests.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        private InternalApiFakeServer _internalFakeServer;
        private WebClientFakeServer _webClientFakeServer;
        private HttpClient _internalClient;
        protected HttpClient _webClient;

        public AcceptanceTestBase()
        {
            _internalFakeServer = new InternalApiFakeServer();
            _webClientFakeServer = new WebClientFakeServer();
        }

        [SetUp]
        public void TestsSetup()
        {
            _internalFakeServer.StartServer();
            _internalClient = _internalFakeServer.GetInternalClient();

            _webClientFakeServer.StartServer();
            _webClient = _webClientFakeServer.GetWebClient();
        }

        [TearDown]
        public void TestsTearDown()
        {
            _internalFakeServer.Dispose();
            _webClientFakeServer.Dispose();
        }
    }
}
