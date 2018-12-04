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
        public async Task GetItem_CorrectInvoiceId_IsSuccessStatusCodeAndItemReturned()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            Item item = element.Item;
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = item.ID.ToString();

            //Act
            var response = await _client.GetAsync("Item/GetItem?" + parameters);
            Item dbItem = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, item.ID == dbItem.ID &&
                                  item.SerNumber.Equals(dbItem.SerNumber) &&
                                  item.IncomingPrice == dbItem.IncomingPrice &&
                                  item.OutgoingPrice == dbItem.OutgoingPrice &&
                                  item.IncomingTaxGroup_ID == dbItem.IncomingTaxGroup_ID &&
                                  item.OutgoingTaxGroup_ID == dbItem.OutgoingTaxGroup_ID &&
                                  item.Product.ID == dbItem.Product_ID &&
                                  item.Product.Barcode == dbItem.Product.Barcode &&
                                  item.Product.Name.Equals(dbItem.Product.Name) &&
                                  item.Product.ProductGroup_ID == dbItem.Product.ProductGroup_ID);//check if object received is the same
        }

        [Test]
        public async Task GetItem_WrongInvoiceId_IsSuccessStatusCodeAndNullReturned()
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
            parameters["barcode"] = "";

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
