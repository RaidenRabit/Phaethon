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
    public class TaxGroupTest
    {
        //InternalTestFakeServerBase
        private static HttpClient _client;

        public TaxGroupTest()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/");
        }

        private static TaxGroup GetTaxGroupSeed()
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
            string json = JsonConvert.SerializeObject(taxGroup);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("TaxGroup/Create", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
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

            //Act
            var response = await _client.GetAsync("TaxGroup/GetTaxGroups");
            List<TaxGroup> taxGroups = JsonConvert.DeserializeObject<List<TaxGroup>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(null, taxGroups);
        }
        #endregion
    }
}
