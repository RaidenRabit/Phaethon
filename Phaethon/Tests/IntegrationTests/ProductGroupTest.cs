using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Model;
using InternalApi.DataAccess;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class ProductGroupTest: InternalTestFakeServerBase
    {
        #region Create
        [Test]
        public async Task Create_NewProductGroupObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            ProductGroup productGroup = InvoiceTest.GetElementSeed().Item.Product.ProductGroup;
            using (var db = new DatabaseContext())
            {
                db.ProductGroups.Attach(productGroup);
                db.Items.RemoveRange(db.Items.Where(x => x.Product.ProductGroup_ID == productGroup.ID));
                db.Products.RemoveRange(db.Products.Where(x => x.ProductGroup_ID == productGroup.ID));
                db.ProductGroups.Remove(productGroup);
                db.SaveChanges();
            }

            //Act
            var response = await _client.PostAsJsonAsync("ProductGroup/Create", productGroup);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
        }

        [Test]
        public async Task Create_ExistingProductGroupObject_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            ProductGroup productGroup = InvoiceTest.GetElementSeed().Item.Product.ProductGroup;

            //Act
            var response = await _client.PostAsJsonAsync("ProductGroup/Create", productGroup);
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

            //Act
            var response = await _client.PostAsJsonAsync("ProductGroup/Create", productGroup);
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
            InvoiceTest.GetElementSeed();

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
