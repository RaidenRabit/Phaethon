using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult> Index()
        {
            var a = await _client.GetAsync("ReadAll");
            var b = JsonConvert.DeserializeObject<List<Job>>(await a.Content.ReadAsStringAsync());
            return View(b);
        }
    }
}