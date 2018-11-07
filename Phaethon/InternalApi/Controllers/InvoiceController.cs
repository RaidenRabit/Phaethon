using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Model;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [RoutePrefix("Invoice")]
    public class InvoiceController: ApiController
    {
        private readonly IInvoiceManagement _invoiceManagement;

        public InvoiceController()
        {
            _invoiceManagement = new InvoiceManagement();
        }
        
        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage Create(Invoice invoice)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.Create(invoice));
        }

        [Route("Read")]
        [HttpGet]
        public HttpResponseMessage Read(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.Read(id));
        }

        [Route("GetInvoices")]
        [HttpGet]
        public HttpResponseMessage GetInvoices()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.GetInvoices());
        }

        [Route("Delete")]
        [HttpPost]
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.Delete(id));
        }
    }
}
