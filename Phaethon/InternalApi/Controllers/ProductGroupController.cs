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
    [RoutePrefix("ProductGroup")]
    public class ProductGroupController: ApiController
    {
        private readonly IProductGroupDM _productGroupManagement = null;

        public ProductGroupController()
        {
            _productGroupManagement = new ProductGroupDM();
        }

        /// <summary>
        /// Creates or Updates a ProductGroup. Distincion based on assigned object ID.
        /// ID = 0 -> new ProductGroup
        /// ID != 0 -> update ProductGroup
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Invalid posted object</response>
        /// <response code="403">Missing/Invalid UserToken</response>   
        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody]ProductGroup productGroup)
        {
            try
            {
                if (productGroup == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                _productGroupManagement.Create(productGroup);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Get a list of ProductGroups
        /// </summary>
        /// <returns>A list of ProductGroups, inside response's body</returns>
        /// <response code="200"></response>
        /// <response code="400"></response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("GetProductGroups")]
        [HttpGet]
        public HttpResponseMessage GetProductGroups()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _productGroupManagement.GetProductGroups());
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}