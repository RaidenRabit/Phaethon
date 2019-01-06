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

        /// <summary>
        /// Creates or Updates an account. Distinction based on assigned object ID.
        /// ID = 0 -> new account
        /// ID != 0 -> update account
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Invalid posted object</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("CreateOrUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateOrUpdate()
        {
            var requestContent = await Request.Content.ReadAsStringAsync();
            Login login = JsonConvert.DeserializeObject<Login>(requestContent);
            return Request.CreateResponse(HttpStatusCode.OK, _loginManagement.CreateOrUpdate(login));
        }

        /// <summary>
        /// Delete an account
        /// </summary>
        /// <response code="200"></response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> Delete([FromBody]int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _loginManagement.Delete(id));
        }

        /// <summary>
        /// Get account by ID
        /// </summary>
        /// <returns>Account, inside response's body</returns>
        /// <response code="200"></response>
        /// <response code="400">No Account with such ID</response>
        /// <response code="403">Missing/Invalid UserToken</response>    
        [Route("GetLogin")]
        [HttpGet]
        public HttpResponseMessage GetLogin(int id)
        {
            Login login = _loginManagement.GetLogin(id);
            if(login != null)
                return Request.CreateResponse(HttpStatusCode.OK, login);
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
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
            login = _loginManagement.GetLogin(loginID);
            if (loginID != 0)
            {
                string userToken = "UserToken: " + loginID + UtilityMethods.ComputeSha256Hash(UtilityMethods.Encipher(login.Username + login.Salt, loginID));
                return Request.CreateResponse(HttpStatusCode.OK, userToken);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorrect login credentials");
            }
        }

    }
}
