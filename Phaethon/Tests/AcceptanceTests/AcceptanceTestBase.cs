using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tests.IntegrationTests;

namespace Tests.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        private InternalApiFakeServer _internalFakeServer;
        protected HttpClient _internalClient;
    }
}
