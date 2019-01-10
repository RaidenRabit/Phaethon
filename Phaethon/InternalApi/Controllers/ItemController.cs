using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Model;
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


        /// <summary>
        /// Creates or Updates an item. Distincion based on assigned object ID.
        /// ID = 0 -> new item
        /// ID != 0 -> update item
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Invalid posted object</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate([FromBody]Item item)
        {
            try
            {
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


        /// <summary>
        /// Get item by ID
        /// </summary>
        /// <returns>Item, inside response's body</returns>
        /// <response code="200"></response>
        /// <response code="400">No item with such ID</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("GetItem")]
        [HttpGet]
        public HttpResponseMessage GetItem(int id)
        {
            Item item = _itemManagement.GetItem(id);
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }


        /// <summary>
        /// Get a list of items
        /// </summary>
        /// <returns>List of items, inside response's body</returns>
        /// <param name="serialNumber">Item's serial number</param>
        /// <param name="productName">Item name</param>
        /// <param name="barcode">Item barcodoe (in numeral format)</param>
        /// <param name="showAll">Show all items, regardless of previous filters</param>
        /// <response code="200"></response>
        /// <response code="400">No item meeting the filter criteria</response>
        /// <response code="403">Missing/Invalid UserToken</response>  
        [Route("GetItems")]
        [HttpGet]
        public HttpResponseMessage GetItems(string serialNumber, string productName, int barcode)
        {
            try { 
                if (serialNumber == null) serialNumber = "";
                if (productName == null) productName = "";
                return Request.CreateResponse(HttpStatusCode.OK, _itemManagement.GetItems(serialNumber, productName, barcode));
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">No item with such ID</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> Delete([FromBody]int id)
        {
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
