using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Model;
using Core.Model.Filters;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;
using Newtonsoft.Json;

namespace InternalApi.Controllers
{
    [RoutePrefix("Customer")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerDM _customerDm;

        public CustomerController()
        {
            _customerDm = new CustomerDM();
        }
        
        [Route("ReadAll")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReadAll()
        {
            try
            {
                if (Request.Content == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, _customerDm.ReadAll(new CustomerQueryFilter()));
                }
                var json = await Request.Content.ReadAsStringAsync();
                CustomerQueryFilter customerQueryFilter = JsonConvert.DeserializeObject<CustomerQueryFilter>(json);
                return Request.CreateResponse(HttpStatusCode.OK, _customerDm.ReadAll(customerQueryFilter));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
