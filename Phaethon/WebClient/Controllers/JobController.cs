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
    public class JobController : Controller
    {
        private readonly HttpClient _client;
        public JobController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64010/Job/");
        }

        // GET: Job
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
            string json = await result.Content.ReadAsStringAsync();
            Job job = JsonConvert.DeserializeObject<Job>(json);
            return View(job);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Job job)
        {
            var result = await _client.PostAsJsonAsync("InsertOrUpdate", job);
            if (HttpStatusCode.OK == result.StatusCode)
            {
                return View();
            }
            else
            {
                return View("Error");
            }
        }
    }
}