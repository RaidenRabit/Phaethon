using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [RoutePrefix("Representative")]
    public class RepresentativeController : ApiController
    {
        private readonly IRepresentativeManagement _representativeManagement = null;

        public RepresentativeController()
        {
            _representativeManagement = new RepresentativeManagement();
        }

        [Route("GetRepresentatives")]
        [HttpGet]
        public HttpResponseMessage GetRepresentatives(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _representativeManagement.GetRepresentatives(id));
        }
    }
}
