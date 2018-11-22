using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Model;
using Newtonsoft.Json;

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
            return await _client.GetAsync(request.ToString());
        }

        [Route("ReadAll")]
        [HttpGet]
        public async Task<HttpResponseMessage> ReadAll(int? numOfRecords = 10, int? jobId = 0, string jobName = "", int? jobStatus = 0, string customerName = "", string description ="", string dateOption = "", string from = "", string to = "")
        {
            UriBuilder request = new UriBuilder(Request.RequestUri)
            {
                Port = _client.BaseAddress.Port
            };
            return await _client.GetAsync(request.ToString()); ;
        }
    }
}
