using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Core.Model;

namespace WebClient.Controllers
{
    public class ItemController : Controller
    {
        private readonly HttpClient _client;

        public ItemController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Item/");
        }

        [HttpGet]
        public ActionResult Select()
        {
            return PartialView(new List<Item>());
        }
    }
}