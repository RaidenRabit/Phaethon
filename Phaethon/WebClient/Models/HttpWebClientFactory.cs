using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace WebClient.Models
{
    public class HttpWebClientFactory
    {
        private HttpClient _client;
        private HttpClient GenerateClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if(HttpContext.Current.Session?["userToken"] != null)
                client.DefaultRequestHeaders.Add("UserToken", HttpContext.Current.Session["userToken"].ToString());
            return client;
        }
        
        public HttpClient GetClient()
        {
            return _client ?? (_client = GenerateClient());
        }

        public void SetBaseAddress(string address)
        {
            GetClient();
            _client.BaseAddress = new Uri(address);
        }
    }
}