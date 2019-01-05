using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Model;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;
using Newtonsoft.Json;
using Core.Decorators;

namespace InternalApi.Controllers
{
    [RoutePrefix("Login")]
    public class LoginController : ApiController
    {
        private readonly ILoginDM _loginManagement;

        internal LoginController()
        {
            _loginManagement = new LoginDM();
        }

        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            Login login = JsonConvert.DeserializeObject<Login>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _loginManagement.CreateOrUpdate(login));
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> Delete()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            int id = JsonConvert.DeserializeObject<int>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _loginManagement.Delete(id));
        }

        [Route("GetLogin")]
        [HttpGet]
        public HttpResponseMessage GetLogin(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _loginManagement.GetLogin(id));
        }

        /// <summary>
        /// Logs in.
        /// </summary>
        /// <returns>A userToken, required to be placed in every further API request</returns>
        /// <response code="200">Returns a userToken</response>
        /// <response code="400">Incorrect login credentials</response>     
        [Route("Login")]
        [HttpPost]
        public async Task<HttpResponseMessage> Login([FromBody]Login login)
        {
            int loginID = _loginManagement.Login(login.Username, login.Password);
            if (loginID != 0)
            {
                string userToken = "UserToken: " + loginID + UtilityMethods.ComputeSha256Hash(UtilityMethods.Encipher(login.Username, loginID));
                return Request.CreateResponse(HttpStatusCode.OK, userToken);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorrect login credentials");
            }
        }

    }
}
