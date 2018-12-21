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
            Session["Username"] = "";
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> Index(Login loginModel)
        {
            var response = await _client.PostAsJsonAsync("Login", loginModel);
            if (HttpStatusCode.OK == response.StatusCode)
            {
                var deserializedResponse = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                if (deserializedResponse != 0)
                {
                    Session["ID"] = deserializedResponse;
                    Session["Username"] = loginModel.Username + " ";
                    return RedirectToAction("Edit", "Login");
                }
            }
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
            var response = await _client.PostAsJsonAsync("Delete", Session["ID"].ToString());

            if (HttpStatusCode.OK == response.StatusCode)
            {
                Session["ID"] = null;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            Session["ID"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}