using NUnit.Framework;
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
        public async Task CreateOrUpdate_NewItemObject_IsSuccessStatusCodeAndResponseTrue()
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
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Api didn't encounter unexpected issue");
            Assert.IsTrue(deserializedResponse, "Api returned that operation has not succeeded");
        }

        [Test]
        public async Task CreateOrUpdate_ExistingItemObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            Item item = element.Item;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/CreateOrUpdate", item);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
        }

        [Test]
        public async Task CreateOrUpdate_ItemObjectNull_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            Item item = null;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/CreateOrUpdate", item);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsFalse(deserializedResponse);
        }
        #endregion

        #region GetItem
        [Test]
        public async Task GetItem_CorrectItemId_IsSuccessStatusCodeAndItemReturned()
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
        public async Task GetItem_WrongItemId_IsSuccessStatusCodeAndNullReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = 0.ToString();

            //Act
            var response = await _internalClient.GetAsync("Item/GetItem?" + parameters);
            Item dbItem = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsNull(dbItem, "Item was not received");//check if object received is the same
        }
        #endregion

        #region GetItems
        [Test]
        public async Task GetItems_MethodCalled_IsSuccessStatusCodeAndItemsReturned()
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
        public async Task Delete_CorrectID_IsSuccessStatusCodeAndItemDeleted()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            Item item = element.Item;
            int id = item.ID;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/Delete", id);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsTrue(deserializedResponse, "Item deleted");
        }

        [Test]
        public async Task Delete_WrongID_IsSuccessStatusCodeAndItemNotDeleted()
        {
            //Setup
            int id = 0;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Item/Delete", id);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsFalse(deserializedResponse, "Not deleted");
        }
        #endregion
    }
}
