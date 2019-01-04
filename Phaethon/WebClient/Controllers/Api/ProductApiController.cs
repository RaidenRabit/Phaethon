using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebClient.Models;

namespace WebClient.Controllers.Api
{
    [RoutePrefix("Api/Product")]
    public class ProductApiController : ApiController
    {
        private readonly HttpClient _client;

        public ProductApiController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Product/");
            _client = clientFactory.GetClient();
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
