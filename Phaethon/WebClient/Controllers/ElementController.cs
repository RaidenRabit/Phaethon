using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebClient.Models;

namespace WebClient.Controllers.Api
{
    [RoutePrefix("Element")]
    public class ElementController : Controller
    {
        private readonly HttpClient _client;

        public ElementController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Element/");
            _client = clientFactory.GetClient();
        }

        //Ajax

        [Route("GetInvoiceElements")]
        [HttpGet]
        public async Task<string> GetInvoiceElements(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var response = await _client.GetAsync("GetInvoiceElements?" + parameters);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
