using System;
using System.Globalization;
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
        
        [Route("CreateOrUpdate")]
        [HttpPost]
        public HttpResponseMessage CreateOrUpdate([FromBody] Invoice invoice)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.CreateOrUpdate(invoice));
        }

        [Route("GetInvoice")]
        [HttpGet]
        public HttpResponseMessage GetInvoice(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.GetInvoice(id));
        }

        [Route("GetInvoices")]
        [HttpGet]
        public HttpResponseMessage GetInvoices(int numOfRecords, int selectedCompany, string name, int selectedDate, string from, string to, string docNumber)
        {
            if (name == null) name = "";
            if (from == null) from = "";
            if (to == null) to = "";
            if (docNumber == null) docNumber = "";
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.GetInvoices(numOfRecords, selectedCompany, name, selectedDate, DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.CurrentCulture), DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.CurrentCulture), docNumber));
        }

        [Route("Delete")]
        [HttpPost]
        public HttpResponseMessage Delete([FromBody] int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.Delete(id));
        }
    }
}
