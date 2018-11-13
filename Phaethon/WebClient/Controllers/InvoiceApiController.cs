using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebClient.Controllers
{
    [RoutePrefix("Api/Invoice")]
    public class InvoiceApiController : ApiController
    {
        private readonly HttpClient _client;

        public InvoiceApiController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64010/Invoice/");
        }

        [Route("GetInvoices")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetInvoices(int numOfRecords = 10, int selectedCompany = 0, string name = "", int selectedDate = 0, string from = "01/01/0001", string to = "01/01/2100", string docNumber = "")
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["numOfRecords"] = numOfRecords.ToString();
            parameters["selectedCompany"] = selectedCompany.ToString();
            parameters["name"] = name;
            parameters["selectedDate"] = selectedDate.ToString();
            parameters["from"] = from;
            parameters["to"] = to;
            parameters["docNumber"] = docNumber;
            return await _client.GetAsync("GetInvoices?" + parameters);
        }
    }
}
