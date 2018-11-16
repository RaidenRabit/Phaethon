using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Model;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;
using Newtonsoft.Json;

namespace InternalApi.Controllers
{
    [RoutePrefix("Job")]
    public class JobController : ApiController
    {
        private readonly IJobDm _jobDm;

        public JobController()
        {
            _jobDm = new JobDm();
        }
        
        [Route("InsertOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            Job job = JsonConvert.DeserializeObject<Job>(requestContent);
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _jobDm.Create(job));
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("Read")]
        [HttpGet]
        public HttpResponseMessage Read(string id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _jobDm.Read(id));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("ReadAll")]
        [HttpGet]
        public HttpResponseMessage ReadAll(int numOfRecords = 10, int jobId = 0, string jobName = "", string from = "", string to = "", int jobStatus = 0, int dateOption = 0, string customerName = "", string description ="")
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _jobDm.ReadAll(numOfRecords, jobId, jobName, from, to, jobStatus, dateOption, customerName, description));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
