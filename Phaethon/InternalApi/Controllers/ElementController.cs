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
        private readonly IElementDM _elementManagement;

        public ElementController()
        {
            _elementManagement = new ElementDM();
        }

        /// <summary>
        /// Get invoice elements, by ID
        /// </summary>
        /// <returns>Invoice elemnt, inside the response's body</returns>
        /// <response code="200">Returns an element</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("GetInvoiceElements")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceElements(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _elementManagement.GetInvoiceElements(id));
        }
    }
}
