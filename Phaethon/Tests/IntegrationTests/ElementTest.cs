using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Model;
using NUnit.Framework;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Tests.IntegrationTests
{
    public class ElementTest
    {
        //InternalTestFakeServerBase
        private static HttpClient _client;

        public ElementTest()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/");
        }

        #region GetInvoiceElements
        [Test]
        public async Task GetInvoiceElements_MethodCalled_IsSuccessStatusCodeAndElementsReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["numOfRecords"] = 1.ToString();
            parameters["selectedCompany"] = 0.ToString();
            parameters["name"] = "";
            parameters["selectedDate"] = 0.ToString();
            parameters["from"] = new DateTime(2000, 1, 11).ToString("dd/MM/yyyy");
            parameters["to"] = DateTime.Now.ToString("dd/MM/yyyy");
            parameters["docNumber"] = "";
            var response = await _client.GetAsync("Invoice/GetInvoices?" + parameters);
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(await response.Content.ReadAsStringAsync());
            parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = invoices[0].ID.ToString();

            //Act
            response = await _client.GetAsync("Element/GetInvoiceElements?" + parameters);
            List<Element> elements = JsonConvert.DeserializeObject<List<Element>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(null, elements);//Check if result returned
        }
        #endregion
    }
}