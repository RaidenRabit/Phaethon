using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Core.Model;
using InternalApi.DataAccess;
using System.Linq;

namespace Core.Decorators
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                base.OnActionExecuting(actionContext);
                string requestUrl = actionContext.Request.RequestUri.ToString();

                if (requestUrl.Contains("Login/CreateOrUpdate")) //if you're just trying to register, allow for it to go on
                    return;
                if (requestUrl.Contains("Login/Login")) //if you're just trying to login, allow for it to go on
                    return;

                var userToken = actionContext.Request.Headers.GetValues("UserToken");
                
            }
            catch (Exception)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }

        private bool DecryptToken(string token)
        {

        }

        private Login GetLogin(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Login.SingleOrDefault(x => x.ID == id);
            }
        }
        
    }
}
