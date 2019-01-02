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
    public class TaxGroupTest: IntegrationTestBase
    {
        #region Create
        [Test]
        public async Task Create_NewTaxGroupObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            TaxGroup taxGroup = InvoiceTest.GetElementSeed().Item.IncomingTaxGroup;
            using (var db = new DatabaseContext())
            {
                db.TaxGroups.Attach(taxGroup);
                db.Items.RemoveRange(db.Items.Where(x => x.IncomingTaxGroup_ID == taxGroup.ID || x.OutgoingTaxGroup_ID == taxGroup.ID));
                db.TaxGroups.Remove(taxGroup);
                db.SaveChanges();
            }

            //Act
            var response = await _internalClient.PostAsJsonAsync("TaxGroup/Create", taxGroup);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsTrue(deserializedResponse, "Tax group created");
        }

        [Test]
        public async Task Create_ExistingTaxGroupObject_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            TaxGroup taxGroup = InvoiceTest.GetElementSeed().Item.IncomingTaxGroup;

            //Act
            var response = await _internalClient.PostAsJsonAsync("TaxGroup/Create", taxGroup);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsFalse(deserializedResponse, "tax group not created");
        }

        [Test]
        public async Task Create_TaxGroupObjectNull_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            TaxGroup taxGroup = null;

            //Act
            var response = await _internalClient.PostAsJsonAsync("TaxGroup/Create", taxGroup);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsFalse(deserializedResponse, "Tax group not created");
        }
        #endregion

        #region GetTaxGroups
        [Test]
        public async Task GetTaxGroups_MethodCalled_IsSuccessStatusCodeAndTaxGroupsReturned()
        {
            //Setup
            InvoiceTest.GetElementSeed();

            //Act
            var response = await _internalClient.GetAsync("TaxGroup/GetTaxGroups");
            List<TaxGroup> taxGroups = JsonConvert.DeserializeObject<List<TaxGroup>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.AreNotEqual(0, taxGroups.Count, "Tax groups received");
        }
        #endregion
    }
}
