using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebClient.Controllers
{
    public class JobController : Controller
    {
        public JobController()
        {

            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64010/Job/");
        }

        // GET: Job
        public ActionResult Index()
        {
            return View(_client.getAsync("/ReadAll"));
        }
    }
}