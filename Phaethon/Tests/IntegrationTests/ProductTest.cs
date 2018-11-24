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
            Element element = await new ElementTest().GetElement();
            if (element == null)
            {
                Invoice invoice = InvoiceTest.GetInvoiceSeed();
                string json = JsonConvert.SerializeObject(invoice);
                var content = new StringContent(json);
                await _client.PostAsync("Invoice/CreateOrUpdate", content);
                element = await new ElementTest().GetElement();
            }
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = element.Item.Product.Barcode.ToString();

            //Act
            var response = await _client.GetAsync("Product/GetProduct?" + parameters);
            Product product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
            
            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, product.ID == element.Item.Product.ID &&
                                  product.Barcode.Equals(element.Item.Product.Barcode) &&
                                  product.Name.Equals(element.Item.Product.Name));//check if object received is the same
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
