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
            catch (DbUpdateException)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Posted Object is invalid");
            }
        }

        [Route("Read")]
        [HttpGet]
        public async Task<HttpResponseMessage> Read(string id)
        {
            try
            {
                Int32.TryParse(id, out var Id);
                return Request.CreateResponse(HttpStatusCode.OK, _jobDm.Read(Id));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No such Id");
            }
        }
    }
}
