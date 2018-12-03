using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;
using Newtonsoft.Json;

namespace InternalApi.Controllers
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        private readonly IUserManagement _userManagement;

        internal UserController()
        {
            _userManagement = new UserManagement();
        }

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _userManagement.CreateOrUpdate(user));
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteUser()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            int id = JsonConvert.DeserializeObject<int>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _userManagement.DeleteUser(id));
        }

        [Route("GetUser")]
        [HttpGet]
        public HttpResponseMessage GetUser(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userManagement.GetUser(id));
        }

        [Route("Login")]
        [HttpPost]
        public async Task<HttpResponseMessage> Login()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _userManagement.Login(user));
        }

    }
}
