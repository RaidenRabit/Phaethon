using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Model;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class JobController : Controller
    {
        private readonly HttpClient _client;
        public JobController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Job/");
            _client = clientFactory.GetClient();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<string> GetJobs(int? numOfRecords, int? jobId, string jobName, int? jobStatus, string customerName, string description, string dateOption, string from, string to)
        {
            DateTime fromDateTime = DateTime.Now, toDateTime = DateTime.Now;
            int dateOp = 0;
            if(!from.IsNullOrWhiteSpace())
                DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDateTime);
            if(!to.IsNullOrWhiteSpace())
                DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out toDateTime);
            if (!dateOption.IsNullOrWhiteSpace())
                Int32.TryParse(dateOption, out dateOp);

            JobQueryFilter jobFilter = new JobQueryFilter { CustomerName = customerName, DateOption = dateOp, Description = description, From = fromDateTime, To = toDateTime };

            if(numOfRecords != null)
                    jobFilter.NumOfRecords = (int)numOfRecords;
            if (jobId != null)
                jobFilter.JobId = (int)jobId;
            if (jobStatus != null)
                jobFilter.JobStatus = (int) jobStatus;
            jobFilter.JobName = jobName;
            var response = await _client.PostAsJsonAsync("ReadAll", jobFilter);
            return await response.Content.ReadAsStringAsync();
        }

        [HttpGet]
        public ActionResult EditJob(List<Job> data)
        {
            return PartialView("_editJob", data[0]);
        }

        [HttpGet]
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
        public async Task PostJob(Job job)
        {
            await _client.PostAsJsonAsync("InsertOrUpdate", job);
        }

        [HttpGet]
        public async Task ResendNotification(string jobId)
        {
            if (!string.IsNullOrEmpty(jobId) && !jobId.Equals("0"))
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["id"] = jobId;
                await _client.GetAsync("ResendNotification?" + parameters);
            }
        }
    }
}