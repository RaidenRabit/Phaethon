using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate()
        {
            try
            {
                var requestContent = await Request.Content.ReadAsStringAsync();
                Item item = JsonConvert.DeserializeObject<Item>(requestContent);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                _itemManagement.CreateOrUpdate(item);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("GetItem")]
        [HttpGet]
        public HttpResponseMessage GetItem(int id)
        {
            Item item = _itemManagement.GetItem(id);
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.GetItem(id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [Route("GetItems")]
        [HttpGet]
        public HttpResponseMessage GetItems(string serialNumber, string productName, int barcode, bool showAll)
        {
            try { 
                if (serialNumber == null) serialNumber = "";
                if (productName == null) productName = "";
                return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.GetItems(serialNumber, productName, barcode, showAll));
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> Delete()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            int id = JsonConvert.DeserializeObject<int>(requestContent);
            if (_itemManagement.Delete(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
