using System.Threading.Tasks;
using System.Web;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class ProductTest: InternalTestFakeServerBase
    {
        #region GetProduct
        [Test]
        public async Task GetProduct_CorrectBarcode_IsSuccessStatusCodeAndSameObjectReturned()
        {
            //Setup
            Product testProduct = InvoiceTest.GetElementSeed().Item.Product;
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = testProduct.Barcode.ToString();

            //Act
            var response = await _client.GetAsync("Product/GetProduct?" + parameters);
            Product product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
            
            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, product.ID == testProduct.ID &&
                                  product.Barcode.Equals(testProduct.Barcode) &&
                                  product.Name.Equals(testProduct.Name));
        }

        [Test]
        public async Task GetProduct_WrongBarcode_IsSuccessStatusCodeAndNullObjectReturned()
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
