using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class CompanyTest : InternalTestFakeServerBase
    {
        #region GetCompanies
        [Test]
        public void GetCompanies_MethodCalled_CompaniesReturned()
        {
            //Setup

            //Act
            var result = _client.GetAsync("Company/GetCompanies").Result;
            string json = result.Content.ReadAsStringAsync().Result;
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreNotEqual(null, companies);//Check if result returned
        }
        #endregion

        #region Read
        [Test]
        public async Task Read_CorrectID_SameObjectReturned()
        {
            //Setup
            string json = _client.GetAsync("Company/GetCompanies").Result.Content.ReadAsStringAsync().Result;
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(json);
            if (companies.Count < 1)//if no invoices create new
            {
                await _client.PostAsJsonAsync("Invoice/Create", GetInvoiceSeed());

                json = _client.GetAsync("Company/GetCompanies").Result.Content.ReadAsStringAsync().Result;
                companies = JsonConvert.DeserializeObject<List<Company>>(json);
            }

            //Act
            var result = _client.GetAsync("Company/Read?id=" + companies[0].ID).Result;
            json = result.Content.ReadAsStringAsync().Result;
            Company company = JsonConvert.DeserializeObject<Company>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreEqual(true, company.ID == companies[0].ID &&
                                      company.Address.Equals(companies[0].Address) &&
                                      company.BankNumber.Equals(companies[0].BankNumber) &&
                                      company.Location.Equals(companies[0].Location) &&
                                      company.Name.Equals(companies[0].Name) &&
                                      company.RegNumber.Equals(companies[0].RegNumber));//info of existing was updated
        }

        [Test]
        public void Read_WrongId_NullReturned()
        {
            //Setup

            //Act
            var result = _client.GetAsync("Company/Read?id=" + 0).Result;
            string json = result.Content.ReadAsStringAsync().Result;
            Company company = JsonConvert.DeserializeObject<Company>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreEqual(null, company);//check if object received is the sames
        }
        #endregion
        private static Invoice GetInvoiceSeed()
        {
            Company receiverCompany = new Company
            {
                Location = "Nibevej 2344 location",
                Name = "shufle",
                RegNumber = "54555556",
                Address = "Nibevej 2344 Address",
                BankNumber = "DK34745624547"
            };

            Representative receiver = new Representative
            {
                Name = "Kabola",
                Company = receiverCompany
            };

            Company senderCompany = new Company
            {
                Location = "Wallstreet location",
                Name = "Pop-top-spain",
                RegNumber = "1233333",
                Address = "Wallstreet Address",
                BankNumber = "DK3477777777"
            };

            Representative sender = new Representative
            {
                Name = "Kasper",
                Company = senderCompany
            };

            Invoice invoice = new Invoice
            {
                DocNumber = "136381022",
                PaymentDate = DateTime.Now,
                PrescriptionDate = DateTime.Now,
                ReceptionDate = DateTime.Now,
                Receiver = receiver,
                Sender = sender,
                Transport = 5
            };

            return invoice;
        }
    }
}
