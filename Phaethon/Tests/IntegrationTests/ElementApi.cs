using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using NUnit.Framework;
using System.Web;
using Newtonsoft.Json;

namespace Tests.IntegrationTests
{
    public class ElementApi: InternalApiFakeServer
    {
        #region GetInvoiceElements
        [Test]
        public async Task GetInvoiceElements_CorrectInvoiceId_IsSuccessStatusCodeAndElementsReturned()
        {
            //Setup
            Element element = InvoiceApi.GetElementSeed();
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

        [Test]
        public async Task GetInvoiceElements_WrongInvoiceId_IsSuccessStatusCodeAndElementsNotReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = 0.ToString();

            //Act
            var response = await _client.GetAsync("Element/GetInvoiceElements?" + parameters);
            List<Element> invoiceElements = JsonConvert.DeserializeObject<List<Element>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(0, invoiceElements.Count);
        }
        #endregion
    }
}