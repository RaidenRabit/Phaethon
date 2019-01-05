using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Core;
using Core.Model;
using InternalApi.DataAccess;
using Newtonsoft.Json;

namespace Tests.IntegrationTests
{
    public class ItemTest: IntegrationTestBase
    {
        private bool AreItemsEqual(Item firstItem, Item secondItem)
        {
            return firstItem.ID == secondItem.ID &&
                   firstItem.SerNumber.Equals(secondItem.SerNumber) &&
                   firstItem.IncomingTaxGroup.ID == secondItem.IncomingTaxGroup.ID;
        }

        #region CreateOrUpdate
        [Test]
        public async Task CreateOrUpdate_NewItemObject_SuccessStatusCode()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            Item item = element.Item;
            item.IncomingTaxGroup = null;
            item.OutgoingTaxGroup = null;
            using (var db = new DatabaseContext())
            {
                db.Items.Remove(db.Items.SingleOrDefault(x => x.ID == item.ID));
                db.SaveChanges();
            }

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/CreateOrUpdate", item);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
        }

        [Test]
        public async Task CreateOrUpdate_ExistingItemObject_SuccessStatusCode()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            Item item = element.Item;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/CreateOrUpdate", item);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
        }

        [Test]
        public async Task CreateOrUpdate_ItemObjectNull_BadRequestStatusCode()
        {
            //Setup
            Item item = null;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/CreateOrUpdate", item);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Server responded with bad request code");//check if internal server error
        }
        #endregion

        #region GetItem
        [Test]
        public async Task GetItem_CorrectItemId_SuccessStatusCodeAndItemReturned()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = element.Item.ID.ToString();

            //Act
            var response = await _internalClient.GetAsync("Item/GetItem?" + parameters);
            Item item = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsTrue(AreItemsEqual(element.Item, item), "Items are equal");//check if object received is the same
        }

        [Test]
        public async Task GetItem_WrongItemId_BadRequestStatusCode()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = 0.ToString();

            //Act
            var response = await _internalClient.GetAsync("Item/GetItem?" + parameters);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Server responded with bad request code");//check if internal server error
        }
        #endregion

        #region GetItems
        [Test]
        public async Task GetItems_MethodCalled_SuccessStatusCodeAndItemsReturned()
        {
            //Setup
            InvoiceTest.GetElementSeed();
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["serialNumber"] = "";
            parameters["productName"] = "";
            parameters["barcode"] = 0.ToString();
            parameters["showAll"] = true.ToString();

            //Act
            var response = await _internalClient.GetAsync("Item/GetItems?" + parameters);
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.AreNotEqual(0, items.Count, "Gets items");
        }
        #endregion

        #region Delete
        [Test]
        public async Task Delete_CorrectID_SuccessStatusCode()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/Delete", element.Item.ID);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
        }

        [Test]
        public async Task Delete_WrongID_BadRequestStatusCode()
        {
            //Setup
            int id = 0;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/Delete", id);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Server responded with bad request code");//check if internal server error
        }
        #endregion
    }
}
