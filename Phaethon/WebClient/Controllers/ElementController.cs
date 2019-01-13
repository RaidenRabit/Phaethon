using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
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

        #region Ajax
        [HttpGet]
        public async Task<string> GetInvoiceElementsAjax(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var response = await _client.GetAsync("GetInvoiceElements?" + parameters);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
