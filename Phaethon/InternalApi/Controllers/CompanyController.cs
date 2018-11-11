using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [RoutePrefix("Company")]
    public class CompanyController: ApiController
    {
        private readonly ICompanyManagement _companyManagement = null;

        public CompanyController()
        {
            _companyManagement = new CompanyManagement();
        }

        [Route("GetCompanies")]
        [HttpGet]
        public HttpResponseMessage GetCompanies()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _companyManagement.GetCompanies());
        }
    }
}
