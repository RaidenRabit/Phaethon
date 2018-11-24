using Core.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ExternalApi.Controllers
{
    [EnableCors(origins: "http://localhost:49873", headers: "*", methods: "*")]
    [RoutePrefix("Api/Invoice")]
    public class InvoiceController : ApiController
    {
        private readonly HttpClient _client;

        public InvoiceController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Invoice/");
        }

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate([FromBody] Invoice invoice)
        {
            return await _client.PostAsJsonAsync("CreateOrUpdate", invoice);
        }

        [Route("GetInvoice")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetInvoice(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            return await _client.GetAsync("GetInvoice?" + parameters);
        }

        [Route("GetInvoices")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetInvoices(int numOfRecords, int selectedCompany, string name, int selectedDate, string from, string to, string docNumber)
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

        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> Delete([FromBody] int id)
        {
            return await _client.PostAsJsonAsync("Delete", id);
        }
    }
}
