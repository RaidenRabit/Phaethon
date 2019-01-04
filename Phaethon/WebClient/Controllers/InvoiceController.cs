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
    public class InvoiceController : Controller
    {
        private readonly HttpClient _client;

        public InvoiceController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/");
            _client = clientFactory.GetClient();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Invoice/Edit/{id:int}")]
        public async Task<ActionResult> Edit(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var result = await _client.GetAsync("Invoice/GetInvoice?" + parameters);
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(await result.Content.ReadAsStringAsync());
            return View(invoice);
        }

        [HttpGet]
        [Route("Invoice/Edit/{incoming:bool}")]
        public ActionResult Edit(bool incoming)
        {
            Invoice invoice = new Invoice();
            invoice.Incoming = incoming;
            return View(invoice);
        }

        [HttpPost]
        [Route("Invoice/Edit")]
        public async Task<ActionResult> Edit(Invoice invoice)
        {
            var response = await _client.PostAsJsonAsync("Invoice/CreateOrUpdate", invoice);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            if (HttpStatusCode.OK == response.StatusCode && deserializedResponse)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _client.PostAsJsonAsync("Invoice/Delete", id);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            if (HttpStatusCode.OK == response.StatusCode && deserializedResponse)
            {
                return Json(new { newUrl = Url.Action("Index") });
            }
            return null;
        }
    }
}