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
    [RoutePrefix("Job")]
    public class JobController : Controller
    {
        private readonly HttpClient _client;
        public JobController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Job/");
            _client = clientFactory.GetClient();
        }

        #region Page
        public ActionResult Index()
        {
            return View();
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
                job = new Job { ID = 0 };
            return PartialView("_editJob", job);
        }
        #endregion

        #region Ajax
        [HttpGet]
        public async Task<string> GetJobsAjax(string jobName, string customerName, string description, int dateOption, string from, string to, int numOfRecords = 10, int jobId = 0, int jobStatus = 0)
        {
            DateTime fromDateTime = DateTime.Now, toDateTime = DateTime.Now;
            if (!from.IsNullOrWhiteSpace())
                DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out fromDateTime);
            if (!to.IsNullOrWhiteSpace())
                DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out toDateTime);

            JobQueryFilter jobFilter = new JobQueryFilter
            {
                NumOfRecords = numOfRecords,
                CustomerName = customerName,
                JobId = jobId,
                JobStatus = jobStatus,
                Description = description,
                DateOption = dateOption,
                From = fromDateTime,
                To = toDateTime
            };

            jobFilter.JobName = jobName;
            var response = await _client.PostAsJsonAsync("ReadAll", jobFilter);
            return await response.Content.ReadAsStringAsync();
        }

        [HttpPost]
        public async Task PostJobAjax(Job job)
        {
            await _client.PostAsJsonAsync("InsertOrUpdate", job);
        }

        [HttpGet]
        public async Task ResendNotificationAjax(string jobId)
        {
            if (!string.IsNullOrEmpty(jobId) && !jobId.Equals("0"))
            {
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["id"] = jobId;
                await _client.GetAsync("ResendNotification?" + parameters);
            }
        }
        #endregion
    }
}