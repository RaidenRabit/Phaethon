using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Model;
using Newtonsoft.Json;
using WebClient.Models;

namespace WebClient.Controllers
{
    [RoutePrefix("Invoice")]
    public class InvoiceController : Controller
    {
        private readonly HttpClient _client;

        public InvoiceController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Invoice/");
            _client = clientFactory.GetClient();
        }

        #region Page
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Edit/{id:int}")]
        public async Task<ActionResult> Edit(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var result = await _client.GetAsync("GetInvoice?" + parameters);
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(await result.Content.ReadAsStringAsync());
            return View(invoice);
        }

        [HttpGet]
        [Route("Edit/{incoming:bool}")]
        public ActionResult Edit(bool incoming)
        {
            Invoice invoice = new Invoice();
            invoice.Incoming = incoming;
            return View(invoice);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Invoice invoice)
        {
            var response = await _client.PostAsJsonAsync("CreateOrUpdate", invoice);
            if (HttpStatusCode.OK == response.StatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var response = await _client.DeleteAsync("Delete?" + parameters);
            if (HttpStatusCode.OK == response.StatusCode)
            {
                return Json(new { newUrl = Url.Action("Index") });
            }
            return null;
        }
        #endregion

        #region Ajax
        [HttpGet]
        public async Task<string> GetInvoicesAjax(int numOfRecords = 10, string regNumber = "", string docNumber = "", string from = "01/01/0001", string to = "01/01/2100", string company = "", decimal sum = 0)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["numOfRecords"] = numOfRecords.ToString();
            parameters["regNumber"] = regNumber;
            parameters["docNumber"] = docNumber;
            parameters["from"] = from;
            parameters["to"] = to;
            parameters["company"] = company;
            parameters["sum"] = sum.ToString();
            var response = await _client.GetAsync("GetInvoices?" + parameters);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}