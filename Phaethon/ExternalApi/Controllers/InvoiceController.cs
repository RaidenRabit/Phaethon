using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace ExternalApi.Controllers
{
    [RoutePrefix("Invoice")]
    public class InvoiceController : ApiController
    {
        private readonly HttpClient _client;

        public InvoiceController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Invoice/");
        }

        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody] Invoice invoice)
        {
            return await _client.PostAsJsonAsync("Create", invoice);
        }

        [Route("Read")]
        [HttpGet]
        public async Task<HttpResponseMessage> Read(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            return await _client.GetAsync("Read?" + parameters);
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
