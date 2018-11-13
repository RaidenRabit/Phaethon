using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class InternalTestFakeServerBase
    {
        protected HttpClient _client;
        private HttpServer _server;

        public InternalTestFakeServerBase()
        {
        }

        [SetUp]
        public void StartServer()
        {
            HttpConfiguration config = new HttpConfiguration();

            config.Filters.Add(new Core.Decorators.ExceptionFilter());
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
                = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling
                = NullValueHandling.Ignore;
            InternalApi.WebApiConfig.Register(config);

            _server = new HttpServer(config);
            _client = new HttpClient(_server);
            _client.BaseAddress = new Uri("http://localhost:64007/");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        [TearDown]
        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
