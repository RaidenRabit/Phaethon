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

        [Route("GetInvoiceElements")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceElements(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _elementManagement.GetInvoiceElements(id));
        }
    }
}
