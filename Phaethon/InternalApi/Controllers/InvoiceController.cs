using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using InternalApi.DataAccess;

namespace InternalApi.Controller
{
    [RoutePrefix("Invoice")]
    public class InvoiceController: ApiController
    {
        private readonly InvoiceDa _invoiceDa;

        public InvoiceController()
        {
            _invoiceDa = new InvoiceDa();
        }
        
        //do like this
        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage Create(Invoice invoice)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _invoiceDa.Create(invoice));
        }

        [Route("Read")]
        [HttpGet]//post
        public Invoice Read(int id)
        {
            try
            {
                return _invoiceDa.Read(id);
            }
            catch
            {
                return null;
            }
        }

        [Route("GetInvoices")]
        [HttpGet]
        public List<Invoice> GetInvoices()
        {
            try
            {
                return _invoiceDa.GetInvoices();
            }
            catch
            {
                return null;
            }
        }

        [Route("Delete")]
        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                return _invoiceDa.Delete(id);
            }
            catch
            {
                return false;
            }
        }
    }
}
