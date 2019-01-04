using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebClient.Models;

namespace WebClient.Controllers.Api
{
    [RoutePrefix("Api/Company")]
    public class CompanyApiController : ApiController
    {
        private readonly HttpClient _client;

        public CompanyApiController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Company/");
            _client = clientFactory.GetClient();
        }

        [Route("GetCompanies")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCompanies()
        {
            return await _client.GetAsync("GetCompanies");
        }

        [Route("GetCompany")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCompany(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            return await _client.GetAsync("GetCompany?" + parameters);
        }
    }
}
