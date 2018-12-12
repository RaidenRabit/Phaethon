using System.Net;
using System.Net.Http;
using System.Web.Http;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
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
