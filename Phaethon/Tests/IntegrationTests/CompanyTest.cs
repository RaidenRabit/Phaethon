using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class CompanyTest
    {
        //InternalTestFakeServerBase
        private static HttpClient _client;

        public CompanyTest()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/");
        }

        #region GetCompany
        [Test]
        public async Task GetCompany_CorrectID_IsSuccessStatusCodeAndSameObjectReturned()
        {
            //Setup
            var response = await _client.GetAsync("Company/GetCompanies");
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(await response.Content.ReadAsStringAsync());
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = companies[0].ID.ToString();

            //Act
            response = await _client.GetAsync("Company/GetCompany?" + parameters);
            Company company = JsonConvert.DeserializeObject<Company>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, company.ID == companies[0].ID &&
                                  company.Address.Equals(companies[0].Address) &&
                                  company.BankNumber.Equals(companies[0].BankNumber) &&
                                  company.Location.Equals(companies[0].Location) &&
                                  company.Name.Equals(companies[0].Name) &&
                                  company.RegNumber.Equals(companies[0].RegNumber));//check if object received is the same
        }

        [Test]
        public async Task GetCompany_WrongId_IsSuccessStatusCodeAndNullObjectReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = 0.ToString();

            //Act
            var response = await _client.GetAsync("Company/GetCompany?" + parameters);
            Company company = JsonConvert.DeserializeObject<Company>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(null, company);
        }
        #endregion

        #region GetCompanies
        [Test]
        public async Task GetCompanies_MethodCalled_IsSuccessStatusCodeAndCompaniesReturned()
        {
            //Setup

            //Act
            var response = await _client.GetAsync("Company/GetCompanies");
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(null, companies);
        }
        #endregion
    }
}
