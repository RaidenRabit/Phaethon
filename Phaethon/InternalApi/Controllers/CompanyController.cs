using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [EnableCors(origins: "http://localhost:49873", headers: "*", methods: "*")]
    [RoutePrefix("Company")]
    public class CompanyController: ApiController
    {
        private readonly ICompanyDM _companyManagement;

        public CompanyController()
        {
            _companyManagement = new CompanyDM();
        }

        [Route("GetCompanies")]
        [HttpGet]
        public HttpResponseMessage GetCompanies()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _companyManagement.GetCompanies());
        }

        [Route("GetCompany")]
        [HttpGet]
        public HttpResponseMessage GetCompany(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _companyManagement.GetCompany(id));
        }
    }
}
