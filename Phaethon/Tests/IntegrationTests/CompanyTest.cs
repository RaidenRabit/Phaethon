using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class CompanyTest: InternalApiFakeServer
    {
        private bool AreCompaniesEqual(Company firstCompany, Company secondCompany)
        {
            return firstCompany.ID == secondCompany.ID &&
                   firstCompany.Address.Equals(secondCompany.Address) &&
                   firstCompany.BankNumber.Equals(secondCompany.BankNumber) &&
                   firstCompany.Location.Equals(secondCompany.Location) &&
                   firstCompany.Name.Equals(secondCompany.Name) &&
                   firstCompany.RegNumber.Equals(secondCompany.RegNumber);
        }

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
            Assert.AreEqual(true, AreCompaniesEqual(company, invoice.Sender.Company));//check if object received is the same
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
