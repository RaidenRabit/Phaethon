using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
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

        #region Ajax
        [HttpGet]
        public async Task<string> GetProductAjax(int barcode)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = barcode.ToString();
            var response = await _client.GetAsync("GetProduct?" + parameters);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
