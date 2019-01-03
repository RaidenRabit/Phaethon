using System.Web.Mvc;
using WebClient.App_Start;

namespace WebClient
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new RequireHttpsAttribute());
            filters.Add(new RequireHstsAttribute(31536000) { IncludeSubDomains = true, Preload = true });
            filters.Add(new HandleErrorAttribute());
        }
    }
}
