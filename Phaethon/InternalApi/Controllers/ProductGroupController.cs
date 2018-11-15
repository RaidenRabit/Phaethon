using System.Net;
using System.Net.Http;
using System.Web.Http;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [RoutePrefix("ProductGroup")]
    public class ProductGroupController: ApiController
    {
        private readonly IProductGroupManagement _productGroupManagement = null;

        public ProductGroupController()
        {
            _productGroupManagement = new ProductGroupManagement();
        }

        [Route("GetProductGroups")]
        [HttpGet]
        public HttpResponseMessage GetProductGroups()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _productGroupManagement.GetProductGroups());
        }
    }
}