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
        [HttpPost]
        public async Task<HttpResponseMessage> ReadAll()
        {
            try
            {
                if (Request.Content == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, _jobDm.ReadAll(new JobQueryFilter()));
                }
                var json = await Request.Content.ReadAsStringAsync();
                JobQueryFilter jobQueryFilter = JsonConvert.DeserializeObject<JobQueryFilter>(json);
                return Request.CreateResponse(HttpStatusCode.OK, _jobDm.ReadAll(jobQueryFilter));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
