using System.Threading.Tasks;
using System.Web;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class ProductTest: IntergrationTestBase
    {
        private bool AreProductsEqual(Product firstProduct, Product secondProduct)
        {
            return firstProduct.ID == secondProduct.ID &&
                   firstProduct.Barcode.Equals(secondProduct.Barcode) &&
                   firstProduct.Name.Equals(secondProduct.Name);
        }

        #region GetProduct
        [Test]
        public async Task GetProduct_CorrectBarcode_IsSuccessStatusCodeAndSameObjectReturned()
        {
            //Setup
            Product testProduct = InvoiceTest.GetElementSeed().Item.Product;
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = testProduct.Barcode.ToString();

            //Act
            var response = await _internalClient.GetAsync("Product/GetProduct?" + parameters);
            Product product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
            
            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, AreProductsEqual(product, product));
        }

        [Test]
        public async Task GetProduct_WrongBarcode_IsSuccessStatusCodeAndNullObjectReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["barcode"] = "-1794";

            //Act
            var response = await _internalClient.GetAsync("Product/GetProduct?" + parameters);
            Product product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(null, product);
        }
        #endregion
    }
}
