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

        internal static List<Element> GetElementsSeed()
        {
            TaxGroup taxGroup = TaxGroupTest.GetTaxGroupSeed();
            if (taxGroup.ID == 0)
            {
                using (var db = new DatabaseContext())
                {
                    TaxGroupDa taxGroupDa = new TaxGroupDa();
                    taxGroupDa.Create(db, taxGroup);
                }
            }

            Product product = ProductTest.GetProductSeed();

            Item item = new Item
            {
                Discount = 99,
                IncomingPrice = 100,
                IncomingTaxGroup_ID = taxGroup.ID,
                Product = product,
                SerNumber = "1233"
            };

            Element element = new Element
            {
                Item = item
            };

            List<Element> elements = new List<Element>
            {
                element
            };

            return elements;
        }

        #region GetInvoiceElements
        [Test]
        public async Task GetInvoiceElements_MethodCalled_IsSuccessStatusCodeAndElementsReturned()
        {
            //Setup
            Invoice invoice = InvoiceTest.GetInvoiceSeed();
            if (invoice.ID == 0 || invoice.Elements == null)
            {
                invoice.Elements = GetElementsSeed();
                invoice = InvoiceTest.CreateInvoice(invoice);
            }
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