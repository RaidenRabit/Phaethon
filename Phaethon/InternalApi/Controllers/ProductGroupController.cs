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
    [EnableCors(origins: "http://localhost:49873", headers: "*", methods: "*")]
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