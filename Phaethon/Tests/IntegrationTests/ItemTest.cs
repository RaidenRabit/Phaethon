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
            //Assert.AreEqual(true, element.Item.ID == item.ID &&
            //                      element.Item.SerNumber.Equals(item.SerNumber) &&
            //                      element.Item.IncomingPrice == item.IncomingPrice &&
            //                      element.Item.OutgoingPrice == item.OutgoingPrice &&
            //                      element.Item.IncomingTaxGroup_ID == item.IncomingTaxGroup_ID &&
            //                      element.Item.OutgoingTaxGroup_ID == item.OutgoingTaxGroup_ID &&
            //                      element.Item.Product.ID == item.Product_ID &&
            //                      element.Item.Product.Barcode == item.Product.Barcode &&
            //                      element.Item.Product.Name.Equals(item.Product.Name) &&
            //                      element.Item.Product.ProductGroup_ID == item.Product.ProductGroup_ID);//check if object received is the same
            Assert.AreEqual(element.Item.ID, item.ID);
            Assert.AreEqual(element.Item.SerNumber, item.SerNumber);
            Assert.AreEqual(element.Item.IncomingPrice, item.IncomingPrice);
            Assert.AreEqual(element.Item.OutgoingPrice, item.OutgoingPrice);
            Assert.AreEqual(element.Item.IncomingTaxGroup_ID, item.IncomingTaxGroup_ID);
            Assert.AreEqual(element.Item.OutgoingTaxGroup_ID, item.OutgoingTaxGroup_ID);
            Assert.AreEqual(element.Item.Product.ID, item.Product.ID);
            Assert.AreEqual(element.Item.Product.Barcode, item.Product.Barcode);
            Assert.AreEqual(element.Item.Product.Name, item.Product.Name);
            Assert.AreEqual(element.Item.Product.ProductGroup_ID, item.Product.ProductGroup_ID);
            //
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
