using System;
using System.Net.Http;
using System.Web.Http;

namespace Tests.IntegrationTests
{
    public class InternalTestFakeServerBase
    {
        public HttpClient _client;
        private HttpServer _server;

        public InternalTestFakeServerBase()
        {
            var config = new HttpConfiguration();
            InternalApi.WebApiConfig.Register(config);
            // additional config ...
            _server = new HttpServer(config);
            _client = new HttpClient(_server);
            _client.BaseAddress = new Uri("http://localhost:64007/");
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
