using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Core.Model;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;
using Newtonsoft.Json;

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

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            Item item = JsonConvert.DeserializeObject<Item>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.CreateOrUpdate(item));
        }

        [Route("GetItems")]
        [HttpGet]
        public HttpResponseMessage GetItems(string serialNumber, string productName, int barcode)
        {
            if (serialNumber == null) serialNumber = "";
            if (productName == null) productName = "";
            return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.GetItems(serialNumber, productName, barcode));
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> Delete()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            int id = JsonConvert.DeserializeObject<int>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.Delete(id));
        }
    }
}
