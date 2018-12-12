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
        public HttpResponseMessage GetInvoices(int numOfRecords, string regNumber, string docNumber, string from, string to, string company, decimal sum)
        { 
            DateTime fromDateTime = new DateTime(2000, 1, 1), toDateTime = DateTime.Now;
            if (regNumber == null) regNumber = "";
            if (docNumber == null) docNumber = "";
            DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDateTime);
            DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out toDateTime);
            if (company == null) company = "";
            var t = _invoiceManagement.GetInvoices(numOfRecords, regNumber, docNumber, fromDateTime, toDateTime,
                company, sum);
            return Request.CreateResponse(HttpStatusCode.OK, t);
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
