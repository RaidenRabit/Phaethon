using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ExternalApi.Controllers
{
    [RoutePrefix("Element")]
    public class ElementController : ApiController
    {
        private readonly HttpClient _client;

        public ElementController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Element/");
        }

        [Route("GetInvoiceElements")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetInvoiceElements(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            return await _client.GetAsync("GetInvoiceElements?" + parameters);
        }
    }
}
