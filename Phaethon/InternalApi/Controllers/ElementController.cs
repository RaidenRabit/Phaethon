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
    [RoutePrefix("Element")]
    public class ElementController : ApiController
    {
        private readonly IElementManagement _elementManagement = null;

        public ElementController()
        {
            _elementManagement = new ElementManagement();
        }

        [Route("GetInvoiceElements")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceElements(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _elementManagement.GetInvoiceElements(id));
        }
    }
}
