using System.Data.Entity.Infrastructure;
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
    [RoutePrefix("TaxGroup")]
    public class TaxGroupController : ApiController
    {
        private readonly ITaxGroupDM _taxGroupManagement = null;

        public TaxGroupController()
        {
            _taxGroupManagement = new TaxGroupDM();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            try
            {
                TaxGroup taxGroup = JsonConvert.DeserializeObject<TaxGroup>(requestContent);
                if (taxGroup == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                _taxGroupManagement.Create(taxGroup);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("GetTaxGroups")]
        [HttpGet]
        public HttpResponseMessage GetTaxGroups()
        {
            try { 
                return Request.CreateResponse(HttpStatusCode.OK, _taxGroupManagement.GetTaxGroups());
            }
            catch (DbUpdateException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
