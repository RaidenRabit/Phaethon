using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Model;
using InternalApi.DataAccess;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class ProductGroupTest: IntegrationTestBase
    {
        #region Create
        [Test]
        public async Task Create_NewProductGroupObject_SuccessStatusCode()
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
            var response = await _internalClient.PostAsJsonAsync("ProductGroup/Create", productGroup);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
        }

        [Test]
        public async Task Create_ExistingProductGroupObject_BadRequestStatusCode()
        {
            //Setup
            ProductGroup productGroup = InvoiceTest.GetElementSeed().Item.Product.ProductGroup;

            //Act
            var response = await _internalClient.PostAsJsonAsync("ProductGroup/Create", productGroup);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Server responded with bad request code");//check if internal server error
        }

        [Test]
        public async Task Create_ProductGroupObjectNull_BadRequestStatusCode()
        {
            //Setup
            ProductGroup productGroup = null;

            //Act
            var response = await _internalClient.PostAsJsonAsync("ProductGroup/Create", productGroup);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Server responded with bad request code");//check if internal server error
        }
        #endregion

        #region GetProductGroups
        [Test]
        public async Task GetProductGroups_MethodCalled_SuccessStatusCodeAndProductGroupsReturned()
        {
            //Setup
            InvoiceTest.GetElementSeed();

            //Act
            var response = await _internalClient.GetAsync("ProductGroup/GetProductGroups");
            List<ProductGroup> productGroups = JsonConvert.DeserializeObject<List<ProductGroup>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.AreNotEqual(0, productGroups.Count, "Product groups received");
        }
        #endregion
    }
}
