using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    [RoutePrefix("Company")]
    public class CompanyController : Controller
    {
        private readonly HttpClient _client;

        public CompanyController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Company/");
            _client = clientFactory.GetClient();
        }

        #region Ajax
        [HttpGet]
        public async Task<string> GetCompaniesAjax()
        {
            var response = await _client.GetAsync("GetCompanies");
            return await response.Content.ReadAsStringAsync();
        }
        
        [HttpGet]
        public async Task<string> GetCompanyAjax(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var response = await _client.GetAsync("GetCompany?" + parameters);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
