using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class CompanyTest: InternalTestFakeServerBase
    {
        #region GetCompany
        [Test]
        public async Task GetCompany_CorrectID_IsSuccessStatusCodeAndSameObjectReturned()
        {
            //Setup
            Element element = InvoiceTest.GetElementSeed();
            Invoice invoice = element.Invoice;
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = invoice.Sender.Company.ID.ToString();

            //Act
            var response = await _client.GetAsync("Company/GetCompany?" + parameters);
            Company company = JsonConvert.DeserializeObject<Company>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, company.ID == invoice.Sender.Company.ID &&
                                  company.Address.Equals(invoice.Sender.Company.Address) &&
                                  company.BankNumber.Equals(invoice.Sender.Company.BankNumber) &&
                                  company.Location.Equals(invoice.Sender.Company.Location) &&
                                  company.Name.Equals(invoice.Sender.Company.Name) &&
                                  company.RegNumber.Equals(invoice.Sender.Company.RegNumber));//check if object received is the same
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
