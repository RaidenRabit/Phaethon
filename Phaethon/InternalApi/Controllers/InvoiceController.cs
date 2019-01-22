using System;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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

        /// <summary>
        /// Creates or Updates an invoice. Distincion based on assigned object ID.
        /// ID = 0 -> new invoice
        /// ID != 0 -> update invoice
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Invalid posted object</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate([FromBody]Invoice invoice)
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            invoice = JsonConvert.DeserializeObject<Invoice>(requestContent);
            bool success = _invoiceManagement.CreateOrUpdate(invoice);
            if(success) {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Get invoice by ID
        /// </summary>
        /// <returns>Invoice, inside the response's body</returns>
        /// <response code="200">Returns an invoice</response>
        /// <response code="400">No invoice with such ID</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
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


        /// <summary>
        /// Get invoices by filter
        /// </summary>
        /// <returns>List of invoices, inside the response's body</returns>
        /// <param name="numOfRecords">Number of records to be selected from database</param>
        /// <param name="regNumber">Inboice registration number</param>
        /// <param name="docNumber">Inboice document number</param>
        /// <param name="from">Starting DateTime period</param>
        /// <param name="to">Ending DateTime period</param>
        /// <param name="company">Invoice company</param>
        /// <param name="sum">Invoice sum</param>
        /// <response code="200">Returns a list of invoices</response>
        /// <response code="400">No invoice meeting the filter criteria</response>
        /// <response code="403">Missing/Invalid UserToken</response>   
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


        /// <summary>
        /// Delete invoice
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">No invoice with such ID</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("Delete")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(int id)
        {
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
