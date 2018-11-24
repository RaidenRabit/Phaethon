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

        internal async Task<List<Element>> GetElementsSeed()
        {
            #region TaxGroup
            TaxGroup taxGroup = TaxGroupTest.GetTaxGroupSeed();
            string json = JsonConvert.SerializeObject(taxGroup);
            var content = new StringContent(json);
            await _client.PostAsync("TaxGroup/Create", content);
            var response = await _client.GetAsync("TaxGroup/GetTaxGroups");
            List<TaxGroup> taxGroups = JsonConvert.DeserializeObject<List<TaxGroup>>(await response.Content.ReadAsStringAsync());
            int taxGroupId = taxGroups[0].ID;
            #endregion

            #region ProductGroup
            ProductGroup productGroup = ProductGroupTest.GetProductGroupSeed();
            json = JsonConvert.SerializeObject(productGroup);
            content = new StringContent(json);
            await _client.PostAsync("ProductGroup/Create", content);
            response = await _client.GetAsync("ProductGroup/GetProductGroups");
            List<ProductGroup> productGroups = JsonConvert.DeserializeObject<List<ProductGroup>>(await response.Content.ReadAsStringAsync());
            int productGroupId = productGroups[0].ID;
            #endregion

            #region Product
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = 0.ToString();
            response = await _client.GetAsync("Product/GetProduct?" + parameters);
            Product product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
            if (product == null)
            {
                product = new Product
                {
                    ID = 0,
                    Barcode = 1, //unique
                    Name = "Tests", //unique
                    ProductGroup_ID = productGroupId
                };
            }
            #endregion

            Item item = new Item
            {
                Discount = 99,
                IncomingPrice = 100,
                IncomingTaxGroup_ID = taxGroupId,
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

        internal async Task<Element> GetElement()
        {
            //Gets all invoices
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
            //Gets elements in invoice
            foreach (var invoice in invoices)
            {
                parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["id"] = invoice.ID.ToString();
                response = await _client.GetAsync("Element/GetInvoiceElements?" + parameters);
                List<Element> elements = JsonConvert.DeserializeObject<List<Element>>(await response.Content.ReadAsStringAsync());
                foreach (var element in elements)
                {
                    element.Invoice_ID = invoice.ID;
                    return element;
                }
            }
            return null;
        }

        #region GetInvoiceElements
        [Test]
        public async Task GetInvoiceElements_MethodCalled_IsSuccessStatusCodeAndElementsReturned()
        {
            //Setup
            Element element = await GetElement();
            if (element == null)
            {
                Invoice invoice = InvoiceTest.GetInvoiceSeed();
                string json = JsonConvert.SerializeObject(invoice);
                var content = new StringContent(json);
                await _client.PostAsync("Invoice/CreateOrUpdate", content);
                element = await new ElementTest().GetElement();
            }
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = element.Invoice_ID.ToString();

            //Act
            var response = await _client.GetAsync("Element/GetInvoiceElements?" + parameters);
            List<Element> elements = JsonConvert.DeserializeObject<List<Element>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(0, elements.Count);
        }
        #endregion
    }
}