using System;
using System.Data.Entity.Infrastructure;
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
        private readonly IEmailSenderDM _emailSenderDm;

        public JobController()
        {
            _jobDm = new JobDm();
            _emailSenderDm = new EmailSenderDM();
        }

        /// <summary>
        /// Creates or Updates a job. Distincion based on assigned object ID.
        /// ID = 0 -> new job
        /// ID != 0 -> update job
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Invalid posted object</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("InsertOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            Job job = JsonConvert.DeserializeObject<Job>(requestContent);
            try
            {
                int response;
                if (job.ID != 0)
                {
                    bool statusChanged = (_jobDm.Read(job.ID.ToString()).JobStatus != JobStatus_enum.Completed) && (job.JobStatus.Equals(JobStatus_enum.Completed));

                    response = _jobDm.Create(job);
                    if (statusChanged && response != 0)
                    {
                        string customerName = job.Customer.GivenName + " " + job.Customer.FamilyName;
                        _emailSenderDm.SendEmail(job.Customer.Email, customerName, job.JobName, job.Description);
                        job.NotificationTime = DateTime.Now;;
                    }
                }

                response = _jobDm.Create(job);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Get job by ID
        /// </summary>
        /// <returns>Job, inside response's body</returns>
        /// <response code="200"></response>
        /// <response code="400">No job with such ID</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
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

        /// <summary>
        /// Get a list of jobs
        /// </summary>
        /// <returns>List of jobs, inside response's body</returns>
        /// <response code="200"></response>
        /// <response code="400">No job meeting the filter criteria</response>
        /// <response code="403">Missing/Invalid UserToken</response>  
        [Route("ReadAll")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReadAll([FromBody]JobQueryFilter jobQueryFilter)
        {
            try
            {
                if (Request.Content == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, _jobDm.ReadAll(new JobQueryFilter()));
                }
                return Request.CreateResponse(HttpStatusCode.OK, _jobDm.ReadAll(jobQueryFilter));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }


        /// <summary>
        /// Send new email to customer, informing of job's completion
        /// </summary>
        /// <response code="200"></response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("ResendNotification")]
        [HttpGet]
        public HttpResponseMessage ResendNotification(string id)
        {
            Job job = _jobDm.Read(id);
            string customerName = job.Customer.GivenName + " " + job.Customer.FamilyName;
            _emailSenderDm.SendEmail(job.Customer.Email, customerName, job.JobName, job.Description);
            job.NotificationTime = DateTime.Now;
            _jobDm.Create(job);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
