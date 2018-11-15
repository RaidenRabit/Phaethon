using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [RoutePrefix("Product")]
    public class ProductController : ApiController
    {
        private readonly IProductManagement _productManagement = null;

        public ProductController()
        {
            _productManagement = new ProductManagement();
        }

        [Route("GetProduct")]
        [HttpGet]
        public HttpResponseMessage GetProduct(int barcode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _productManagement.GetProduct(barcode));
        }
    }
}
