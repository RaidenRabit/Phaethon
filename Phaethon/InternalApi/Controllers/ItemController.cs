using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.Controllers
{
    [EnableCors(origins: "http://localhost:49873", headers: "*", methods: "*")]
    [RoutePrefix("Item")]
    public class ItemController : ApiController
    {
        private readonly IItemManagement _itemManagement;

        public ItemController()
        {
            _itemManagement = new ItemManagement();
        }

        [Route("GetItem")]
        [HttpGet]
        public HttpResponseMessage GetItem(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.GetItem(id));
        }
    }
}
