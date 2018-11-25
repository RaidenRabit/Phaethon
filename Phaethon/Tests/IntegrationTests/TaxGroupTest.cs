using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class TaxGroupTest : InternalTestFakeServerBase
    {

        internal static TaxGroup GetTaxGroupSeed()
        {
            TaxGroup taxGroup = new TaxGroup
            {
                Name = "Test",
                Tax = 99
            };
            return taxGroup;
        }

        #region Create
        [Test]
        public async Task Create_NewTaxGroupObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            TaxGroup taxGroup = GetTaxGroupSeed();

            //Act
            var response = await _client.PostAsJsonAsync("TaxGroup/Create", taxGroup);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
        }

        [Test]
        public async Task Create_ExistingTaxGroupObject_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            TaxGroup taxGroup = GetTaxGroupSeed();
            string json = JsonConvert.SerializeObject(taxGroup);
            var content = new StringContent(json);
            await _client.PostAsync("TaxGroup/Create", content);

            //Act
            var response = await _client.PostAsync("TaxGroup/Create", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsFalse(deserializedResponse);
        }

        [Test]
        public async Task Create_TaxGroupObjectNull_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            TaxGroup taxGroup = null;
            string json = JsonConvert.SerializeObject(taxGroup);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("TaxGroup/Create", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsFalse(deserializedResponse);
        }
        #endregion

        #region GetTaxGroups
        [Test]
        public async Task GetTaxGroups_MethodCalled_IsSuccessStatusCodeAndTaxGroupsReturned()
        {
            //Setup
            TaxGroup taxGroup = GetTaxGroupSeed();
            string json = JsonConvert.SerializeObject(taxGroup);
            var content = new StringContent(json);
            await _client.PostAsync("TaxGroup/Create", content);

            //Act
            var response = await _client.GetAsync("TaxGroup/GetTaxGroups");
            List<TaxGroup> taxGroups = JsonConvert.DeserializeObject<List<TaxGroup>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(0, taxGroups.Count);
        }
        #endregion
    }
}
