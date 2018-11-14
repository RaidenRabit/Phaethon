using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExternalApi.Controllers
{
    [RoutePrefix("Job")]
    public class JobController : ApiController
    {
        private HttpClient _client;

        public JobController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Job/");
        }

        [Route("InsertOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            var request = new UriBuilder(Request.RequestUri)
            {
                Port = _client.BaseAddress.Port
            };
            return await _client.PostAsync(request.ToString(), Request.Content);
        }

        [Route("Read")]
        [HttpGet]
        public async Task<HttpResponseMessage> Read(string id)
        {
            var request = new UriBuilder(Request.RequestUri)
            {
                Port = _client.BaseAddress.Port
            };
            return await _client.PostAsync(request.ToString(), Request.Content);
        }

        [Route("ReadAll")]
        [HttpGet]
        public async Task<HttpResponseMessage> ReadAll()
        {
            UriBuilder request = new UriBuilder(Request.RequestUri)
            {
                Port = _client.BaseAddress.Port
            };
            return await _client.GetAsync(request.ToString());
        }
    }
}
