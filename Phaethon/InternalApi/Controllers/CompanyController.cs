using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Model;
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
            Company company = _companyManagement.GetCompany(id);
            if (company != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, company);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
