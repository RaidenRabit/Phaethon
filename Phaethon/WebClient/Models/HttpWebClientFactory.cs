using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.ApplicationInsights;

namespace WebClient.Models
{
    public class HttpWebClientFactory
    {
        private HttpClient _client;
        private string _userToken;
         private HttpClient GenerateClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if(HttpContext.Current.Session?["userToken"] != null)
                _userToken = HttpContext.Current.Session["userToken"].ToString();
            client.DefaultRequestHeaders.Add("UserToken", _userToken);
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

        public void SetUserToken(string userToken)
        {
            _userToken = userToken;
        }
    }
}