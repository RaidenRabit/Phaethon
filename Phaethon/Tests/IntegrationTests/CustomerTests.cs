using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Core.Model.Filters;
using InternalApi.DataAccess;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class CustomerTests : InternalTestFakeServerBase
    {
        private Address _address;
        private Customer _customer;

        private void InitializeData()
        {
            _address = new Address { City = "TestCity1", Number = "21", Street = "TestStreet1" };
            _customer = new Customer { Address = _address, Email = "testEmail@email.com", FamilyName = "TestFamily", GivenName = "TestGiven", Phone = "072379899" };
        }

        [Test]
        public async Task ReadAllFullFilter_ListOfCustomers()
        {
            //Setup
            InitializeData();
            CustomerDa _customerDa = new CustomerDa();
            _customerDa.InsertOrUpdate(_customer);
            _customer.ID = 0;
            _customerDa.InsertOrUpdate(_customer);
            CustomerQueryFilter customerQueryFilter = new CustomerQueryFilter
            {
                Phone = "072379899",
                FamilyName = "TestFamily",
                GivenName = "TestGiven",
                Email = "testEmail@email.com"
            };

            //Act
            var result = await _client.PostAsJsonAsync("Customer/ReadAll", customerQueryFilter);
            string json = await result.Content.ReadAsStringAsync();
            List<Customer> jobs = JsonConvert.DeserializeObject<List<Customer>>(json);

            //Assert
            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsTrue(jobs.Count >= 2, $"Job Count: {jobs.Count}");

        }
    }
}
