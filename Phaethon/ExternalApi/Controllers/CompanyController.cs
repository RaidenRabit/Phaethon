using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExternalApi.Controllers
{
    [RoutePrefix("Company")]
    public class CompanyController : ApiController
    {
        [Route("GetCompanies")]
        [HttpGet]
        public HttpResponseMessage GetCompanies()
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:64007/Company/GetCompanies").Result;
            return result;
        }

        [Route("GetCompany")]
        [HttpGet]
        public HttpResponseMessage GetCompany(int id)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:64007/Company/GetCompany?id="+id).Result;
            return result;
        }
    }
}
