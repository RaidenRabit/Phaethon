using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebClient.Models;

namespace WebClient.Controllers.Api
{
    [RoutePrefix("Product")]
    public class ProductController : Controller
    {
        private readonly HttpClient _client;

        public ProductController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Product/");
            _client = clientFactory.GetClient();
        }

        [Route("GetProduct")]
        [HttpGet]
        public async Task<string> GetProduct(int barcode)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = barcode.ToString();
            var response = await _client.GetAsync("GetProduct?" + parameters);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
