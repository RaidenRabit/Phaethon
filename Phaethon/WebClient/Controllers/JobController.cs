using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Job
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetJobs")]
        public async Task<string> GetJobs(int? numOfRecords, int? jobId, string jobName, int? jobStatus, string customerName, string description)
        {
            var a = await _client.GetAsync("ReadAll?" + Request.Url.Query);
            return await a.Content.ReadAsStringAsync();
        }

        [HttpGet]
        [Route("EditJob")]
        public ActionResult EditJob(List<Job> data)
        {
            return PartialView("_editJob", data[0]);
        }

    [HttpGet]
    [Route("ReadJob")]
        public async Task<ActionResult> ReadJob(string id)
        {
            Job job;
            if (!string.IsNullOrEmpty(id) && !id.Equals("0"))
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["id"] = id;
                var result = await _client.GetAsync("Read?" + parameters);
                string json = await result.Content.ReadAsStringAsync();
                job = JsonConvert.DeserializeObject<Job>(json);
            }
            else
                job = new Job {ID = 0};
            return PartialView("_editJob", job);
        }

        [HttpPost]
        [Route("PostJob")]
        public async Task PostJob(Job job)
        {
            await _client.PostAsJsonAsync("InsertOrUpdate", job);
        }
    }
}