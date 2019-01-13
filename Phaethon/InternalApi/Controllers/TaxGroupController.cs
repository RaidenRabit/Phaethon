using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Model;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;
using Newtonsoft.Json;

namespace InternalApi.Controllers
{
    [RoutePrefix("TaxGroup")]
    public class TaxGroupController : ApiController
    {
        private readonly ITaxGroupDM _taxGroupManagement = null;

        public TaxGroupController()
        {
            _taxGroupManagement = new TaxGroupDM();
        }

        /// <summary>
        /// Creates or Updates a TaxGroup. Distinction based on assigned object ID.
        /// ID = 0 -> new TaxGroup
        /// ID != 0 -> update TaxGroup
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Invalid posted object</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody]TaxGroup taxGroup)
        {
            try
            {
                if (taxGroup == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                _taxGroupManagement.Create(taxGroup);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Get a list of TaxGroups
        /// </summary>
        /// <returns>A list of TaxGroups, inside response's body</returns>
        /// <response code="200">A list of TaxGroups</response>
        /// <response code="400"></response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("GetTaxGroups")]
        [HttpGet]
        public HttpResponseMessage GetTaxGroups()
        {
            try { 
                return Request.CreateResponse(HttpStatusCode.OK, _taxGroupManagement.GetTaxGroups());
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
