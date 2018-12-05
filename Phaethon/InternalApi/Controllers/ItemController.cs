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
    [RoutePrefix("Item")]
    public class ItemController : ApiController
    {
        private readonly IItemDM _itemManagement;

        public ItemController()
        {
            _itemManagement = new ItemDM();
        }

        [Route("GetItem")]
        [HttpGet]
        public HttpResponseMessage GetItem(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.GetItem(id));
        }

        [Route("GetItems")]
        [HttpGet]
        public HttpResponseMessage GetItems(string serialNumber, string productName, int barcode)
        {
            if (serialNumber == null) serialNumber = "";
            if (productName == null) productName = "";
            return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.GetItems(serialNumber, productName, barcode));
        }
    }
}
