using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using NUnit.Framework;
using System.Web;
using Newtonsoft.Json;

namespace Tests.IntegrationTests
{
    public class ElementTest: IntegrationTestBase
    {
        #region GetInvoiceElements
        [Test]
        public async Task GetInvoiceElements_CorrectInvoiceId_SuccessStatusCodeAndElementsReturned()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            Invoice invoice = element.Invoice;
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = invoice.ID.ToString();

            //Act
            var response = await _internalClient.GetAsync("Element/GetInvoiceElements?" + parameters);
            List<Element> invoiceElements = JsonConvert.DeserializeObject<List<Element>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.AreNotEqual(0, invoiceElements.Count, "There were elements in invoice");
        }

        [Test]
        public async Task GetInvoiceElements_WrongInvoiceId_SuccessStatusCodeAndElementsNotReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = 0.ToString();

            //Act
            var response = await _internalClient.GetAsync("Element/GetInvoiceElements?" + parameters);
            List<Element> invoiceElements = JsonConvert.DeserializeObject<List<Element>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.AreEqual(0, invoiceElements.Count, "There were no elements in invoice");
        }
        #endregion
    }
}