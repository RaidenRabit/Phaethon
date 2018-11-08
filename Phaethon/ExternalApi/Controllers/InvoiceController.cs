using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;

namespace ExternalApi.Controllers
{
    [RoutePrefix("Invoice")]
    public class InvoiceController : ApiController
    {
        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] Invoice invoice)
        {
            HttpClient client = new HttpClient();
            var result = client.PostAsJsonAsync("http://localhost:64007/Invoice/Create", invoice).Result;
            return result;
        }

        [Route("Read")]
        [HttpGet]
        public HttpResponseMessage Read(int id)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:64007/Invoice/Read?id=" + id).Result;
            return result;
        }

        [Route("GetInvoices")]
        [HttpGet]
        public HttpResponseMessage GetInvoices(int numOfRecords)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords="+numOfRecords).Result;
            return result;
        }

        [Route("Delete")]
        [HttpPost]
        public HttpResponseMessage Delete([FromBody] int id)
        {
            HttpClient client = new HttpClient();
            var result = client.PostAsJsonAsync("http://localhost:64007/Invoice/Delete", id).Result;
            return result;
        }
    }
}
