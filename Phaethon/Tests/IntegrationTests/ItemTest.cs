using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Model;
using Newtonsoft.Json;

namespace Tests.IntegrationTests
{
    public class ItemTest: InternalTestFakeServerBase
    {
        private bool AreItemsEqual(Item firstItem, Item secondItem)
        {
            return firstItem.ID == secondItem.ID &&
                   firstItem.SerNumber.Equals(secondItem.SerNumber) &&
                   firstItem.IncomingPrice == secondItem.IncomingPrice &&
                   firstItem.OutgoingPrice == secondItem.OutgoingPrice &&
                   firstItem.IncomingTaxGroup_ID == secondItem.IncomingTaxGroup_ID &&
                   firstItem.OutgoingTaxGroup_ID == secondItem.OutgoingTaxGroup_ID &&
                   firstItem.Product.ID == secondItem.Product.ID &&
                   firstItem.Product.Barcode == secondItem.Product.Barcode &&
                   firstItem.Product.Name.Equals(secondItem.Product.Name) &&
                   firstItem.Product.ProductGroup_ID == secondItem.Product.ProductGroup_ID;
        }

        #region GetItem
        [Test]
        public async Task GetItem_CorrectItemId_IsSuccessStatusCodeAndItemReturned()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = element.Item.ID.ToString();

            //Act
            var response = await _client.GetAsync("Item/GetItem?" + parameters);
            Item item = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, AreItemsEqual(element.Item, item));//check if object received is the same
        }

        [Test]
        public async Task GetItem_WrongItemId_IsSuccessStatusCodeAndNullReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = 0.ToString();

            //Act
            var response = await _client.GetAsync("Item/GetItem?" + parameters);
            Item dbItem = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(null, dbItem);//check if object received is the same
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

            //Act
            var response = await _client.GetAsync("Item/GetItems?" + parameters);
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(0, items.Count);
        }
        #endregion
    }
}
