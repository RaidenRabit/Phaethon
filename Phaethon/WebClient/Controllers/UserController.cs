using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Model;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;

        public UserController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/User/");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(User userModel)
        {
            var response = await _client.PostAsJsonAsync("Login", userModel);
            var deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

            if (HttpStatusCode.OK == response.StatusCode && deserializedResponse != 0)
            {
                Session["ID"] = deserializedResponse;
                return RedirectToAction("Edit", "User");
            }
            else
            {
                return View();
            }

        }
        
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["id"] = Session["ID"].ToString();
                var result = await _client.GetAsync("GetUser?" + parameters);
                string json = result.Content.ReadAsStringAsync().Result;
                User user = JsonConvert.DeserializeObject<User>(json);
                return View(user);
            }
            catch
            {
                return View(new User());
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(User userModel)
        {
            try
            {
                userModel.id = Int32.Parse(Session["ID"].ToString());
            }
            catch{}
            await _client.PostAsJsonAsync("CreateOrUpdate", userModel);
            return await Edit();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser()
        {
            var response = await _client.PostAsJsonAsync("Delete", Session["ID"].ToString());
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            if (HttpStatusCode.OK == response.StatusCode && deserializedResponse)
            {
                Session["ID"] = null;
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View("Error");
            }
        }
    }
}