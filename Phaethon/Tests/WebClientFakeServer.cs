using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;
using WebClient;

namespace Tests
{
    public class WebClientFakeServer
    {
        protected HttpClient _webClient;
        private HttpServer _webClientServer;

        public HttpClient GetWebClient()
        {
            return _webClient;
        }

        public void StartServer()
        {
            //set up configuration for the server
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            config.Filters.Add(new Core.Decorators.ExceptionFilter());
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
                = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling
                = NullValueHandling.Ignore;
            InternalApi.WebApiConfig.Register(config);

            //create server and client
            _webClientServer = new HttpServer(config);
            _webClient = new HttpClient(_webClientServer);
            _webClient.BaseAddress = new Uri("http://localhost:64007/");
            _webClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public void Dispose()
        {
            _webClient.Dispose();
            _webClientServer.Dispose();
        }
    }
}
