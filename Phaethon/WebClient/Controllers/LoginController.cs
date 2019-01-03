using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Model;
using Newtonsoft.Json;
using WebClient.Resources.Language_Files;

namespace WebClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;

        public LoginController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/Login/");
            
        }

        [HttpGet]
        public ActionResult Index()
        {
            Session["ID"] = null;
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> Index(Login loginModel)
        {
            var response = await _client.PostAsJsonAsync("Login", loginModel);
            if (HttpStatusCode.OK == response.StatusCode)
            {
                int deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                if (deserializedResponse != 0)
                {
                    Session["ID"] = deserializedResponse;
                    return RedirectToAction("Index", "Invoice");
                }
            }
            ModelState.AddModelError("", LanguagePack.Error1);
            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["id"] = Session["ID"].ToString();
                var result = await _client.GetAsync("GetLogin?" + parameters);
                string json = result.Content.ReadAsStringAsync().Result;
                Login login = JsonConvert.DeserializeObject<Login>(json);
                return View(login);
            }
            catch
            {
                return Index();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Login loginModel)
        {
            await _client.PostAsJsonAsync("CreateOrUpdate", loginModel);
            return await Edit();
        }

        [HttpGet]
        public async Task<ActionResult> Delete()
        {
            try
            {
                var response = await _client.PostAsJsonAsync("Delete", Session["ID"].ToString());
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View("Error");
            }
        }
    }
}