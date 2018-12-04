using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [EnableCors(origins: "http://localhost:49873", headers: "*", methods: "*")]
    [RoutePrefix("Product")]
    public class ProductController : ApiController
    {
        private readonly IProductDM _productManagement = null;

        public ProductController()
        {
            _productManagement = new ProductDM();
        }

        [Route("GetProduct")]
        [HttpGet]
        public HttpResponseMessage GetProduct(int barcode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _productManagement.GetProduct(barcode));
        }
    }
}
