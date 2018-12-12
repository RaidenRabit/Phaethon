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
using WebClient.Controllers.Api;

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
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var result = await _client.GetAsync("GetItem?" + parameters);
            Item item = JsonConvert.DeserializeObject<Item>(await result.Content.ReadAsStringAsync());
            return View(item);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Item item)
        {
            var response = await _client.PostAsJsonAsync("CreateOrUpdate", item);
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

        [HttpGet]
        public ActionResult _select()
        {
            ViewBag.MethodCalled = new object();
            return PartialView(new List<Item>());
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _client.PostAsJsonAsync("Delete", id);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            if (HttpStatusCode.OK == response.StatusCode && deserializedResponse)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}