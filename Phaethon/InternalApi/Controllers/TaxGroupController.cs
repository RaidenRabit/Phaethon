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
    [RoutePrefix("TaxGroup")]
    public class TaxGroupController : ApiController
    {
        private readonly ITaxGroupManagement _taxGroupManagement = null;

        public TaxGroupController()
        {
            _taxGroupManagement = new TaxGroupManagement();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            TaxGroup taxGroup = JsonConvert.DeserializeObject<TaxGroup>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _taxGroupManagement.Create(taxGroup));
        }

        [Route("GetTaxGroups")]
        [HttpGet]
        public HttpResponseMessage GetTaxGroups()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _taxGroupManagement.GetTaxGroups());
        }
    }
}
