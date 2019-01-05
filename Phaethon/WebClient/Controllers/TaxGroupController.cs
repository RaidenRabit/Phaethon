using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Model;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class TaxGroupController : Controller
    {
        private readonly HttpClient _client;

        public TaxGroupController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/TaxGroup/");
            _client = clientFactory.GetClient();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Create(TaxGroup taxGroup)
        {
            return await _client.PostAsJsonAsync("Create", taxGroup);
        }

        //Ajax

        [Route("GetTaxGroups")]
        [HttpGet]
        public async Task<string> GetTaxGroups()
        {
            var response = await _client.GetAsync("GetTaxGroups");
            return await response.Content.ReadAsStringAsync();
        }
    }
}