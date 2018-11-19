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
    [RoutePrefix("TaxGroup")]
    public class TaxGroupController : ApiController
    {
        private readonly ITaxGroupManagement _taxGroupManagement = null;

        public TaxGroupController()
        {
            _taxGroupManagement = new TaxGroupManagement();
        }

        [Route("GetTaxGroups")]
        [HttpGet]
        public HttpResponseMessage GetTaxGroups()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _taxGroupManagement.GetTaxGroups());
        }
    }
}
