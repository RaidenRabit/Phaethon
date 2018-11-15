using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebClient.Controllers
{
    [RoutePrefix("Api/ProductGroup")]
    public class ProductGroupApiController : ApiController
    {
        private readonly HttpClient _client;

        public ProductGroupApiController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64010/ProductGroup/");
        }

        [Route("GetProductGroups")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetProductGroups()
        {
            return await _client.GetAsync("GetProductGroups");
        }
    }
}
