using System.Net.Http;
using System.Web.Http;

namespace Tests.IntegrationTests
{
    public class IntegrationTestBase
    {
            public HttpClient _client;
            private HttpServer _server;

            public IntegrationTestBase()
            {
                var config = new HttpConfiguration();
                InternalApi.WebApiConfig.Register(config);
                // additional config ...
                _server = new HttpServer(config);
                _client = new HttpClient(_server);
            }

            public void Dispose()
            {
                _client.Dispose();
                _server.Dispose();
            }

    }
}
