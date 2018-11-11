using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExternalApi.Controllers
{
    [RoutePrefix("Representative")]
    public class RepresentativeController : ApiController
    {
        [Route("GetRepresentatives")]
        [HttpGet]
        public HttpResponseMessage GetRepresentatives(int id)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:64007/Representative/GetRepresentatives?id="+id).Result;
            return result;
        }
    }
}
