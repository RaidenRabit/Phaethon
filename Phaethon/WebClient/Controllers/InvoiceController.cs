using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Core.Model;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    public class InvoiceController : Controller
    {
        public ActionResult Index()
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:64007/Invoice/GetInvoices").Result;
            string json = result.Content.ReadAsStringAsync().Result;
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(json);
            return View(invoices);
        }

        public ActionResult Statement()
        {
            return View();
        }
    }
}