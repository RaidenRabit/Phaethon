﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExternalApi.Controllers
{
    [RoutePrefix("TaxGroup")]
    public class TaxGroupController : ApiController
    {
        private readonly HttpClient _client;

        public TaxGroupController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/TaxGroup/");
        }

        [Route("GetTaxGroups")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTaxGroups()
        {
            return await _client.GetAsync("GetTaxGroups");
        }
    }
}