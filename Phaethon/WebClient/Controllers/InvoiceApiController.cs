using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebClient.Controllers
{
    [RoutePrefix("InvoiceApi")]
    public class InvoiceApiController : ApiController
    {
        [Route("GetInvoices")]
        [HttpGet]
        public HttpResponseMessage GetInvoices(int numOfRecords = 10, int selectedCompany = 0, string name = "", int selectedDate = 0, string from = "01/01/0001", string to = "01/01/2100")
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:64010/Invoice/GetInvoices" +
                                         "?numOfRecords=" + numOfRecords +
                                         "&selectedCompany=" + selectedCompany +
                                         "&name=" + name +
                                         "&selectedDate=" + selectedDate +
                                         "&from=" + from +
                                         "&to=" + to).Result;
            return result;
        }
    }
}
