using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Model;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly HttpClient _client;

        public InvoiceController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64010/Invoice/");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var result = await _client.GetAsync("Read?" + parameters);
            string json = result.Content.ReadAsStringAsync().Result;
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(json);
            return View(invoice);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Invoice invoice)
        {
            var result = await _client.PostAsJsonAsync("Create", invoice);
            if (HttpStatusCode.OK == result.StatusCode)
            {
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public void Delete(int id)
        {
            _client.PostAsJsonAsync("Delete", id);
        }
    }
}