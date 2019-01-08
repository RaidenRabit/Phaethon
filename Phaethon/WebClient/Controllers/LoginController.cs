using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Model;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using WebClient.Models;
using WebClient.Resources.Language_Files;

namespace WebClient.Controllers
{
    [RoutePrefix("Login")]
    public class LoginController : Controller
    {
        private readonly HttpClient _client;

        public LoginController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Login/");
            _client = clientFactory.GetClient();
        }

        #region Page
        [HttpGet]
        public ActionResult Index()
        {
            Session["userToken"] = null;
            Session["ID"] = null;
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> Index(Login loginModel)
        {
            var response = await _client.PostAsJsonAsync("Login", loginModel);
            if (HttpStatusCode.OK == response.StatusCode)
            {
                string deserializedResponse = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                if (!deserializedResponse.IsNullOrWhiteSpace() && !deserializedResponse.Contains("Incorrect login credentials"))
                {
                    string cleanToken = deserializedResponse.Substring(deserializedResponse.IndexOf(" ")+1);
                    Session["userToken"] = cleanToken;
                    Session["ID"] = Int32.Parse(cleanToken.Remove(cleanToken.Length - 64));
                    return RedirectToAction("Index", "Invoice");
                }
            }
            ModelState.AddModelError("", LanguagePack.Error1);
            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> Edit(bool existing)
        {
            try
            {
                Login login = new Login();
                if (existing)
                {
                    var parameters = HttpUtility.ParseQueryString(string.Empty);
                    parameters["id"] = Session["ID"].ToString();
                    var result = await _client.GetAsync("GetLogin?" + parameters);
                    string json = result.Content.ReadAsStringAsync().Result;
                    login = JsonConvert.DeserializeObject<Login>(json);
                }
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
            return await Edit(loginModel.ID > 0);
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
        #endregion
    }
}