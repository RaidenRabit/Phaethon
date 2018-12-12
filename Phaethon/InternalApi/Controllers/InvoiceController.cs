using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Core.Model;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;
using Newtonsoft.Json;

namespace InternalApi.Controllers
{
    [RoutePrefix("Invoice")]
    public class InvoiceController: ApiController
    {
        private readonly IInvoiceDM _invoiceManagement;

        public InvoiceController()
        {
            _invoiceManagement = new InvoiceDM();
        }
        
        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(requestContent);
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
            DateTime fromDateTime = new DateTime(2000, 1, 1), toDateTime = DateTime.Now;
            if (name == null) name = "";
            DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDateTime);
            DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out toDateTime);
            if (docNumber == null) docNumber = "";
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.GetInvoices(numOfRecords, selectedCompany, name, selectedDate, fromDateTime, toDateTime, docNumber));
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> Delete()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            int id = JsonConvert.DeserializeObject<int>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.Delete(id));
        }
    }
}
