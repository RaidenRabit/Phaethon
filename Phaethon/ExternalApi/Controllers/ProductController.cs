using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ExternalApi.Controllers
{
    [EnableCors(origins: "http://localhost:49873", headers: "*", methods: "*")]
    [RoutePrefix("Api/Product")]
    public class ProductController : ApiController
    {
        private readonly HttpClient _client;

        public ProductController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Product/");
        }

        [Route("GetProduct")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetProduct(int barcode)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = barcode.ToString();
            return await _client.GetAsync("GetProduct?" + parameters);
        }
    }
}
