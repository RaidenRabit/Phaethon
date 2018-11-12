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
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:64010/Invoice/Read?id=" + id).Result;
            string json = result.Content.ReadAsStringAsync().Result;
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(json);
            return View(invoice);
        }

        [HttpPost]
        public ActionResult Edit(Invoice invoice)
        {
            HttpClient client = new HttpClient();
            var result = client.PostAsJsonAsync("http://localhost:64010/Invoice/Create", invoice).Result;
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
            HttpClient client = new HttpClient();
            var result = client.PostAsJsonAsync("http://localhost:64010/Invoice/Delete", id).Result;
        }
    }
}