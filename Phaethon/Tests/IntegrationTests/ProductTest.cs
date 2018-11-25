using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
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

        internal static Product GetProductSeed()
        {
            ProductGroup productGroup = ProductGroupTest.GetProductGroupSeed();
            if (productGroup.ID == 0)
            {
                using (var db = new DatabaseContext())
                {
                    ProductGroupDa productGroupDa = new ProductGroupDa();
                    productGroupDa.Create(db, productGroup);
                }
            }

            Product product = new Product
            {
                Barcode = 0,
                Name = "Test",
                ProductGroup_ID = productGroup.ID
            };

            using (var db = new DatabaseContext())
            {
                ProductDa productDa = new ProductDa();
                Product oldProduct = productDa.GetProducts(db).SingleOrDefault(x => x.Barcode == product.Barcode || x.Name.Equals(product.Name));
                if (oldProduct != null)
                {
                    product = oldProduct;
                }
            }

            return product;
        }

        #region GetProduct
        [Test]
        public async Task GetProduct_CorrectID_IsSuccessStatusCodeAndSameObjectReturned()
        {
            //Setup
            Product testProduct = GetProductSeed();
            if (testProduct.ID == 0)
            {
                using (var db = new DatabaseContext())
                {
                    ProductDa productDa = new ProductDa();
                    productDa.CreateOrUpdate(db, testProduct);
                }
            }

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
