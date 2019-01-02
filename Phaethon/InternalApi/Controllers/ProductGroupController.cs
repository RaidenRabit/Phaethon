using System.Data.Entity.Infrastructure;
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
    [RoutePrefix("ProductGroup")]
    public class ProductGroupController: ApiController
    {
        private readonly IProductGroupDM _productGroupManagement = null;

        public ProductGroupController()
        {
            _productGroupManagement = new ProductGroupDM();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            try
            {
                ProductGroup productGroup = JsonConvert.DeserializeObject<ProductGroup>(requestContent);
                if (productGroup == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                _productGroupManagement.Create(productGroup);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("GetProductGroups")]
        [HttpGet]
        public HttpResponseMessage GetProductGroups()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _productGroupManagement.GetProductGroups());
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}