using System.Net.Http;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class IntegrationTestBase
    {
        private InternalApiFakeServer _internalFakeServer;
        protected HttpClient _internalClient;

        public IntegrationTestBase()
        {
            _internalFakeServer = new InternalApiFakeServer();
        }

        [SetUp]
        public void TestsSetup()
        {
            _internalFakeServer.StartServer();
            _internalClient = _internalFakeServer.GetInternalClient();
        }

        [TearDown]
        public void TestsTearDown()
        {
            _internalFakeServer.Dispose();
        }
    }
}
