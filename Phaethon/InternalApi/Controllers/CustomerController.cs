using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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


        /// <summary>
        /// Get all customers by filter
        /// </summary>
        /// <returns>A list of customers, inside the response's body</returns>
        /// <response code="200">Returns a list of customers</response>
        /// <response code="400">No customers meeting the queried criteria</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("ReadAll")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReadAll([FromBody]CustomerQueryFilter customerQueryFilter)
        {
            try
            {
                if (Request.Content == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, _customerDm.ReadAll(new CustomerQueryFilter()));
                }
                return Request.CreateResponse(HttpStatusCode.OK, _customerDm.ReadAll(customerQueryFilter));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
