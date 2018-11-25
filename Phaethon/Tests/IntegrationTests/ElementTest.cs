using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Model;
using NUnit.Framework;
using System.Linq;
using System.Web;
using InternalApi.DataAccess;
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
            Element element = InvoiceTest.GetElementSeed();
            Invoice invoice = element.Invoice;
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = invoice.ID.ToString();

            //Act
            var response = await _client.GetAsync("Element/GetInvoiceElements?" + parameters);
            List<Element> invoiceElements = JsonConvert.DeserializeObject<List<Element>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(0, invoiceElements.Count);
        }
        #endregion
    }
}