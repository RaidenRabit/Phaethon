using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebClient.Models;

namespace WebClient.Controllers.Api
{
    [RoutePrefix("Api/ProductGroup")]
    public class ProductGroupApiController : ApiController
    {
        private readonly HttpClient _client;

        public ProductGroupApiController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/ProductGroup/");
            _client = clientFactory.GetClient();
        }
        
        [Route("GetProductGroups")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetProductGroups()
        {
            return await _client.GetAsync("GetProductGroups");
        }
    }
}
