using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ExternalApi.Controllers
{
    [EnableCors(origins: "http://localhost:49873", headers: "*", methods: "*")]
    [RoutePrefix("Api/Company")]
    public class CompanyController : ApiController
    {
        private readonly HttpClient _client;

        public CompanyController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Company/");
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
