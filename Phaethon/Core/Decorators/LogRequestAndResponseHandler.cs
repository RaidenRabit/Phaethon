using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Model;

namespace Core.Decorators
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Logs log = new Logs();

            // log request body
            if (request.Content != null)
            {
                log.RequestBody = await request.Content.ReadAsStringAsync();
            }

            // log request headers
            log.RequestHeaders = request.Headers.ToString();

            // log request URL
            log.RequestUrl = request.RequestUri.ToString();

            // log UserToken
            try
            {
                log.UserToken = request.Headers.GetValues("UserToken").FirstOrDefault();
            }
            catch (Exception)
            {
                log.UserToken = "";
            }
            
            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);
            
            if (result.Content != null)
            {
                // once response body is ready, log it
                log.ResponseBody = await result.Content.ReadAsStringAsync();
            }
            
            if (!request.RequestUri.ToString().Contains("swagger"))
            {
                log.TimeStamp = DateTime.Now;
                LogToDb(log);
            }
            
            return result;
        }

        private void LogToDb(Logs log)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    db.Logs.AddOrUpdate(log);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
