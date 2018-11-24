using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class ProductTest
    {
        //InternalTestFakeServerBase
        private static HttpClient _client;

        public ProductTest()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/");
        }

        #region GetProduct
        [Test]
        public async Task GetProduct_CorrectID_IsSuccessStatusCodeAndSameObjectReturned()
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
            response = await _client.GetAsync("Element/GetInvoiceElements?" + parameters);
            List<Element> elements = JsonConvert.DeserializeObject<List<Element>>(await response.Content.ReadAsStringAsync());
            parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = elements[0].Item.Product.Barcode.ToString();

            //Act
            response = await _client.GetAsync("Product/GetProduct?" + parameters);
            Product product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
            
            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, product.ID == elements[0].Item.Product.ID &&
                                  product.Barcode.Equals(elements[0].Item.Product.Barcode) &&
                                  product.Name.Equals(elements[0].Item.Product.Name));//check if object received is the same
        }

        [Test]
        public async Task GetProduct_WrongID_IsSuccessStatusCodeAndNullObjectReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = "-1794";

            //Act
            var response = await _client.GetAsync("Product/GetProduct?" + parameters);
            Product product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(null, product);
        }
        #endregion
    }
}
