using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebClient.Models;

namespace WebClient.Controllers.Api
{
    [RoutePrefix("Api/Item")]
    public class ItemApiController : ApiController
    {
        private readonly HttpClient _client;

        public ItemApiController()
        {
            HttpWebClientFactory clientFactory = new HttpWebClientFactory();
            clientFactory.SetBaseAddress("http://localhost:64007/Item/");
            _client = clientFactory.GetClient();
        }

        [Route("GetItem")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetItem(int id)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = id.ToString();
            return await _client.GetAsync("GetItem?" + parameters);
        }

        [Route("GetItems")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetItems(string serialNumber, string productName, int barcode, bool showAll)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["serialNumber"] = serialNumber;
            parameters["productName"] = productName;
            parameters["barcode"] = barcode.ToString();
            parameters["showAll"] = showAll.ToString();
            return await _client.GetAsync("GetItems?" + parameters);
        }
    }
}
