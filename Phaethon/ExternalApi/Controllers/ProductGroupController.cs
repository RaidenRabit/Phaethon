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
    [RoutePrefix("Api/ProductGroup")]
    public class ProductGroupController : ApiController
    {
        private readonly HttpClient _client;

        public ProductGroupController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/ProductGroup/");
        }

        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody] ProductGroup productGroup)
        {
            return await _client.PostAsJsonAsync("Create", productGroup);
        }

        [Route("GetProductGroups")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetProductGroups()
        {
            return await _client.GetAsync("GetProductGroups");
        }
    }
}
