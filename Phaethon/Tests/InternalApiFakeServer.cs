using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;

namespace Tests
{
    public class InternalApiFakeServer
    {
        private HttpClient _internalClient;
        private HttpServer _internalServer;

        public HttpClient GetInternalClient()
        {
            return _internalClient;
        }
        
        public void StartServer()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Filters.Add(new Core.Decorators.ExceptionFilter());
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
                = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling
                = NullValueHandling.Ignore;
            InternalApi.WebApiConfig.Register(config);

            _internalServer = new HttpServer(config);
            _internalClient = new HttpClient(_internalServer);
            _internalClient.BaseAddress = new Uri("http://localhost:64007/");
            _internalClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public void Dispose()
        {
            _internalClient.Dispose();
            _internalServer.Dispose();
        }
    }
}