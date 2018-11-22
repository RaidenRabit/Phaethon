using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Core.Model;

namespace ExternalApi.Controllers
{
    [EnableCors(origins: "http://localhost:49873", headers: "*", methods: "*")]
    [RoutePrefix("Api/TaxGroup")]
    public class TaxGroupController : ApiController
    {
        private readonly HttpClient _client;

        public TaxGroupController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/TaxGroup/");
        }

        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody] TaxGroup taxGroup)
        {
            return await _client.PostAsJsonAsync("Create", taxGroup);
        }

        [Route("GetTaxGroups")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTaxGroups()
        {
            return await _client.GetAsync("GetTaxGroups");
        }
    }
}
