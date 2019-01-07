using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Model;
using WebClient.Models;

namespace WebClient.Controllers
{
    [RoutePrefix("ProductGroup")]
    public class ProductGroupController : Controller
    {
        private readonly HttpClient _client;

        public ProductGroupController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/ProductGroup/");
            _client = clientFactory.GetClient();
        }

        #region Page
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }
        
        [HttpPost]
        public async Task<HttpResponseMessage> Create(ProductGroup productGroup)
        {
            return await _client.PostAsJsonAsync("Create", productGroup);
        }
        #endregion

        #region Page
        [HttpGet]
        public async Task<string> GetProductGroupsAjax()
        {
            var response = await _client.GetAsync("GetProductGroups");
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}