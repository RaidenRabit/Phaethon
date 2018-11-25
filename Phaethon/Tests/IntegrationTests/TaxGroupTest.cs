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

        internal static TaxGroup GetTaxGroupSeed()
        {
            TaxGroup taxGroup = new TaxGroup
            {
                Name = "Test",
                Tax = 99
            };

            using (var db = new DatabaseContext())
            {
                TaxGroupDa taxGroupDa = new TaxGroupDa();
                TaxGroup oldTaxGroup = taxGroupDa.GetTaxGroups(db).SingleOrDefault(x => x.Name.Equals(taxGroup.Name));
                if (oldTaxGroup != null)
                {
                    taxGroup = oldTaxGroup;
                }
            }

            return taxGroup;
        }
        //needs cascade delete
        #region Create
        [Test]
        public async Task Create_NewTaxGroupObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            TaxGroup taxGroup = GetTaxGroupSeed();
            if (taxGroup.ID != 0)
            {
                using (var db = new DatabaseContext())
                {
                    TaxGroupDa taxGroupDa = new TaxGroupDa();
                    db.TaxGroups.Attach(taxGroup);
                    db.Items.RemoveRange(db.Items.Where(x => x.IncomingTaxGroup_ID == taxGroup.ID || x.OutgoingTaxGroup_ID == taxGroup.ID));
                    taxGroupDa.Delete(db, taxGroup);
                }
            }
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
        public async Task Create_ExistingTaxGroupObject_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            TaxGroup taxGroup = GetTaxGroupSeed();
            if (taxGroup.ID == 0)
            {
                try
                {
                    using (var db = new DatabaseContext())
                    {
                        TaxGroupDa taxGroupDa = new TaxGroupDa();
                        taxGroupDa.Create(db, taxGroup);
                    }
                }
                catch
                {
                    // ignored
                }
            }
            string json = JsonConvert.SerializeObject(taxGroup);
            var content = new StringContent(json);

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
            using (var db = new DatabaseContext())
            {
                try
                {
                    TaxGroupDa taxGroupDa = new TaxGroupDa();
                    taxGroupDa.Create(db, taxGroup);
                }
                catch
                {
                    // ignored
                }
            }

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
