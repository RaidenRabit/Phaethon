using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Model;
using WebClient.Models;

namespace WebClient.Controllers
{
    [RoutePrefix("TaxGroup")]
    public class TaxGroupController : Controller
    {
        private readonly HttpClient _client;

        public TaxGroupController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/TaxGroup/");
            _client = clientFactory.GetClient();
        }

        #region Page
        [HttpGet]
        public ActionResult CreateGroup()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateGroup(TaxGroup taxGroup)
        {
            return await _client.PostAsJsonAsync("Create", taxGroup);
        }
        #endregion

        #region Ajax
        [HttpGet]
        public async Task<string> GetTaxGroupsAjax()
        {
            var response = await _client.GetAsync("GetTaxGroups");
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}