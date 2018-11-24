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
    [RoutePrefix("ProductGroup")]
    public class ProductGroupController: ApiController
    {
        private readonly IProductGroupManagement _productGroupManagement = null;

        public ProductGroupController()
        {
            _productGroupManagement = new ProductGroupManagement();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            ProductGroup productGroup = JsonConvert.DeserializeObject<ProductGroup>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _productGroupManagement.Create(productGroup));
        }

        [Route("GetProductGroups")]
        [HttpGet]
        public HttpResponseMessage GetProductGroups()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _productGroupManagement.GetProductGroups());
        }
    }
}