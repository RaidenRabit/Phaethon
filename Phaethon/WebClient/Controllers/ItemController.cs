using System.Collections.Generic;
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
    [RoutePrefix("Item")]
    public class ItemController : Controller
    {
        private readonly HttpClient _client;

        public ItemController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Item/");
            _client = clientFactory.GetClient();
        }

        #region Page
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
            if (HttpStatusCode.OK == response.StatusCode)
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
        #endregion

        #region Ajax
        [HttpGet]
        public async Task<string> GetItemAjax(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            var response = await _client.GetAsync("GetItem?" + parameters);
            return await response.Content.ReadAsStringAsync();
        }
        
        [HttpGet]
        public async Task<string> GetItemsAjax(string serialNumber, string productName, int barcode)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["serialNumber"] = serialNumber;
            parameters["productName"] = productName;
            parameters["barcode"] = barcode.ToString();
            var response = await _client.GetAsync("GetItems?" + parameters);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}