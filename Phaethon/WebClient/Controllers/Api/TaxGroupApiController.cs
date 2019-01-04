using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebClient.Models;

namespace WebClient.Controllers.Api
{
    [RoutePrefix("Api/TaxGroup")]
    public class TaxGroupApiController : ApiController
    {
        private readonly HttpClient _client;

        public TaxGroupApiController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/TaxGroup/");
            _client = clientFactory.GetClient();
        }

        [Route("GetTaxGroups")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTaxGroups()
        {
            return await _client.GetAsync("GetTaxGroups");
        }
    }
}
