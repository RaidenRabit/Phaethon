using System;
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
        public HttpResponseMessage Create([FromBody] Invoice invoice)
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
        public HttpResponseMessage GetInvoices(int numOfRecords, int selectedCompany, string name, int selectedDate, string from, string to, string docNumber)
        {
            if (name == null) name = "";
            if (from == null) from = "";
            if (to == null) to = "";
            if (docNumber == null) docNumber = "";
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.GetInvoices(numOfRecords, selectedCompany, name, selectedDate, DateTime.Parse(from), DateTime.Parse(to).AddDays(1), docNumber));
        }

        [Route("Delete")]
        [HttpPost]
        public HttpResponseMessage Delete([FromBody] int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.Delete(id));
        }
    }
}
