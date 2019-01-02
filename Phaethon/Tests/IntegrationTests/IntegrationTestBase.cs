using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
