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
    public class ProductGroupTest
    {
        //InternalTestFakeServerBase
        private static HttpClient _client;

        public ProductGroupTest()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/");
        }

        internal static ProductGroup GetProductGroupSeed()
        {
            ProductGroup productGroup = new ProductGroup
            {
                Margin = 99,
                Name = "Test"
            };
            return productGroup;
        }

        #region Create
        [Test]
        public async Task Create_NewProductGroupObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            ProductGroup productGroup = GetProductGroupSeed();
            var response = await _client.GetAsync("ProductGroup/GetProductGroups");
            List<ProductGroup> productGroups = JsonConvert.DeserializeObject<List<ProductGroup>>(await response.Content.ReadAsStringAsync());
            while (productGroups.Exists(x => x.Name == productGroup.Name))
            {
                productGroup.Name = productGroup.Name + "X";
            }
            string json = JsonConvert.SerializeObject(productGroup);
            var content = new StringContent(json);

            //Act
            response = await _client.PostAsync("ProductGroup/Create", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
        }

        [Test]
        public async Task Create_ExistingProductGroupObject_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            ProductGroup productGroup = GetProductGroupSeed();
            string json = JsonConvert.SerializeObject(productGroup);
            var content = new StringContent(json);
            await _client.PostAsync("ProductGroup/Create", content);

            //Act
            var response = await _client.PostAsync("ProductGroup/Create", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsFalse(deserializedResponse);
        }

        [Test]
        public async Task Create_ProductGroupObjectNull_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            ProductGroup productGroup = null;
            string json = JsonConvert.SerializeObject(productGroup);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("ProductGroup/Create", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsFalse(deserializedResponse);
        }
        #endregion

        #region GetProductGroups
        [Test]
        public async Task GetProductGroups_MethodCalled_IsSuccessStatusCodeAndProductGroupsReturned()
        {
            //Setup
            ProductGroup productGroup = GetProductGroupSeed();
            string json = JsonConvert.SerializeObject(productGroup);
            var content = new StringContent(json);
            await _client.PostAsync("ProductGroup/Create", content);

            //Act
            var response = await _client.GetAsync("ProductGroup/GetProductGroups");
            List<ProductGroup> productGroups = JsonConvert.DeserializeObject<List<ProductGroup>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(0, productGroups.Count);
        }
        #endregion
    }
}
