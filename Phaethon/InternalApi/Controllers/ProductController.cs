using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Model;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [RoutePrefix("Product")]
    public class ProductController : ApiController
    {
        private readonly IProductDM _productManagement = null;

        public ProductController()
        {
            _productManagement = new ProductDM();
        }

        /// <summary>
        /// Get Product by barcode(in numeral form)
        /// </summary>
        /// <returns>Product, inside response's body</returns>
        /// <response code="200"></response>
        /// <response code="400">No Product with such ID</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("GetProduct")]
        [HttpGet]
        public HttpResponseMessage GetProduct(int barcode)
        {
            Product product = _productManagement.GetProduct(barcode);
            if (product != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
