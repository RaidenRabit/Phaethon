using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Core.Decorators
{
    public class ExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
