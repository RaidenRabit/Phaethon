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

        /// <summary>
        /// Get list of companies
        /// </summary>
        /// <returns>A list of companies inside the response's body</returns>
        /// <response code="200">Returns a list of companies</response>
        /// <response code="403">Missing/Invalid UserToken</response>     
        [Route("GetCompanies")]
        [HttpGet]
        public HttpResponseMessage GetCompanies()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _companyManagement.GetCompanies());
        }


        /// <summary>
        /// Get company by id
        /// </summary>
        /// <returns>A company, inside the response's body</returns>
        /// <response code="200">Returns a company</response>
        /// <response code="400">No company with such ID</response>
        /// <response code="403">Missing/Invalid UserToken</response>     
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
