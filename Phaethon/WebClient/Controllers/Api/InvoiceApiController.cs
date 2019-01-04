using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebClient.Controllers.Api
{
    [RoutePrefix("Api/Invoice")]
    public class InvoiceApiController : ApiController
    {
        private readonly HttpClient _client;

        public InvoiceApiController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Invoice/");
        }

        [Route("GetInvoices")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetInvoices(int numOfRecords = 10, string regNumber = "", string docNumber = "", string from = "01/01/0001", string to = "01/01/2100", string company = "", decimal sum = 0)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["numOfRecords"] = numOfRecords.ToString();
            parameters["regNumber"] = regNumber;
            parameters["docNumber"] = docNumber;
            parameters["from"] = from;
            parameters["to"] = to;
            parameters["company"] = company;
            parameters["sum"] = sum.ToString();
            return await _client.GetAsync("GetInvoices?" + parameters);
        }
    }
}
