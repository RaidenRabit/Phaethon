using System;
using System.Data.Entity.Infrastructure;
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
            bool success = _invoiceManagement.CreateOrUpdate(invoice);
            if(success) {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [Route("GetInvoice")]
        [HttpGet]
        public HttpResponseMessage GetInvoice(int id)
        {
            Invoice invoice = _invoiceManagement.GetInvoice(id);
            if (invoice != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, invoice);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [Route("GetInvoices")]
        [HttpGet]
        public HttpResponseMessage GetInvoices(int numOfRecords, string regNumber, string docNumber, string from, string to, string company, decimal sum)
        {
            try { 
                DateTime fromDateTime = new DateTime(2000, 1, 1), toDateTime = DateTime.Now;
                if (regNumber == null) regNumber = "";
                if (docNumber == null) docNumber = "";
                DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDateTime);
                DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out toDateTime);
                if (company == null) company = "";
                return Request.CreateResponse(HttpStatusCode.OK, _invoiceManagement.GetInvoices(numOfRecords, regNumber, docNumber, fromDateTime, toDateTime,
                    company, sum));
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> Delete()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            int id = JsonConvert.DeserializeObject<int>(requestContent);
            if (_invoiceManagement.Delete(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
